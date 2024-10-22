using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeons
{
    public sealed class Dungeon : MonoBehaviour
    {
        public int MinLevel;
        public int MaxLevel;

        [SerializeField] private GameObject dungeonEnter;
        [SerializeField] private DungeonRoom[] rooms1;

        [SerializeField] private GameObject spawnPoint;

        [Serializable]
        public class Rule
        {
            public GameObject room;
            public Vector2Int minPosition;
            public Vector2Int maxPosition;

            public bool obligatory;

            public int ProbabilityOfSpawning(int x, int y)
            {
                // TODO: make chest rooms with it
                // TODO: !!!!! make exit mb with it
                // 0 - cannot spawn 1 - can spawn 2 - HAS to spawn

                if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
                    return obligatory ? 2 : 1;

                return 0;
            }
        }

        private void Awake()
        {
            MazeGenerator();
        }

        public Vector2Int size;
        public int startPos = 0;
        public Rule[] rooms;
        public Vector2 offset;

        private List<Cell> _grid;

        private void GenerateDungeon()
        {
            for (var i = 0; i < size.x; i++)
            for (var j = 0; j < size.y; j++)
            {
                var currentCell = _grid[i + j * size.x];
                if (!currentCell.Visited) continue;
                
                var randomRoom = -1;
                var availableRooms = new List<int>();

                for (var k = 0; k < rooms.Length; k++)
                    switch (rooms[k].ProbabilityOfSpawning(i, j))
                    {
                        case 1:
                            availableRooms.Add(k);
                            break;
                        case 2:
                            randomRoom = k;
                            break;
                    }

                if (randomRoom == -1)
                {
                    randomRoom = availableRooms.Count > 0 ? availableRooms[Random.Range(0, availableRooms.Count)] : 0;
                }


                var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y),
                    Quaternion.identity, transform).GetComponent<DungeonRoom>();
                if (i == 0 && j == 0)
                {
                    spawnPoint = newRoom.spawnPoint;
                    Debug.Log($"SpawnPoint set");
                }
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

                //Check the cell's neighbors
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

        // mb add params?
        public void Create()
        {
            var roomsCount = 1; //Random.Range(3, 6);
            Array.Resize(ref rooms1, roomsCount);

            var sb = new StringBuilder();
            const string dungeonPath = "DungeonRooms/DungRoom";
            for (var i = 0; i < roomsCount; i++)
            {
                sb.Insert(dungeonPath.Length, i);
                var dungObj = (GameObject)Instantiate(Resources.Load(sb.ToString(), typeof(GameObject)));

                var dungTransform = dungObj.transform;
                //dungTransform.position = 
                // TODO: dungObj.position/rotation set to free DungeonRoom.roomConnection
            }
        }

        public Vector3 GetEnterPosition() => spawnPoint.transform.position;

        private class Cell
        {
            public bool Visited = false;
            public readonly bool[] Status = new bool[4];
        }
    }
}