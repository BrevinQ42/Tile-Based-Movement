using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGrid : MonoBehaviour
{
    public NPC1 npc1;

    private int rows;
    private int columns;
    private string[,] grid; // 2d list

    private List<int> npc1Coords;
    private int npc1X;
    private int npc1Y;

    void Start()
    {
        rows = 5;
        columns = 3;
        grid = new string[rows,columns];

        npc1Coords = npc1.GetPosition();
        npc1X = npc1Coords[0];
        npc1Y = npc1Coords[1];

        ConfigureGrid();
    }

    void ConfigureGrid()
    {
        // initially set up stage as all open spaces (excluding world borders)
        for(int y = 0; y < rows; y++){
            for(int x = 0; x < columns; x++){
                grid[y,x] = "open";
            }
        }

        // tile npc is initially on
        grid[npc1Y, npc1X] = "npc";

        // barrier tiles
        grid[0,0] = "wall";

        // "ice" tiles
        grid[1,2] = "ice";
        grid[2,2] = "ice";

        // "arrow" tiles
        grid[0,2] = "force-left";
        grid[2,1] = "force-down";
        grid[3,1] = "force-right";
        grid[3,2] = "force-up";
    }

    // queries what tile is on (x, y)
    public string GetTile(int x, int y)
    {
        // if the coordinates are within the grid
        if(x > -1 && x < columns && y > -1 && y < rows){
            // return name of the tile on (x, y)
            return grid[y,x];
        }

        // else, it is considered a "wall" (world border)
        return "wall";
    }

    void Update()
    {
        // get the position of NPC1
        npc1Coords = npc1.GetPosition();

        // if new coordinates are not the same with the old ones,
        if (npc1X != npc1Coords[0] || npc1Y != npc1Coords[1]){
            // the old tile that NPC1 is on is now open again
            grid[npc1Y, npc1X] = "open";

            // update NPC coordinates in this script
            npc1X = npc1Coords[0];
            npc1Y = npc1Coords[1];

            // update tile on which the NPC is standing
            grid[npc1Y, npc1X] = "npc";
        }
    }
}
