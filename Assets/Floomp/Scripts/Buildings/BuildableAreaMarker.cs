using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
[RequireComponent(typeof(BoxCollider))]
public class BuildableAreaMarker : Interactable {

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

    public override void OnInteract() {
        Debug.Log("Buildable Area Marker Interacted");
    }
}

public class BuildableAreaData {
    public float width;
    public float height;
    public Vector3 position;
}
