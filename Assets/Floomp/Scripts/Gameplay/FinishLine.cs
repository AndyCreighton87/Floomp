using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FinishLine : MonoBehaviour
{
    public Action<Unit> OnUnitEntered;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(StringLibrary.ScorableUnitTag)) {
            Unit unit = other.GetComponent<Unit>();
            if (unit != null) {
                OnUnitEntered(unit);
            }
        }
    }
}
