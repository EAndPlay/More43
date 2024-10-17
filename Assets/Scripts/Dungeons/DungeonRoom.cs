using UnityEngine;

namespace Dungeons
{
    public class DungeonRoom : MonoBehaviour
    {
        [SerializeField] private GameObject spawnPoint;
        /// coordinates (pos + rotation) to connect/spawn other rooms.
        /// must be size of 4
        [SerializeField] private GameObject[] roomsConnections;

        public void ConnectTo(DungeonRoom room, int roomNum)
        {
            var mainT = transform;
            var enter = room.roomsConnections[roomNum];
            var enterRotation = enter.transform.rotation;
            mainT.rotation = new Quaternion(enterRotation.x, -enterRotation.y, enterRotation.z, 0);
            
            // TODO: find way to connect point to point of sub objects (constraints)
        }
    }
}