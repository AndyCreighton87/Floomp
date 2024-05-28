using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float panSpeed = 20.0f;

    [SerializeField] private float panLimitX = 50.0f;
    [SerializeField] private float panLimitZ = 50.0f;

    [SerializeField] private float scrollSpeed = 20.0f;
    [SerializeField] private float minY = 20.0f;
    [SerializeField] private float maxY = 120.0f;

    private Vector3 panDirection;
    private float scrollAmount;

    // Start is called before the first frame update
    void Start()
    {
        InputHandler inputHandler = InputHandler.Instance;

        // Horizontal Input
        inputHandler.OnPanHorizontal.AddListener(PanHorizontal);
        inputHandler.OnPanHorizontalEnded.AddListener(PanHorizontalEnded);

        //Vertical Input
        inputHandler.OnPanVertical.AddListener(PanVertical);
        inputHandler.OnPanVerticalEnded.AddListener(PanVerticalEnded);

        //Zoom
        inputHandler.OnZoom.AddListener(Zoom);
        inputHandler.OnZoomEnded.AddListener(ZoomEnded);

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
        Vector3 newPos = transform.position + panDirection;

        newPos.x = Mathf.Clamp(newPos.x, -panLimitX, panLimitX);
        newPos.z = Mathf.Clamp(newPos.z, -panLimitZ, panLimitZ);

        newPos.y -= scrollAmount;
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = newPos;
    }
}
