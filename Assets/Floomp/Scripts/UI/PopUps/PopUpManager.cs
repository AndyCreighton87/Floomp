using System.Collections;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance;

    [SerializeField] private RectTransform popUpTransform;

    private PopUp currentPopUp;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ShowPopUp(string _popUpName, object _data = null) {
        HidePopUp();
        StartCoroutine(ShowPopUpAsync(_popUpName, _data));
    }

    public void HidePopUp() {
        if (currentPopUp != null) {
            currentPopUp.OnHide();
            Destroy(currentPopUp.gameObject);
        }
    }

    private IEnumerator ShowPopUpAsync(string _popUpName, object _data = null) {
        ResourceRequest request = Resources.LoadAsync<PopUp>(_popUpName);
        while (!request.isDone) {
            yield return null;
        }

        currentPopUp = Instantiate(request.asset as PopUp, popUpTransform);
        currentPopUp.OnShow(_data);

        // Have to ensure instanitated pop-ups are on the same layer as it's parent so that the input will work as intended
        var layer = gameObject.layer;

        foreach (Transform child in currentPopUp.transform) {
            child.gameObject.layer = layer;
        }
    }
}
