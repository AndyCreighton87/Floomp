using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PopUp : MonoBehaviour
{
    public object data;
    public virtual void OnShow(object _data = null) { }

    public virtual void OnHide() { }
}
