using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealthBar : PoolableObject
{
    // Need to bind it to a unit
    // then we need to work out screen pos based on unit pos
    // need to consider zoom - is should probably get smaller as we zoom out
    // We could also do this as 

    private INodeObject _owner;
    private Vector3 offset = new Vector3(0.0f, 1.5f, 1.5f);

    public void BindToNodeObject(INodeObject _object) {
        if (_object != null) {
            _owner = _object;
        }
    }

    private void FixedUpdate() {
        if (_owner != null) {
            Vector3 screenPos = _owner.Position + offset;
            transform.position = screenPos;
        }
    }
}
