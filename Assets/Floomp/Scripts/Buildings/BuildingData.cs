using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Floomp/BuildingData")]

public class BuildingData : ScriptableObject
{
    public string buildingID;
    public int buildCost;
    public Sprite uiSprite;
    public Building buildingPrefab;
}
