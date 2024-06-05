using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class BuildableAreaMarker : MonoBehaviour {

    private DecalProjector decalProjector;

    private void Start() {
        decalProjector = GetComponent<DecalProjector>();

        Vector2Int areaSize = GridManager.Instance.BuildableAreaSize;
        Vector3 newSize = new Vector3(areaSize.x, decalProjector.size.y, areaSize.y);
        decalProjector.size = newSize;
    }
}
