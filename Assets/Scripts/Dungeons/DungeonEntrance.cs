using System;
using UnityEngine;

namespace Dungeons
{
    public class DungeonEntrance : MonoBehaviour
    {
        public Dungeon linkedDungeon;

        private void OnTriggerEnter(Collider other)
        {
            Character character;
            if (!(character = other.GetComponent<Character>())) return;

            // TODO: lvl/smth check

            character.transform.position = linkedDungeon.GetEnterPosition();
        }
    }
}