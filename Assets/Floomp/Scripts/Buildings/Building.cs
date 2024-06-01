using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Team team;
    public Team Team => team;

    [SerializeField] private string id;
    public string ID => id;

    [SerializeField] private UnitSpawnData spawnData;

    [SerializeField] private Transform[] spawnPoints;

    public string UnitID => spawnData.unit.ID;

    // Num Of Units, Spawn Point Indicies
    private Dictionary<int, int[]> spawnLocationMapping = new Dictionary<int, int[]> {
        { 1, new int[] { 1 } },
        { 2, new int[]  { 0, 2 } },
        { 3, new int[] { 0, 1, 2 } }
    };

    private void Start() {
        SpawnUnit();
        StartCoroutine(StartSpawnUnits());
    }

    private IEnumerator StartSpawnUnits() {
        while (true) {
            yield return new WaitForSeconds(spawnData.spawnTime);
            SpawnUnit();
        }
    }

    private void SpawnUnit() {
        int numToSpawn = spawnData.numToSpawn;
        int[] spawnPointIndicies = spawnLocationMapping[numToSpawn];

        if (numToSpawn != spawnPointIndicies.Length) {
            Debug.LogError($"Discrepancy in num to spawn ({numToSpawn} and spawn point indicies {spawnPointIndicies}. Check mapping. No units will be spawned.");
            return;
        }

        for (int i = 0; i < numToSpawn; i++) {
            Transform spawnPoint = spawnPoints[spawnPointIndicies[i]];
            Unit unit = (Unit)PoolManager.Instance.GetObject(UnitID);
            unit.transform.position = spawnPoint.position;
            unit.transform.rotation = spawnPoint.rotation;
            unit.Init(Team);
        }

        //foreach(Transform t in spawnPoints) {
        //    string ID = spawnData.unit.ID;
        //    Unit unit = (Unit)PoolManager.Instance.GetObject(ID);
        //    unit.transform.position = t.position;
        //    unit.transform.rotation = t.rotation;
        //    unit.Init(team);
        //}
    }
}
