using UnityEngine;

[CreateAssetMenu(fileName = "UnitSpawnData", menuName = "ScriptableObjects/UnitSpawnData")]

public class BuildingData : ScriptableObject
{
    public string buildingID;
    public int buildCost;
    public Sprite uiSprite;
    public Building buildingPrefab;
}
