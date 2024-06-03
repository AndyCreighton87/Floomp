using UnityEngine;

public class FreeCamera : MonoBehaviour {
    public float movementSpeed = 10.0f;
    public float lookSpeed = 2.0f;
    public float zoomSpeed = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // Handle mouse look
        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // Handle movement
        float moveSpeed = movementSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * moveSpeed);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * moveSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * moveSpeed);
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(Vector3.down * moveSpeed);
        if (Input.GetKey(KeyCode.E))
            transform.Translate(Vector3.up * moveSpeed);

        // Handle zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * zoomSpeed);
    }

    void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
    }
}