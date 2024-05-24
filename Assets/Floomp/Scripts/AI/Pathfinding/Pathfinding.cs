using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public readonly int straightCost = 10;
    public readonly int diagonalCost = 14;

    public Transform seeker;
    public Transform target;

    private void Update() {
        FindPath(seeker.position, target.position);
    }

    private void FindPath(Vector3 _startPos, Vector3 _targetPos) {
        GridManager gridManager = GridManager.Instance;

        Node startNode = gridManager.NodeFromWorldPosition(_startPos);
        Node targetNode = gridManager.NodeFromWorldPosition(_targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbour in gridManager.GetNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }

        }
    }

    private void RetracePath(Node _startNode, Node _endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = _endNode;

        while(currentNode != _startNode) {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        GridManager.Instance.path = path;
    }

    private int GetDistance(Node _nodeA, Node _nodeB) {
        int distX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int distY = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        if (distX > distY) {
            return diagonalCost * distY + straightCost * (distX - distY);
        }

        return diagonalCost * distY + straightCost * (distY - distX);
    }
}
