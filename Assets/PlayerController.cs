using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StageGrid grid;
    public Animator animator;
    public SpriteRenderer playerSprite;

    private Transform objTransform;
    private int step;
    private string direction;
    private float time;
    private bool isWalking;

    private int playerX;
    private int playerY;

    void Start()
    {
        objTransform = this.gameObject.transform;

        step = 1 * 4; // tile size times 4

        direction = "down";
        time = 0;
        isWalking = false;

        // player is initially at the tile (1, 1)
        // Note: Counting starts with 0
        playerX = 1;
        playerY = 1;
    }

    void Update()
    {
        if(isWalking)
        {
            animator.SetFloat("Speed", 1);

            // if animation is finished,
            if (time <= 0){

                // if current tile is a normal "open" tile
                if (grid.GetTile(playerX, playerY).Equals("open")){

                    // adjust position for consistency
                    objTransform.position = new Vector3(Mathf.Round(objTransform.position.x),
                                                    Mathf.Round(objTransform.position.y), 0);

                    // stop animation
                    isWalking = false;
                    
                } else {
                    // if current tile forces movement,

                    int x = playerX;
                    int y = playerY;

                    // configure direction accordingly
                    List<int> Coordinates = ConfigureDirectionAndCoordinates(x, y);
                    x = Coordinates[0];
                    y = Coordinates[1];

                    // initiate moving to another tile
                    MoveTo(x, y);
                }
            } else {
                // if animation is still ongoing,

                // move character accordingly
                if (direction.Equals("up")){
                    objTransform.position += new Vector3(0, step * Time.deltaTime, 0);

                    // make player face up
                    animator.SetBool("isDown", false);
                    animator.SetBool("isUp", true);
                    animator.SetBool("isSide", false);
                } else if (direction.Equals("down")){
                    objTransform.position += new Vector3(0, -step * Time.deltaTime, 0);

                    // make player face down
                    animator.SetBool("isDown", true);
                    animator.SetBool("isUp", false);
                    animator.SetBool("isSide", false);
                } else if (direction.Equals("left")){
                    objTransform.position += new Vector3(-step * Time.deltaTime, 0, 0);

                    // make player face left
                    playerSprite.flipX = false;
                    animator.SetBool("isDown", false);
                    animator.SetBool("isUp", false);
                    animator.SetBool("isSide", true);
                } else if (direction.Equals("right")){
                    objTransform.position += new Vector3(step * Time.deltaTime, 0, 0);

                    // make player face right
                    playerSprite.flipX = true;
                    animator.SetBool("isDown", false);
                    animator.SetBool("isUp", false);
                    animator.SetBool("isSide", true);
                }

                time -= Time.deltaTime;
            }
        } else {
            // if player is not moving
            animator.SetFloat("Speed", 0);

            int x = -1;
            int y = -1;

            // accept inputs from user (via keyboard) and
            // configure direction and coordinates of next tile based on input
            if (Input.GetKey(KeyCode.UpArrow)){
                direction = "up";
                x = playerX;
                y = playerY - 1;

                // make player face up
                animator.SetBool("isDown", false);
                animator.SetBool("isUp", true);
                animator.SetBool("isSide", false);
            } else if (Input.GetKey(KeyCode.DownArrow)){
                direction = "down";
                x = playerX;
                y = playerY + 1;

                // make player face down
                animator.SetBool("isDown", true);
                animator.SetBool("isUp", false);
                animator.SetBool("isSide", false);
            } else if (Input.GetKey(KeyCode.LeftArrow)){
                direction = "left";
                x = playerX - 1;
                y = playerY;

                // make player face left
                playerSprite.flipX = false;
                animator.SetBool("isDown", false);
                animator.SetBool("isUp", false);
                animator.SetBool("isSide", true);
            } else if (Input.GetKey(KeyCode.RightArrow)){
                direction = "right";
                x = playerX + 1;
                y = playerY;

                // make player face right
                playerSprite.flipX = true;
                animator.SetBool("isDown", false);
                animator.SetBool("isUp", false);
                animator.SetBool("isSide", true);
            }

            // also accept user input to interact with NPC
            if (Input.GetKeyDown(KeyCode.Space)){
                int npcX = playerX;
                int npcY = playerY;

                if (direction.Equals("up")){
                    npcY -= 1;
                } else if (direction.Equals("down")){
                    npcY += 1;
                } else if (direction.Equals("left")){
                    npcX -= 1;
                } else if (direction.Equals("right")){
                    npcX += 1;
                }

                if(grid.GetTile(npcX, npcY).Equals("npc")){
                    print("Player initiated interaction with NPC!");
                }
            }

            // initiate moving to next tile
            MoveTo(x, y);
        }
    }

    // configure direction when movement is forced
    List<int> ConfigureDirectionAndCoordinates(int x, int y)
    {
        int tileNameLength = grid.GetTile(x, y).Length;

        // if the tile is not an ice tile
        if (!grid.GetTile(x, y).Equals("ice")){
            // configure direction to forced direction
            direction = grid.GetTile(x, y).Substring(6,tileNameLength - 6);
        }
        // else, direction of forced movement is the direction
        // that the player is currently facing (no changes needed)

        // configure coordinates of next tile to check the next
        // tile that the player will possibly move to
        if (direction.Equals("up")){
            y -= 1;
        } else if (direction.Equals("down")){  
            y += 1;
        } else if (direction.Equals("left")){
            x -= 1;
        } else if (direction.Equals("right")){
            x += 1;
        }

        // put potential new coordinates to a list and return it
        List<int> Coordinates = new List<int>();
        Coordinates.Add(x);
        Coordinates.Add(y);

        return Coordinates;
    }

    // move player to tile with coordinates x and y
    void MoveTo(int x, int y)
    {
        // if moving to a "wall",
        if(grid.GetTile(x, y).Equals("wall") || grid.GetTile(x, y).Equals("npc")){
            // adjust position for consistency
            objTransform.position = new Vector3(Mathf.Round(objTransform.position.x),
                                            Mathf.Round(objTransform.position.y), 0);

            // stop animation
            isWalking = false;

        } else {
            // if moving to an open tile,

            // set animation time to 0.25 sec
            time = 0.25f;

            // update player position to new position after movement
            playerX = x;
            playerY = y;

            // initiate animation to (x, y)
            isWalking = true;
        }
    }

    // returns the current coordinates of the player
    public List<int> GetPosition()
    {
        List<int> Coordinates = new List<int>();
        Coordinates.Add(playerX);
        Coordinates.Add(playerY);

        return Coordinates;
    }
}
