using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StageGrid grid;

    private Transform objTransform;
    private int step;
    private string direction;
    private float time;
    private bool animating;

    public int playerX;
    public int playerY;

    void Start()
    {
        objTransform = this.gameObject.transform;

        step = 1 * 4; // tile size times 4

        direction = "down";
        time = 0;
        animating = false;

        // player is initially at the tile (1, 1)
        // Note: Counting starts with 0
        playerX = 1;
        playerY = 1;
    }

    void Update()
    {
        if(animating)
        {
            // if animation is finished,
            if (time <= 0){

                // if current tile is a normal "open" tile
                if (grid.Tile(playerX, playerY).Equals("open")){

                    // adjust position for consistency
                    objTransform.position = new Vector3(Mathf.Round(objTransform.position.x),
                                                    Mathf.Round(objTransform.position.y), 0);

                    // stop animation
                    animating = false;
                    
                } else {
                    // if current tile forces movement,

                    int x = playerX;
                    int y = playerY;
                    int tileNameLength = grid.Tile(x, y).Length;

                    // if the tile is not an ice tile
                    if (!grid.Tile(x, y).Equals("ice")){
                        // configure direction to forced direction
                        direction = grid.Tile(x, y).Substring(6,tileNameLength - 6);
                    }
                    // else, direction of forced movement is the direction
                    // that the player is currently facing (no changes needed)

                    // configure coordinates of next tile to check the next
                    // tile that the player will possibly move to
                    if (direction.Equals("up")){
                        x = playerX;
                        y = playerY - 1;
                    } else if (direction.Equals("down")){  
                        x = playerX;
                        y = playerY + 1;
                    } else if (direction.Equals("left")){
                        x = playerX - 1;
                        y = playerY;
                    } else if (direction.Equals("right")){
                        x = playerX + 1;
                        y = playerY;
                    }

                    // initiate moving to another tile
                    MoveTo(x, y);
                }
            } else {
                // if animation is still ongoing,

                // move character accordingly
                if (direction.Equals("up")){
                    objTransform.position += new Vector3(0, step * Time.deltaTime, 0);
                } else if (direction.Equals("down")){
                    objTransform.position += new Vector3(0, -step * Time.deltaTime, 0);
                } else if (direction.Equals("left")){
                    objTransform.position += new Vector3(-step * Time.deltaTime, 0, 0);
                } else if (direction.Equals("right")){
                    objTransform.position += new Vector3(step * Time.deltaTime, 0, 0);
                }

                time -= Time.deltaTime;
            }
        } else {
            // if there is no animation happening,

            int x = -1;
            int y = -1;

            // accept inputs from user (via keyboard) and
            // configure direction and coordinates of next tile based on input
            if (Input.GetKey(KeyCode.UpArrow)){
                direction = "up";
                x = playerX;
                y = playerY - 1;
            } else if (Input.GetKey(KeyCode.DownArrow)){
                direction = "down";
                x = playerX;
                y = playerY + 1;
            } else if (Input.GetKey(KeyCode.LeftArrow)){
                direction = "left";
                x = playerX - 1;
                y = playerY;
            } else if (Input.GetKey(KeyCode.RightArrow)){
                direction = "right";
                x = playerX + 1;
                y = playerY;
            }

            // initiate moving to next tile
            MoveTo(x, y);
        }
    }

    // move player to tile with coordinates x and y
    void MoveTo(int x, int y)
    {
        // if moving to a "wall",
        if(grid.Tile(x, y).Equals("wall")){
            // adjust position for consistency
            objTransform.position = new Vector3(Mathf.Round(objTransform.position.x),
                                            Mathf.Round(objTransform.position.y), 0);

            // stop animation
            animating = false;

        } else {
            // if moving to an open tile,

            // set animation time to 0.25 sec
            time = 0.25f;

            // update player position to new position after movement
            playerX = x;
            playerY = y;

            // initiate animation to (x, y)
            animating = true;
        }
    }
}
