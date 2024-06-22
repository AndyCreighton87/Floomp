using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class BuildableAreaMarker : MonoBehaviour {

    private DecalProjector decalProjector;

    private BuildableAreaData buildableAreaData;

    public BuildableAreaData GetData() {
        if (decalProjector == null) {
            decalProjector = GetComponent<DecalProjector>();
        }

        if (buildableAreaData == null) {
            buildableAreaData = new BuildableAreaData {
                width = decalProjector.size.x,
                height = decalProjector.size.z,
                position = transform.position
            };
        }

        return buildableAreaData;
    }
}

public class BuildableAreaData {
    public float width;
    public float height;
    public Vector3 position;
}
