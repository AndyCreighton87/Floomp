using UnityEngine;

public class BuildablePopUp : WorldPopUp {
    [SerializeField] private BuildingDataEntry buildingDataEntryPrefab;
    [SerializeField] private Transform entryTransform;

    public override void OnShow(object _data = null) {
        base.OnShow(_data);

        var allBuildingData = BuildingLibrary.Instance.GetAllBuildingData();

        foreach (BuildingData building in allBuildingData) {
            if (building != null) {
                BuildingDataEntry entry = Instantiate(buildingDataEntryPrefab, entryTransform);
                entry.SetEntry(building.name, building.buildCost, building.uiSprite, null);
                entry.Button.onClick.RemoveAllListeners();
                entry.Button.onClick.AddListener(Test);
            }
        }
    }

    private void Test() {
        Debug.Log("Test");
    }

    public void OnBuildingSelected(BuildingData _buildingData) {
        Debug.Log($"Building Selected: {_buildingData.name}");
    }
}
