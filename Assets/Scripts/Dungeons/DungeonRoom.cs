using UnityEngine;

namespace Dungeons
{
    public class DungeonRoom : MonoBehaviour
    {
        // used for first enter to dung
        [SerializeField] private GameObject spawnPoint;

        [Header("For generation")]
        [SerializeField] private GameObject[] walls;
        [SerializeField] private GameObject[] doors;
        
        private Dungeon _dungeon;
        
        private void Start()
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