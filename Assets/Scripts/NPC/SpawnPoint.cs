using System;
using UnityEngine;

namespace NPC
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        private void Awake()
        {
            Instantiate(target, transform.position, Quaternion.identity);
            target.GetComponent<Mob>().spawnPoint = gameObject;
        }
    }
}