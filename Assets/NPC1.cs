using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC Script that gives NPC fixed movement
// (in this case, continuously walking left to right)

public class NPC1 : MonoBehaviour
{
    public PlayerController player;
    private int playerX;
    private int playerY;
    public SpriteRenderer playerSprite;

    private Transform objTransform;

    private int startX;
    private int endX;
    private int y;

    private int currentX;

    private int step;
    private string direction;
    private bool animating;
    private float time;

    void Start()
    {
        objTransform = this.gameObject.transform;
        playerSprite.flipX = true;

        startX = 0;
        endX = 2;
        y = 4;

        currentX = 0;

        step = 1 * 2; // tile size times 2
        direction = "right";
        animating = false;
        time = 0.5f;
    }

    void Update()
    {
        // get the coordinates of the player
        List<int> PlayerCoords = player.GetPosition();
        playerX = PlayerCoords[0];
        playerY = PlayerCoords[1];

        if(animating)
        {
            // if animation is finished,
            if (time <= 0){
                // stop animation
                animating = false;

                // check if the player needs to switch directions
                if (direction.Equals("left") && currentX == startX){
                    direction = "right";
                    playerSprite.flipX = true;

                } else if (direction.Equals("right") && currentX == endX){
                    direction = "left";
                    playerSprite.flipX = false;
                }

            } else {
                // if animation is still ongoing,

                // move character accordingly
                if (direction.Equals("left")){
                    objTransform.position += new Vector3(-step * Time.deltaTime, 0, 0);
                } else if (direction.Equals("right")){
                    objTransform.position += new Vector3(step * Time.deltaTime, 0, 0);
                }

                time -= Time.deltaTime;
            }
        } else {
            // before initiating next movement, check if the npc will collide with the player

            // if not,
            if (!WillCollideWithPlayer()){
                // configure x-coordinate based on new position
                if (direction.Equals("left")){
                    currentX -= 1;
                } else if (direction.Equals("right")){
                    currentX += 1;
                }

                // set the animation time to 0.5 seconds
                time = 0.5f;

                // start the animation
                animating = true;
            }
        }
    }

    // checks if the npc will collide with the player
    bool WillCollideWithPlayer()
    {
        return (direction.Equals("left") && playerX == currentX - 1 && playerY == y) ||
                (direction.Equals("right") && playerX == currentX + 1 && playerY == y);
    }

    // gets the current position of the npc
    public List<int> GetPosition()
    {
        List<int> Coords = new List<int>();
        Coords.Add(currentX);
        Coords.Add(y);

        return Coords;
    }
}
