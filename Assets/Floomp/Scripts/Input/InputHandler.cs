using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;


    [Header("Layer Masks")]
    [SerializeField] private LayerMask interactableLayer;


    private GraphicRaycaster[] raycasters;

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

    private void Start() {
        raycasters = FindObjectsOfType<GraphicRaycaster>();
    }

    private void Update() {
        HandlePan();
        HandleZoom();
        HandleMouseClick();
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

    private void HandleMouseClick() {
        mouseClickPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI()) {
            Ray ray = Camera.main.ScreenPointToRay(mouseClickPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer)) {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null) {
                    interactable.OnInteract();
                }
            }
        }

        bool IsPointerOverUI() {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mouseClickPosition;

            foreach (var raycaster in raycasters) {
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(eventData, results);

                if (results.Count > 0) {
                    return true;
                }
            }

            return false;
        }
    }
}
