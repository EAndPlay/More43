using System;
using NPC;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Dungeons
{
    public class DungeonRoom : MonoObject
    {
        // used for first enter to dung
        public GameObject playerSpawnPoint;
        public DungeonExit exitPortalSpawnPoint;

        private Dungeon _dungeon;
        
        [Header("For generation")]
        [SerializeField] private GameObject[] walls;
        [SerializeField] private GameObject[] doors;
        [SerializeField] private SpawnPoint[] spawnPoints;
        
        private void Awake()
        {
            _dungeon = gameObject.GetComponentInParent<Dungeon>();
        }

        public void Initialize(bool[] status)
        {
            for (byte i = 0; i < 4; i++)
            {
                doors[i].SetActive(status[i]);
                walls[i].SetActive(!status[i]);
            }

            foreach (var spawnPoint in spawnPoints)
            {
                var mobsCount = Random.Range(spawnPoint.MinCount, spawnPoint.MaxCount + 1);
                for (int i = 0; i < mobsCount; i++)
                {
                    const float offsetSize = 4;
                    var circleOffset = Random.insideUnitCircle * offsetSize;
                    
                    var mobObject = Instantiate(spawnPoint.MobsArray[Random.Range(0, spawnPoint.MobsArray.Length)],
                        spawnPoint.transform.position + new Vector3(circleOffset.x, 0, circleOffset.y), Quaternion.identity, _dungeon.transform);
                    mobObject.transform.SetParent(_dungeon.transform);
                    var mob = mobObject.GetComponentInChildren<Mob>() ?? mobObject.GetComponent<Mob>();
                    mobObject.transform.parent = Transform;
                    if (mob.isBoss)
                    {
                        circleOffset = 2 * offsetSize * Random.insideUnitCircle;
                        mob.transform.position += new Vector3(circleOffset.x, 0, circleOffset.y);
                    }

                    mob.spawnPoint = spawnPoint.gameObject;
                    mob.dungeon = _dungeon;
                    mob.level = Math.Max(Random.Range(_dungeon.MinLevel, _dungeon.MaxLevel), 1);
                    mob.Initialize();
                    
                    if (!mob.isBoss)
                        _dungeon.MaxMobsCount++;
                }
            }
        }
        
        // public void ConnectTo(DungeonRoom room, int roomNum)
        // {
        //     var mainT = transform;
        //     var enter = room.connections[roomNum];
        //     var enterRotation = enter.transform.rotation;
        //     mainT.rotation = new Quaternion(enterRotation.x, -enterRotation.y, enterRotation.z, 0);
        //     
        //     // TODO: find way to connect point to point of sub objects (constraints)
        // }
    }
}