using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public readonly int straightCost = 10;
    public readonly int diagonalCost = 14;

    public void FindPath(PathRequest _request, Action<PathResult> _callback) {
        GridManager gridManager = GridManager.Instance;

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = gridManager.NodeFromWorldPosition(_request.pathStart);
        Node targetNode = gridManager.NodeFromWorldPosition(_request.pathEnd);

        if (startNode.walkable && targetNode.walkable) {

            Heap<Node> openSet = new Heap<Node>(gridManager.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in gridManager.GetNeighbours(currentNode)) {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour)) {
                            openSet.Add(neighbour);
                        }
                        else {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }

        _callback(new PathResult(waypoints, pathSuccess, _request.callback, _request.requestID));
    }

    private Vector3[] RetracePath(Node _startNode, Node _endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = _endNode;

        while(currentNode != _startNode) {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> _path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < _path.Count; i++) {
            Vector2 directionNew = new Vector2(
                _path[i - 1].gridX - _path[i].gridX,
                _path[i - 1].gridY - _path[i].gridY);

            if (directionNew != directionOld) {
                waypoints.Add(_path[i].worldPos);
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    private int GetDistance(Node _nodeA, Node _nodeB) {
        int distX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int distY = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        if (distX > distY) {
            return diagonalCost * distY + straightCost * (distX - distY);
        }

        return diagonalCost * distX + straightCost * (distY - distX);
    }
}
