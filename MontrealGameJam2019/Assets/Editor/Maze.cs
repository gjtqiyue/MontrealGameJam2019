using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Maze : EditorWindow
{
    public int ROW_LENGTH = 8;
    public int COL_LENGTH = 8;
    public float GRID_LENGTH = 5f;      // the fixed grid length per unit

    public Vector3 origin;                              // origin poing of the grid
    public GameObject wall;                             // wall reference
    public GameObject floor;                            // floor reference

    [MenuItem("Window/MazeGenerator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Maze));
    }

    private void OnGUI()
    {
        GUILayout.Label("Floor Prefab");
        floor = (GameObject)EditorGUILayout.ObjectField(floor, typeof(GameObject), false);

        GUILayout.Label("Wall Prefab");
        wall = (GameObject)EditorGUILayout.ObjectField(wall, typeof(GameObject), false);

        origin = EditorGUILayout.Vector3Field("Origin", origin);

        GUILayout.BeginHorizontal();
        ROW_LENGTH = EditorGUILayout.IntField("Row", ROW_LENGTH);
        COL_LENGTH = EditorGUILayout.IntField("Col", COL_LENGTH);
        GUILayout.EndHorizontal();

        GRID_LENGTH = EditorGUILayout.FloatField("Grid length", GRID_LENGTH);

        if (GUILayout.Button("Initialize"))
        {
            ReInitializeMaze();
        }

        if (GUILayout.Button("Generate Level"))
        {
            InitializeMaze();
            for (int i=0; i<COL_LENGTH; i++)
                RandomGenerate();
            EndMaze();
        }
    }


    public Transform Environment;
    private ArrayList maze;                             // it stores the structure of current maze
    private Dictionary<int, List<Cell>> mazeCell;       // it stores all the cells of maze in its own set
    private ArrayList mazeObjects;                      // it stores all the instantiated maze objects in scene
    private int rowIndex = 0;                           // current row index
    private int uniqueId = 0;                           // current unique id for the cell
    private int[] wallInfo;                             // an array that stores the info of generation of the front wall in that row

    // this class represent a grid cell in a maze
    // each row has 8 cells
    protected class Cell
    {
        public int m_id;            // unique id of this cell
        public bool m_frontWall;    // if this cell has front wall
        public bool m_leftWall;     // if this cell has left wall

        // constructor
        public Cell(int id, bool hasFront, bool hasLeft)
        {
            m_id = id;
            m_frontWall = hasFront;
            m_leftWall = hasLeft;
        }
    }

    private void Awake()
    {
        
    }

    private void InitializeMaze()
    {
        Environment = new GameObject("Environment").transform;
        // initialize data structure
        wallInfo = new int[ROW_LENGTH];
        mazeCell = new Dictionary<int, List<Cell>>();
        maze = new ArrayList();
        mazeObjects = new ArrayList();
    }

    public void ReInitializeMaze()
    {
        // clear the mazeCell
        if (mazeCell.Count > 0)
        {
            foreach (int key in mazeCell.Keys)
            {
                mazeCell[key].Clear();
            }

            mazeCell.Clear();
        }

        // clear the maze array
        maze.Clear();

        // set the ID back to 0
        uniqueId = 0;

        // rowIndex back to 0
        rowIndex = 0;

        // delete all the objects
        if (mazeObjects.Count > 0)
        {
            for (int i = 0; i < mazeObjects.Count; i++)
            {
                DestroyImmediate((GameObject)mazeObjects[i]);
            }
            mazeObjects.Clear();
        }

        DestroyImmediate(Environment.gameObject);
    }

    /* main generate method
     * generate a row and merge cells randomly
     * then construct it in the real map
     */
    public void RandomGenerate()
    {
        Cell[] row;

        // create a row based on the rowindex
        // the first row has no previous row so it is easier than the inner rows
        if (rowIndex == 0)
        {
            // if the index is 0, which means we want to create the first row

            row = CreateRow(0);

            row = Merge(row);

            // add the complete row to the map
            maze.Add(row);

            // increase the rowindex
            rowIndex++;

            row = ExtendWall(row);


            // the code below simpliy print out the generation result in the console
            string rowString = "";
            for (int i = 0; i < row.Length - 1; i++)
            {
                if (row[i].m_id != row[i + 1].m_id)
                {
                    rowString = string.Concat(rowString, row[i].m_id);
                    rowString = string.Concat(rowString, "|");
                }
                else
                {
                    rowString = string.Concat(rowString, row[i].m_id);
                }
            }
            rowString = string.Concat(rowString, row[7].m_id);

            PrintWallInfo();
        }
        else
        {
            // if the first row is already created, we come to here, generate inner rows
            // the steps are pretty similar to the above, but we pass differenct param to the CreateRow() method

            row = CreateRow(1);

            Cell[] mergeCell = Merge(row);

            // add the complete row to the map
            maze.Add(row);
            rowIndex++;

            row = ExtendWall(row);

            // print out the merged row infor to the console
            string mergeString = "";
            for (int i = 0; i < row.Length - 1; i++)
            {
                if (mergeCell[i].m_id != mergeCell[i + 1].m_id)
                {
                    mergeString = string.Concat(mergeString, mergeCell[i].m_id);
                    mergeString = string.Concat(mergeString, "|");
                }
                else
                {
                    mergeString = string.Concat(mergeString, mergeCell[i].m_id);
                }
            }
            mergeString = string.Concat(mergeString, mergeCell[7].m_id);

            PrintWallInfo();
        }


        // construct the maze based on the row
        GenerateRow(rowIndex);
    }

    // a helper method printing out the front wall data of the current row
    private void PrintWallInfo()
    {
        string wall = "";
        for (int i = 0; i < 8; i++)
        {
            if (wallInfo[i] == 1)
            {
                wall = string.Concat(wall, "1|");
            }
            else
            {
                wall = string.Concat(wall, "0|");
            }
        }
    }

    // Create a row 
    private Cell[] CreateRow(int rowType)
    {
        Cell[] newRow = new Cell[ROW_LENGTH];
        switch (rowType)
        {
            // create the first row get generate a cell in each array slot
            case 0:
                for (int i = 0; i < newRow.Length; i++)
                {
                    int id = getUniqueId();
                    newRow[i] = new Cell(id, true, true);

                    // create new list
                    mazeCell.Add(newRow[i].m_id, new List<Cell>());
                    mazeCell[id].Add(newRow[i]);
                }

                return newRow;


            // create inner row
            default:
                // create the second row based on the wall information
                // if in this slot there is a 0, then we know this cell must have the same number as the last row
                // if there is a 1, then we know a wall is there, and we assign a new set number to the new cell
                for (int i = 0; i < wallInfo.Length; i++)
                {

                    switch (wallInfo[i])
                    {
                        // if it's a opening from previous row, then we create a cell that has the same set number as the previous one
                        case 0:
                            Cell[] previousRow = (Cell[])maze[maze.Count - 1];
                            int oldId = previousRow[i].m_id;
                            newRow[i] = new Cell(oldId, true, true);
                            mazeCell[oldId].Add(newRow[i]);
                            break;
                        // else if there is a wall blocked the way, we set this cell to a new id
                        case 1:
                            int newId = getUniqueId();
                            newRow[i] = new Cell(newId, true, true);
                            mazeCell[newId].Add(newRow[i]);
                            break;
                        default:
                            break;
                    }
                }

                return newRow;
        }
    }

    // Merge this cell horizontally in a random way
    private Cell[] Merge(Cell[] row)
    {
        // check the row
        // if they are not in the same set then randomly decide if merge or not
        for (int i = 0; i < row.Length - 1; i++)
        {
            // if they are not same
            if (row[i].m_id != row[i + 1].m_id)
            {
                int choice = Random.Range(0, 3);

                // merge
                if (choice == 1)
                {
                    // move everything in that set to the set it's merging to
                    int newId = row[i].m_id;
                    int oldId = row[i + 1].m_id;
                    row[i].m_leftWall = false;
                    foreach (Cell cell in mazeCell[oldId])
                    {
                        Cell temp = cell;
                        temp.m_id = newId;

                        mazeCell[newId].Add(temp);
                    }

                    // then we clear the old set because everything in it has been moved to the new set
                    mazeCell[oldId].Clear();
                }

            }
        }
        return row;
    }

    // the maze must have a way out, so we determine vertical connection to the next row randomly
    private Cell[] ExtendWall(Cell[] row)
    {
        // the dictionary stores the set nunmber of each cell and put the cell with same set number into one category
        Dictionary<int, List<Cell>> map = new Dictionary<int, List<Cell>>();

        // first we iterate through the row and put them into the dictionary
        for (int i = 0; i < row.Length; i++)
        {
            int setId = row[i].m_id;

            // create a new list if it's not exist
            if (!map.ContainsKey(setId))
            {
                map.Add(setId, new List<Cell>());
            }
            // add the element in the list

            map[setId].Add(row[i]);
        }

        // determine a opening randomly for each set
        // we take a random number based on the count of element in that category
        // then we have a random index number
        // and we set the cell corresponding to the number to have no front wall, meaning it's a open route
        foreach (int key in map.Keys)
        {
            int randomOpening = Random.Range(0, map[key].Count);

            map[key][randomOpening].m_frontWall = false;

        }

        // do the rest
        // after we decide each opening of each set, we then randomly choose the remaining wall to have openings as well
        // and we update the wall info
        for (int i = 0; i < row.Length; i++)
        {
            if (row[i].m_frontWall)
            {
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    row[i].m_frontWall = false;
                    wallInfo[i] = 0;
                }
                else
                    wallInfo[i] = 1;
            }
            else
                wallInfo[i] = 0;
        }

        return row;
    }

    // provide a unique set id
    // if there is any exsiting id in the dictionary with no element, we then reuse it to prevent a large amount of keys
    private int getUniqueId()
    {
        int validId = 0;
        // check the maze Cell set to find any unused set number
        if (mazeCell.Keys.Count > 0)
        {
            foreach (int key in mazeCell.Keys)
            {
                if (mazeCell[key].Count == 0)
                    validId = key;
            }
        }

        if (validId != 0)
        {
            return validId;
        }
        else
        {
            uniqueId++;
            return uniqueId;
        }
    }

    // generate the row in the world based on the row created
    private void GenerateRow(int rowIndex)
    {
        // calculate the offset from the origin
        Vector3 startPoint = origin + Vector3.forward * GRID_LENGTH * rowIndex;

        // create floor
        for (int i = 0; i < ROW_LENGTH; i++)
        {
            GameObject floorObject = Instantiate(floor, startPoint + Vector3.left * GRID_LENGTH * i, Quaternion.identity);
            mazeObjects.Add(floorObject.gameObject);
            floorObject.transform.SetParent(Environment);
        }

        //create right side walls
        GameObject wallobject = Instantiate(wall, startPoint + new Vector3((GRID_LENGTH / 2), (GRID_LENGTH / 2), 0f), Quaternion.Euler(0, 90, 0));
        mazeObjects.Add(wallobject.gameObject);
        wallobject.transform.SetParent(Environment);

        //create walls in the row
        Cell[] row = (Cell[])maze[rowIndex - 1];
        for (int i = 0; i < row.Length; i++)
        {
            // if two cells are not in the same set
            // generate the left walls
            if (row[i].m_leftWall)
            {
                Vector3 spawnPoint = startPoint + Vector3.left * GRID_LENGTH * i + new Vector3(-(GRID_LENGTH / 2), (GRID_LENGTH / 2), 0f);
                GameObject leftWall = Instantiate(wall, spawnPoint, Quaternion.Euler(0, 90, 0));

                mazeObjects.Add(leftWall.gameObject);
                leftWall.transform.SetParent(Environment);
            }

            // generate the front walls
            if (row[i].m_frontWall)
            {
                Vector3 spawnPoint = startPoint + Vector3.left * GRID_LENGTH * i + new Vector3(0, (GRID_LENGTH / 2), (GRID_LENGTH / 2));
                GameObject frontWall = Instantiate(wall, spawnPoint, Quaternion.identity);
                mazeObjects.Add(frontWall.gameObject);
                frontWall.transform.SetParent(Environment);
            }
        }
    }

    internal bool EndMaze()
    {
        // the player can only end the maze when it has at least one row
        if (rowIndex > 0)
        {
            // then we create another inner row
            // but every cell in this row has a fornt wall to stop the player
            // so we don't call ExtendWall() in this method

            Cell[] lastRow = new Cell[ROW_LENGTH];

            lastRow = CreateRow(1);

            EndMerge(lastRow);

            maze.Add(lastRow);
            rowIndex++;

            GenerateRow(rowIndex);

            return true;
        }

        return false;
    }

    private void EndMerge(Cell[] lastRow)
    {
        // merge everything together, no wall in between
        for (int i = 0; i < lastRow.Length - 1; i++)
        {
            // if they are not same
            if (lastRow[i].m_id != lastRow[i + 1].m_id)
            {

                // move everything in that set to the other set
                int newId = lastRow[i].m_id;
                int oldId = lastRow[i + 1].m_id;
                lastRow[i].m_leftWall = false;
                foreach (Cell cell in mazeCell[oldId])
                {
                    Cell temp = cell;
                    temp.m_id = newId;

                    mazeCell[newId].Add(temp);
                }

                mazeCell[oldId].Clear();
            }

        }
        lastRow[7].m_leftWall = true;
    }


}
