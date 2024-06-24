using UnityEngine;

public class WorldPopUp : PopUp
{
    private Camera mainCamera;

    public override void OnShow(object _data = null) {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        Vector3 worldPosition = InputHandler.Instance.mouseClickPosition;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        RectTransform rectTransform = GetComponent<RectTransform>();

        float yOffset = rectTransform.sizeDelta.y / 2;
        screenPosition.y += yOffset;
        transform.position = screenPosition;
    }

    public override void OnHide() { 
    }
}
