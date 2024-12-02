using System;
using UnityEngine;

namespace Dungeons
{
    public class DungeonEntrance : MonoBehaviour
    {
        public Dungeon linkedDungeon;

        public GameObject exitSpawnPoint;
        
        private void OnTriggerEnter(Collider other)
        {
            Player player;
            if (!(player = other.GetComponent<Player>())) return;
            linkedDungeon.exitSpawnPoint = exitSpawnPoint;
            player.EnterDungeon(linkedDungeon);
            
            player.Character.transform.position = linkedDungeon.GetEnterPosition();
        }
    }
}