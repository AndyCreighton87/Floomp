using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;


    [Header("Layer Masks")]
    [SerializeField] private LayerMask interactableLayer;

    public InputAction leftClick { get; private set; }


    [HideInInspector] public UnityEvent<float> OnPanHorizontal = new UnityEvent<float>();

    [HideInInspector] public UnityEvent<float> OnPanVertical = new UnityEvent<float>();

    [HideInInspector] public UnityEvent<float> OnZoom = new UnityEvent<float>();
    [HideInInspector] public UnityEvent OnZoomEnded = new UnityEvent();

    public Vector3 mouseClickPosition { get; private set; }

    private float lastZoom = 0;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        HandlePan();
        HandleZoom();
    }

    private void HandlePan() {
        float panHorizontal = Input.GetAxis("Horizontal");
        float panVertical = Input.GetAxis("Vertical");

        if (panHorizontal != 0) {
            OnPanHorizontal.Invoke(panHorizontal);
        }

        if (panVertical != 0) {
            OnPanVertical.Invoke(panVertical);
        }
    }

    private void HandleZoom() {
        float zoom = Input.GetAxis("Mouse ScrollWheel");

        if (lastZoom != 0 && zoom == 0) {
            OnZoomEnded.Invoke();
        }

        if (zoom != 0) {
            lastZoom = zoom;
            OnZoom.Invoke(zoom);
        }
    }

    private void OnClick(InputAction.CallbackContext context) {
        if (!IsPointerOverUI()) {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            mouseClickPosition = ray.origin;
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer)) {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null) {
                    interactable.OnInteract();
                }
            }
        }

        bool IsPointerOverUI() {
            PointerEventData eventData = new PointerEventData(EventSystem.current) {
                position = Mouse.current.position.ReadValue()
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }

}
