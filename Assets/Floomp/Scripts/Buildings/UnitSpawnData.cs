using UnityEngine;

[CreateAssetMenu(fileName = "UnitSpawnData", menuName = "Floomp/UnitSpawnData")]
public class UnitSpawnData : ScriptableObject {
    public Unit unit;
    public int numToSpawn = 3;
    public float spawnTime = 5.0f;
}