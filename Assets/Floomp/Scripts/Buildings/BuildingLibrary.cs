using System.Collections.Generic;
using UnityEngine;

public class BuildingLibrary : MonoBehaviour
{
    public static BuildingLibrary Instance;

    [SerializeField] private List<BuildingData> buildingDataList = new List<BuildingData>();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public List<BuildingData> GetAllBuildingData() => buildingDataList;

    public BuildingData GetBuildingData(string buildingID) {
        foreach (BuildingData buildingData in buildingDataList) {
            if (buildingData.buildingID == buildingID) {
                return buildingData;
            }
        }

        Debug.LogError($"No building data found for building ID: {buildingID}");
        return null;
    }
}
