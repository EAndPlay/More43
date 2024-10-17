using System;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeons
{
    public class Dungeon : MonoBehaviour
    {
        [SerializeField] private DungeonRoom[] rooms;

        // TODO: create grid for dungs positioning
        
        public void Create()
        {
            var roomsCount = Random.Range(3, 6);
            Array.Resize(ref rooms, roomsCount);

            var sb = new StringBuilder();
            const string dungeonPath = "DungeonRooms/Dungeon";
            for (var i = 0; i < roomsCount; i++)
            {
                sb.Insert(dungeonPath.Length, i);
                var dungObj = (GameObject) Instantiate(Resources.Load(sb.ToString(), typeof(GameObject)));
                
                // TODO: dungObj.position/rotation set to free DungeonRoom.roomConnection
            }
        }
    }
}