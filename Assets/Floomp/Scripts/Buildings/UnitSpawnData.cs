using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSpawnData", menuName = "ScriptableObjects/UnitSpawnData")]
public class UnitSpawnData : ScriptableObject {
    public Unit[] units;
    public int numToSpawn = 3;
    public float spawnTime = 5.0f;
}