using System;
using Dungeons;
using UnityEngine;
using UnityEngine.Serialization;

public class Location : MonoBehaviour
{
    public new string name;

    public GameObject spawnPoint;

    public Dungeon[] dungeons;
    
    private void Awake()
    {
        // var dungeon = new GameObject().AddComponent<Dungeon>();
        // dungeon.Create();

    }
}