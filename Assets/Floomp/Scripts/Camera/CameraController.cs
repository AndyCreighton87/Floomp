using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayer;

    [SerializeField] private float panSpeed = 20.0f;

    [SerializeField] private float panLimitX = 50.0f;
    [SerializeField] private float panLimitZ = 50.0f;

    [SerializeField] private float scrollSpeed = 20.0f;
    [SerializeField] private float minY = 20.0f;
    [SerializeField] private float maxY = 120.0f;

    [SerializeField] Transform groundTransform;

    private Vector3 panDirection;
    private float scrollAmount;

    void Start()
    {
        SetupGroundTransform();

        InputHandler inputHandler = InputHandler.Instance;

        // Horizontal Input
        inputHandler.OnPanHorizontal.AddListener(PanHorizontal);

        //Vertical Input
        inputHandler.OnPanVertical.AddListener(PanVertical);

        //Zoom
        inputHandler.OnZoom.AddListener(Zoom);
        inputHandler.OnZoomEnded.AddListener(ZoomEnded);
    }

    private void SetupGroundTransform() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayer)) {
            groundTransform.position = hit.point;
        }
        else {
            groundTransform.position = Vector3.zero;
        }
    }

    private void PanHorizontal(float _value) {
        panDirection.x = _value * panSpeed * Time.deltaTime;
    }

    private void PanHorizontalEnded() {
        panDirection.x = 0.0f;
    }


    private void PanVertical(float _value) {
        panDirection.z = _value * panSpeed * Time.deltaTime;
    }

    private void PanVerticalEnded() {
        panDirection.z = 0;
    }

    private void Zoom(float _value) {
        scrollAmount = _value * scrollSpeed * Time.deltaTime;
    }

    private void ZoomEnded() {
        scrollAmount = 0.0f;
    }

    private void Update() {
        if (CheckCanMove()) {
            Vector3 newPos = transform.position + panDirection;

            newPos.y -= scrollAmount;
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

            transform.position = newPos;
        }
    }

    private bool CheckCanMove() {
        Vector3 newPos = groundTransform.position + panDirection;

        if (newPos.x <= -panLimitX || newPos.x >= panLimitX) {
            return false;
        }

        if (newPos.z <= -panLimitZ || newPos.z >= panLimitZ) {
            return false;
        }

        return true;
    }
}
