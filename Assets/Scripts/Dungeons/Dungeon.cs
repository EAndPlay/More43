using System;
using System.Collections.Generic;
using System.Text;
using NPC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeons
{
    public sealed class Dungeon : MonoObject
    {
        public int MinLevel;
        public int MaxLevel;

        public int MaxMobsCount;
        public int CurrentMobsCount;
        public bool isBossDead;
        
        public Vector2Int size;
        public int startPos;
        public Vector2 offset;
        public Rule[] rooms;
        
        [SerializeField] private GameObject[] bossRooms;
        [SerializeField] private GameObject dungeonEnter;
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] private Player player;

        private GameObject _exitPortal;
        public GameObject exitSpawnPoint;
        
        private List<Cell> _grid;
        private int _bossRoomId;
        
        [Serializable]
        public class Rule
        {
            public GameObject room;
            public Vector2Int minPosition;
            public Vector2Int maxPosition;

            public bool obligatory;

            public int ProbabilityOfSpawning(int x, int y)
            { 
                if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
                    return obligatory ? 2 : 1;

                return 0;
            }
        }

        private void Start()
        {
            // MazeGenerator();
            // CurrentMobsCount = MaxMobsCount;
        }

        public void Create()
        {
            MaxMobsCount = 0;
            isBossDead = false;
            MazeGenerator();
            CurrentMobsCount = MaxMobsCount;
        }

        public void Reset()
        {
            for (int i = 0; i < Transform.childCount; i++)
            {
                Destroy(Transform.GetChild(i).gameObject);
            }
        }

        public void RegisterMobDeath(Mob mob)
        {
            if (mob.isBoss)
                isBossDead = true;
            else
                CurrentMobsCount--;

            MobDied?.Invoke(mob);

            if (CurrentMobsCount == 0 && isBossDead)
                OnComplete();
        }

        private void OnComplete()
        {
            _exitPortal.SetActive(true);
            player.GiveExperience(Random.Range(MinLevel, MaxLevel + 1) * 100);

            MinLevel = player.level;
            MaxLevel = player.level + Random.Range(1, 3);
        }

        public event Action<Mob> MobDied;

        private void GenerateDungeon()
        {
            for (var i = 0; i < size.x; i++)
            for (var j = 0; j < size.y; j++)
            {
                var roomId = i + j * size.x;
                var currentCell = _grid[roomId];

                var roomPosition = new Vector3(i * offset.x, 0, -j * offset.y);
                if (_bossRoomId == roomId)
                {
                    goto BossRoom;
                }
                if (!currentCell.Visited) continue;
                
                var randomRoom = -1;
                var availableRooms = new List<int>();

                for (var k = 0; k < rooms.Length; k++)
                {
                    switch (rooms[k].ProbabilityOfSpawning(i, j))
                    {
                        case 1:
                            availableRooms.Add(k);
                            break;
                        case 2:
                            randomRoom = k;
                            break;
                    }
                }
                
                if (randomRoom == -1)
                {
                    randomRoom = availableRooms.Count > 0 ? availableRooms[Random.Range(0, availableRooms.Count)] : 0;
                }

                goto CommonRoom;
                
                DungeonRoom newRoom;
                
                BossRoom: 
                newRoom = Instantiate(bossRooms[Random.Range(0, bossRooms.Length)], roomPosition,
                    Quaternion.identity, transform).GetComponent<DungeonRoom>();
                
                _exitPortal = newRoom.exitPortalSpawnPoint.gameObject;
                _exitPortal.SetActive(false);
                newRoom.exitPortalSpawnPoint.exitSpawnPoint = exitSpawnPoint;
                //newRoom.GetComponentInChildren<DungeonExit>().exitSpawnPoint = exitPortal
                goto RoomInitialize;
                
                CommonRoom:
                newRoom = Instantiate(rooms[randomRoom].room, roomPosition,
                    Quaternion.identity, transform).GetComponent<DungeonRoom>();
                if (i == 0 && j == 0)
                {
                    spawnPoint = newRoom.playerSpawnPoint;
                }
                
                RoomInitialize:
                newRoom.Initialize(currentCell.Status);
                newRoom.name = new StringBuilder(" ").Append(i).Append("-").Append(j).ToString();
            }
        }

        private void MazeGenerator()
        {
            _grid = new List<Cell>();

            for (var i = 0; i < size.x; i++)
            for (var j = 0; j < size.y; j++)
                _grid.Add(new Cell());

            var currentCell = startPos;

            var path = new Stack<int>();

            var k = 0;

            while (k < size.x * size.y)
            {
                k++;

                _grid[currentCell].Visited = true;

                if (currentCell == _grid.Count - 1) break;

                var neighbors = CheckNeighbors(currentCell);

                if (neighbors.Count == 0)
                {
                    if (path.Count == 0)
                        break;
                    currentCell = path.Pop();
                }
                else
                {
                    path.Push(currentCell);
                    var newCell = neighbors[Random.Range(0, neighbors.Count)];

                    if (newCell > currentCell)
                    {
                        //down or right
                        if (newCell - 1 == currentCell)
                        {
                            _grid[currentCell].Status[2] = true;
                            currentCell = newCell;
                            _grid[currentCell].Status[3] = true;
                        }
                        else
                        {
                            _grid[currentCell].Status[1] = true;
                            currentCell = newCell;
                            _grid[currentCell].Status[0] = true;
                        }
                    }
                    else
                    {
                        //up or left
                        if (newCell + 1 == currentCell)
                        {
                            _grid[currentCell].Status[3] = true;
                            currentCell = newCell;
                            _grid[currentCell].Status[2] = true;
                        }
                        else
                        {
                            _grid[currentCell].Status[0] = true;
                            currentCell = newCell;
                            _grid[currentCell].Status[1] = true;
                        }
                    }

                    _bossRoomId = currentCell;
                }
            }

            GenerateDungeon();
        }

        private List<int> CheckNeighbors(int cell)
        {
            var neighbors = new List<int>();

            // up
            if (cell - size.x >= 0 && !_grid[cell - size.x].Visited) neighbors.Add(cell - size.x);

            // down
            if (cell + size.x < _grid.Count && !_grid[cell + size.x].Visited) neighbors.Add(cell + size.x);

            // right
            if ((cell + 1) % size.x != 0 && !_grid[cell + 1].Visited) neighbors.Add(cell + 1);

            // left
            if (cell % size.x != 0 && !_grid[cell - 1].Visited) neighbors.Add(cell - 1);

            return neighbors;
        }

        public Vector3 GetEnterPosition() => spawnPoint.transform.position;

        private class Cell
        {
            public bool Visited;
            public readonly bool[] Status = new bool[4];
        }
    }
}