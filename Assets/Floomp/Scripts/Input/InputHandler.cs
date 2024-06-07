using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    [SerializeField] private InputActionAsset inputActionMap;

    public InputAction panHorizontal { get; private set; }
    public InputAction panVertical { get; private set; }
    public InputAction zoom { get; private set; }


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

        panHorizontal.started += (InputAction.CallbackContext context) => OnPanHorizontal.Invoke(context.ReadValue<float>());
        panHorizontal.canceled += (InputAction.CallbackContext context) => OnPanHorizontalEnded.Invoke();

        panVertical.started += (InputAction.CallbackContext context) => OnPanVertical.Invoke(context.ReadValue<float>());
        panVertical.canceled += (InputAction.CallbackContext context) => OnPanVerticalEnded.Invoke();

        zoom.started += (InputAction.CallbackContext context) => OnZoom.Invoke(context.ReadValue<float>());
        zoom.canceled += (InputAction.CallbackContext context) => OnZoomEnded.Invoke();
    }
}
