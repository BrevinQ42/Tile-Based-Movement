using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGrid : MonoBehaviour
{
    private int rows;
    private int columns;
    public string[,] grid; // 2d list

    void Start()
    {
        rows = 4;
        columns = 3;
        grid = new string[rows,columns];

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
    public string Tile(int x, int y)
    {
        // if the coordinates are within the grid
        if(x > -1 && x < columns && y > -1 && y < rows){
            // return name of the tile on (x, y)
            return grid[y,x];
        }

        // else, it is considered a "wall" (world border)
        return "wall";
    }
}
