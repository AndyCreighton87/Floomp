using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Team team;
    public Team Team => team;

    [SerializeField] private string id;
    public string ID => id;

    [SerializeField] private UnitSpawnData spawnData;

    [SerializeField] private Transform[] spawnPoints;

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
        foreach(Transform t in spawnPoints) {
            string ID = spawnData.units[0].ID;
            Unit unit = (Unit)PoolManager.Instance.GetObject(ID);
            unit.transform.position = t.position;
            unit.transform.rotation = t.rotation;
            unit.Init(team);
        }
    }
}
