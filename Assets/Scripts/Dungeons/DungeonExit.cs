using UnityEngine;

namespace Dungeons
{
    public class DungeonExit : MonoBehaviour
    {
        public GameObject exitSpawnPoint;
        
        private void OnTriggerEnter(Collider other)
        {
            Player player;
            if (!(player = other.GetComponent<Player>())) return;

            player.character.transform.position = exitSpawnPoint.transform.position;
            player.LeaveDungeon();
        }
    }
}