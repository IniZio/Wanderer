using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.RandomMap {
    public class Maze : MonoBehaviour {
        // Start is called before the first frame update
        void Start () {

        }

        // Update is called once per frame
        void Update () {

        }
        // public int sizeX, sizeZ;

        public MazeRoomSettings[] roomSettings;

        public MazeCell cellPrefab, firstCellPrefab, goalCellPrefab;

        public float generationStepDelay;

        private MazeCell[, ] cells;

        public IntVector2 size;

        public bool genGoal = false;

        public MazePassage passagePrefab;

        public MazeWall[] wallPrefabs;

        public MazeCell GetCell (IntVector2 coordinates) {
            return cells[coordinates.x, coordinates.z];
        }

        public void Generate (RandomMap.GameManager gm) {
            cells = new MazeCell[size.x, size.z];
            List<MazeCell> activeCells = new List<MazeCell> ();
            DoFirstGenerationStep (activeCells);
            while (activeCells.Count > 0) {
                DoNextGenerationStep (activeCells);
            }
            gm.isDone();
        }

        private MazeCell CreateCell (IntVector2 coordinates, bool isFirst, bool isGoal) {
            MazeCell newCell;
            if (isFirst) {
                newCell = Instantiate (firstCellPrefab) as MazeCell;
            }
            else if (isGoal) {
                newCell = Instantiate (goalCellPrefab) as MazeCell;
            }
            else {
                newCell = Instantiate (cellPrefab) as MazeCell;
            }
            cells[coordinates.x, coordinates.z] = newCell;
            newCell.coordinates = coordinates;
            newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
            newCell.transform.parent = transform;
            newCell.transform.localPosition =
                // new Vector3(coordinates.x - size.x * 5.8f + 0.5f, 0f, coordinates.z - size.z * 5.8f + 0.5f);
                //new Vector3 ((coordinates.x) * 5.8f + 0.5f, 0f, (coordinates.z) * 5.8f + 0.5f);
                new Vector3 ((coordinates.x - size.x * 0.5f) * 5.8f + 0.5f, 0f, (coordinates.z - size.z * 0.5f) * 5.8f + 0.5f);

            Debug.Log (coordinates.x.ToString ());
            Debug.Log (coordinates.z.ToString ());
            Debug.Log (size.x.ToString ());
            Debug.Log (size.z.ToString ());

            Debug.Log (newCell.transform.localPosition.ToString ());
            Debug.Log (transform.ToString ());
            //  new Vector3 (coordinates.x - size.x+500, 0, coordinates.z - size.z+500);
            return newCell;

        }
        public IntVector2 RandomCoordinates {
            get {
                return new IntVector2 (Random.Range (0, size.x), Random.Range (0, size.z));
            }
        }

        public bool ContainsCoordinates (IntVector2 coordinate) {
            return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
        }

        private void DoFirstGenerationStep (List<MazeCell> activeCells) {
            MazeCell newCell = CreateCell (RandomCoordinates, true, false);
            newCell.Initialize (CreateRoom (-1));
            activeCells.Add (newCell);
        }

        private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
            MazePassage passage = Instantiate (passagePrefab) as MazePassage;
            passage.Initialize (cell, otherCell, direction);
            passage = Instantiate (passagePrefab) as MazePassage;
            passage.Initialize (otherCell, cell, direction.GetOpposite ());
            if (cell.room != otherCell.room) {
                MazeRoom roomToAssimilate = otherCell.room;
                cell.room.Assimilate (roomToAssimilate);
                rooms.Remove (roomToAssimilate);
                Destroy (roomToAssimilate);
            }
        }

        private void DoNextGenerationStep (List<MazeCell> activeCells) {
            int currentIndex = activeCells.Count - 1;
            MazeCell currentCell = activeCells[currentIndex];
            if (currentCell.IsFullyInitialized) {
                activeCells.RemoveAt (currentIndex);
                return;
            }
            MazeDirection direction = currentCell.RandomUninitializedDirection;
            IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2 ();
            if (ContainsCoordinates (coordinates)) {
                MazeCell neighbor = GetCell (coordinates);
                if (neighbor == null) {
                    neighbor = CreateCell (coordinates, false, currentIndex == 0);
                    CreatePassage (currentCell, neighbor, direction);
                    activeCells.Add (neighbor);
                } else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex) {
                    CreatePassageInSameRoom (currentCell, neighbor, direction);
                } else {
                    CreateWall (currentCell, neighbor, direction);
                    // No longer remove the cell here.
                }
            } else {
                CreateWall (currentCell, null, direction);
                // No longer remove the cell here.
            }
        }

        private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
            MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
            MazePassage passage = Instantiate (prefab) as MazePassage;
            passage.Initialize (cell, otherCell, direction);
            passage = Instantiate (prefab) as MazePassage;
            if (passage is MazeDoor) {
                otherCell.Initialize (CreateRoom (cell.room.settingsIndex));
            } else {
                otherCell.Initialize (cell.room);
            }
            passage.Initialize (otherCell, cell, direction.GetOpposite ());
        }

        private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
            MazeWall wall = Instantiate (wallPrefabs[Random.Range (0, wallPrefabs.Length)]) as MazeWall;
            wall.Initialize (cell, otherCell, direction);
            if (otherCell != null) {
                wall = Instantiate (wallPrefabs[Random.Range (0, wallPrefabs.Length)]) as MazeWall;
                wall.Initialize (otherCell, cell, direction.GetOpposite ());
            }
        }

        public MazeDoor doorPrefab;

        [Range (0f, 1f)]

        public float doorProbability;

        private List<MazeRoom> rooms = new List<MazeRoom> ();

        private MazeRoom CreateRoom (int indexToExclude) {
            MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom> ();
            newRoom.settingsIndex = Random.Range (0, roomSettings.Length);
            if (newRoom.settingsIndex == indexToExclude) {
                newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
            }
            newRoom.settings = roomSettings[newRoom.settingsIndex];
            rooms.Add (newRoom);
            return newRoom;
        }

    }
}