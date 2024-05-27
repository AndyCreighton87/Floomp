using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Debug")]
    [SerializeField] private bool displayGridGizmos;

    [Header("Terrain")]
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private TerrainType[] walkableRegions;

    [Header("Parameters")]
    [SerializeField] private int obstacleProximityPenalty = 10;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;

    private LayerMask walkableMask;
    private Dictionary<int, int> walkableRegionsDict = new Dictionary<int, int>();
    public Node[,] grid { get; private set; }

    private float nodeDiameter;

    private int gridSizeX;
    private int gridSizeY;
    public int MaxSize => gridSizeX * gridSizeY;

    private int penaltyMin = int.MaxValue;
    private int penaltyMax = int.MinValue;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach(TerrainType region in walkableRegions) {
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDict.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    private void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 WorldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = WorldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;

                Ray ray = new Ray(worldPoint + Vector3.up * 50.0f, Vector3.down);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 100.0f, walkableMask)){
                    walkableRegionsDict.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                if (!walkable) {
                    movementPenalty += obstacleProximityPenalty;
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(3);
    }

    public Node NodeFromWorldPosition(Vector3 _worldPosition) {
        float percentX = (_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (_worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    private void BlurPenaltyMap(int _blurSize) {
        int kernelSize = _blurSize * 2 + 1;
        int kernalExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++) {
            for (int x = -kernalExtents; x <= kernalExtents; x++) {
                int sampleX = Mathf.Clamp(x, 0, kernalExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++) {
                int removeIndex = Mathf.Clamp(x - kernalExtents - 1, 0 , gridSizeX);
                int addIndex = Mathf.Clamp(x + kernalExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x,y] = penaltiesHorizontalPass[x-1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = -kernalExtents; y <= kernalExtents; y++) {
                int sampleY = Mathf.Clamp(y, 0, kernalExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++) {
                int removeIndex = Mathf.Clamp(y - kernalExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernalExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];

                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax) {
                    penaltyMax = blurredPenalty;
                }

                if (blurredPenalty < penaltyMin) {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node _node) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {

                if (x == 0 && y == 0) {
                    continue;
                }

                int checkX = _node.gridX + x;
                int checkY = _node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX &&
                    checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public void AddObjectToNode(Vector3 _worldPosition, INodeObject _object) => AddObjectToNode(NodeFromWorldPosition(_worldPosition), _object);
    public void RemoveObjectFromNode(Vector3 _worldPosition, INodeObject _object) => RemoveObjectFromNode(NodeFromWorldPosition(_worldPosition), _object);

    public void AddObjectToNode(Node _node, INodeObject _object) {
        if (!_node.objects.Contains(_object)) {
            _node.objects.Add(_object);
        }
    }

    public void RemoveObjectFromNode(Node _node, INodeObject _object) {
        if (_node.objects.Contains(_object)) {
            _node.objects.Remove(_object);
        }
    }

    public List<INodeObject> GetObjectsInSurroundingNode(Node _node, int _range) {
        List<INodeObject> nodeObjects = new List<INodeObject>();

        for (int x = -_range; x <= _range; x++) {
            for (int y = -_range; y <= _range; y++) {
                int checkX = _node.gridX + x;
                int checkY = _node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX &&
                    checkY >= 0 && checkY < gridSizeY) {
                    nodeObjects.AddRange(grid[checkX, checkY].objects);
                }
            }
        }

        NodeObjectComparer comparer = new NodeObjectComparer(_node);
        nodeObjects.Sort(comparer);
        return nodeObjects;
    }

    #region GIZMOS
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1.0f, gridWorldSize.y));

        if (grid != null && displayGridGizmos) {
            foreach (Node node in grid) {

                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, node.movementPenalty));

                Gizmos.color = (node.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * nodeDiameter);
            }
        }
    }

    #endregion


    [Serializable]
    public class TerrainType {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
