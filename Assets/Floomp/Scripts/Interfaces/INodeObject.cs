using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeObject
{
    public Vector3 Position { get; }

    public Transform Transform { get; }
}
