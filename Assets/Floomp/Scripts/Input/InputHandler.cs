using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    [SerializeField] private InputActionAsset inputActionMap;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask interactableLayer;

    public InputAction panHorizontal { get; private set; }
    public InputAction panVertical { get; private set; }
    public InputAction zoom { get; private set; }

    public InputAction leftClick { get; private set; }


    [HideInInspector] public UnityEvent<float> OnPanHorizontal = new UnityEvent<float>();
    [HideInInspector] public UnityEvent OnPanHorizontalEnded = new UnityEvent();

    [HideInInspector] public UnityEvent<float> OnPanVertical = new UnityEvent<float>();
    [HideInInspector] public UnityEvent OnPanVerticalEnded = new UnityEvent();

    [HideInInspector] public UnityEvent<float> OnZoom = new UnityEvent<float>();
    [HideInInspector] public UnityEvent OnZoomEnded = new UnityEvent();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        panHorizontal = inputActionMap.FindAction(StringLibrary.PanHorizontal);
        panVertical = inputActionMap.FindAction(StringLibrary.PanVertical);
        zoom = inputActionMap.FindAction(StringLibrary.Zoom);
        leftClick = inputActionMap.FindAction(StringLibrary.LeftClick);

        panHorizontal.started += (InputAction.CallbackContext context) => OnPanHorizontal.Invoke(context.ReadValue<float>());
        panHorizontal.canceled += (InputAction.CallbackContext context) => OnPanHorizontalEnded.Invoke();

        panVertical.started += (InputAction.CallbackContext context) => OnPanVertical.Invoke(context.ReadValue<float>());
        panVertical.canceled += (InputAction.CallbackContext context) => OnPanVerticalEnded.Invoke();

        zoom.started += (InputAction.CallbackContext context) => OnZoom.Invoke(context.ReadValue<float>());
        zoom.canceled += (InputAction.CallbackContext context) => OnZoomEnded.Invoke();

        leftClick.started += OnClick;
    }

    private void OnClick(InputAction.CallbackContext context) {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI) {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer)) {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null) {
                    interactable.OnInteract();
                }
            }
        }
    }

}
