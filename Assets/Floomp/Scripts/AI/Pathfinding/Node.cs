using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public Node Parent;
    
    public int gridX;
    public int gridY;

    public int movementPenalty;

    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;

    private int heapIndex;

    public List<INodeObject> objects;

    public bool isBuildable = false;

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty) {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;

        objects = new List<INodeObject>();
    }

    public int CompareTo(Node _nodeToCompare) {
        int compare = fCost.CompareTo(_nodeToCompare.fCost);

        if (compare == 0) {
            compare = hCost.CompareTo(_nodeToCompare.hCost);
        }

        return -compare;
    }
}

public class NodeObjectComparer : IComparer<INodeObject> {
    private readonly Node currentNode;

    public NodeObjectComparer(Node _currentNode) {
        currentNode = _currentNode;
    }

    public int Compare(INodeObject _obj1, INodeObject _obj2) {
        float distance1 = Vector3.SqrMagnitude(_obj1.Position - currentNode.worldPos);
        float distance2 = Vector3.SqrMagnitude(_obj2.Position - currentNode.worldPos);
        return distance1.CompareTo(distance2);
    }
}
