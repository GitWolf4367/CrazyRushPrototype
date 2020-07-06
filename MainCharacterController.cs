using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{

    /* MainCharacterController Script does the following:
     * --------------------------------------------------
     * 1.Controlls the Player animations state (Running, Jumping, Rolling etc..)
     * 2.Controlls the Player movement parameters (Running Speed, Jumping height, reciving damage etc..)
     * 3.Written By Joseph Wolf (yossig91@gmail.com)
     */

    Animator mainCharacterAnimator; //Refference to Player Animator component
    TouchInputManager inputManager; //Refference to TouchInputManager script component
    UIManager uiManager; //Refference to TouchInputManager script component
    PlayerData playerData; // Refference to the Player Data script component
    private Rigidbody rb; //Refference to the Rigidbody component
    public EnvoirmentMovement envoirmentMovement; //Refference to the envoirmentMovement script component;
    public CameraFollowPlayer cameraFollowPlayer; //Refference to the cameraFollowPlayer script component;
    GameManager gameManager; //Refference to the gameManager script component;


    public bool movingLeft;
    public bool movingRight;
    public bool isGrounded = true;
    public bool activeObstacles = true;

    public string currentLane = "middle";
    public string targetLane = "DependsOnNextSwipeDirection";

    public int uiHeartsCounter;

    public float jumpHeight = 6.5f;
    float leftLanePos = 2.0f;
    float rightLanePos = -2.0f;
    float middleLanePos = 0;
    float playerSwitchLanesSpeed = 7.0f;


    void Start()
    {
        gameManager = this.GetComponent<GameManager>();//Assigns the GameManger script component to gameManager
        mainCharacterAnimator = this.GetComponent<Animator>(); //Assigns the Animator component to mainCharacterAnimator
        inputManager = this.GetComponent<TouchInputManager>();  //Assigns the TouchInputManager script component to InputManager
        rb = GetComponent<Rigidbody>(); //Assigns the Rigidbody component to InputManager
        playerData = this.GetComponent<PlayerData>();  //Assigns the PlayerData component to playerData
        uiManager = this.GetComponent<UIManager>(); //Assigns the UIManager component to uiManager
        uiHeartsCounter = uiManager.uiHeartsarr.Length; //get the numbers of hearts in array from UImanager
        activeObstacles = true; //player will recive damage from obstacles
        SittingOnTrainStation(); //calls the function, runs the menu screen on start
    }

    void OnCollisionEnter(Collision collision) //if player's collider enters ground coliider
    {
        isGrounded = true;
        mainCharacterAnimator.SetBool("isJumping", false); //Sets the boolean 'isJumping' parameter in the Animator to false
        mainCharacterAnimator.SetBool("isOnAir", false); //Sets the boolean 'isOnAir' parameter in the Animator to false

        Debug.Log("Grounded");
    }

    void OnCollisionStay(Collision collision) //if player's collider stays on ground coliider
    {
        isGrounded = true; //player is touching the ground



        // Code for future Refference:
        // mainCharacterAnimator.SetBool("isJumping", false); //Sets the boolean 'isJumping' parameter in the Animator to false
        //   mainCharacterAnimator.SetBool("isOnAir", false); //Sets the boolean 'isJumping' parameter in the Animator to false
    }

    void OnCollisionExit(Collision collision) //when player's collider exits ground coliider - means onAir
    {
        isGrounded = false; //player is no longer grounded
        mainCharacterAnimator.SetBool("isOnAir", true); //Sets the boolean 'isOnAir' parameter in the Animator to false
        Debug.Log("On Air");
    }

    void OnTriggerEnter(Collider other) //if player's collider enters another trigger
    {
        if (other.gameObject.tag == "Obstacle" && activeObstacles) //if the trigger tag is Obstacle + obsticles are active (true by default)
        {
            ReducePlayerLifePoints();
        }

        if (other.gameObject.tag == "FinishLine") //if the player has reached the finish line
        {
            PlayerHasReachedFinishLine();
        }

    }

    void OnTriggerExit(Collider other) //if player's collider exit another trigger
    {
        if (other.gameObject.tag == "Coin") //if the trigger tag is Coin
        {
            playerData.coinsAmount += 1; //increase coin Amount in Player Data
            Destroy(other.gameObject); //destroy coin
            Debug.Log("coin pick up");
        }
    }



    void Update()
    {

        if (inputManager.SwipeUp && !movingLeft && !movingRight &&isGrounded && !mainCharacterAnimator.GetBool("isJumping")
            && !mainCharacterAnimator.GetBool("isOnAir") && mainCharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunningForward"))   // Avoids double jump.
        {
            PlayerJump();
        }

        if (inputManager.SwipeDown && !movingLeft && !movingRight && isGrounded) //if the player swipes down and is not currently moving lanes
        {
            mainCharacterAnimator.SetBool("isRolling", true); //Sets the boolean 'isRolling' parameter in the Animator to false - pefrom roll animation
            Debug.Log("Roll");
        }

        if (mainCharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("LandRoll")) //if player has finished rolling animation
        {
            mainCharacterAnimator.SetBool("isRolling", false); //reset roll 
        }

        if (inputManager.SwipeLeft && !movingLeft && !movingRight) //if the player swipes left and is not currently moving lanes
        {
            if (currentLane == "middle")
            {
                targetLane = "left";
                movingLeft = true;
            }

            if (currentLane == "right")
            {
                targetLane = "middle";
                movingLeft = true;
            }

            if (currentLane == "left")
            {
                //do nothing
                return;
            }
        }

         if (inputManager.SwipeRight && !movingLeft && !movingRight) //if the player swipes right and is not currently moving lanes
        {
                if (currentLane == "middle")
                {
                    targetLane = "right";
                    movingRight = true;
                }

                if (currentLane == "left")
                {
                    targetLane = "middle";
                    movingRight = true;
                }

                if (currentLane == "right")
                {
                    //do nothing
                    return;
                }
            }

        //Currently in middle lane + Swipe Left = right lane
        if (movingLeft && currentLane=="middle" && targetLane == "left") 
        {
            Vector3 targetPos = new Vector3(leftLanePos, this.transform.position.y, 0); 
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSwitchLanesSpeed * Time.deltaTime);
            mainCharacterAnimator.SetBool("RunningForwardLeft", true); //Sets the boolean 'RunningForwardRight' parameter in the Animator to true

            if (this.transform.position == targetPos) //if the player position is equal to target lane position
            {
                movingLeft = false;
                currentLane = "left";
                targetLane = "DependsOnNextSwipeDirection";
                mainCharacterAnimator.SetBool("RunningForwardLeft", false); //Sets the boolean 'RunningForwardRight' parameter in the Animator to false
            }
        }

        //Currently in right lane + Swipe Left = middle lane
        if (movingLeft && currentLane == "right" && targetLane == "middle") 
        {
            Vector3 targetPos = new Vector3(middleLanePos, this.transform.position.y, 0);
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSwitchLanesSpeed * Time.deltaTime);
            mainCharacterAnimator.SetBool("RunningForwardLeft", true); //Sets the boolean 'RunningForwardRight' parameter in the Animator to true
            if (this.transform.position == targetPos)
            {
                movingLeft = false;
                currentLane = "middle";
                targetLane = "DependsOnNextSwipeDirection";
                mainCharacterAnimator.SetBool("RunningForwardLeft", false); //Sets the boolean 'RunningForwardRight' parameter in the Animator to false
            }
        }

        //Currently in middle lane + Swipe Right = right lane
        if (movingRight && currentLane == "middle" && targetLane == "right")
        {
            Vector3 targetPos = new Vector3(rightLanePos, this.transform.position.y, 0);
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSwitchLanesSpeed * Time.deltaTime);
            mainCharacterAnimator.SetBool("RunningForwardRight", true); //Sets the boolean 'RunningForwardRight' parameter in the Animator to true

            if (this.transform.position == targetPos)
            {
                movingRight = false;
                currentLane = "right";
                targetLane = "DependsOnNextSwipeDirection";
                mainCharacterAnimator.SetBool("RunningForwardRight", false); //Sets the boolean 'RunningForwardRight' parameter in the Animator to false

            }
        }

        //Currently in left lane + Swipe Right = middle lane
        if (movingRight && currentLane == "left" && targetLane == "middle")
        {
            Vector3 targetPos = new Vector3(0f, this.transform.position.y, 0);
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSwitchLanesSpeed * Time.deltaTime);
            mainCharacterAnimator.SetBool("RunningForwardRight", true); //Sets the boolean 'RunningForwardRight' parameter in the Animator to true

            if (this.transform.position == targetPos)
            {
                movingRight = false;
                currentLane = "middle";
                targetLane = "DependsOnNextSwipeDirection";
                mainCharacterAnimator.SetBool("RunningForwardRight", false); //Sets the boolean 'RunningForwardRight' parameter in the Animator to false
            }
        }

        if (isGrounded) //if the player is on the ground
            mainCharacterAnimator.SetBool("isOnAir", false); //Sets the boolean 'isOnAir' parameter in the Animator to false

    } //end of update method

    //Methods

    public void SittingOnTrainStation()
    {
        cameraFollowPlayer.enabled = false;
        envoirmentMovement.movementSpeed = 0;
        mainCharacterAnimator.SetTrigger("isSitting"); //Sets the boolean 'isFallOver' parameter in the Animator to false
        transform.position = new Vector3(0.72f, 0, 0);
        transform.rotation = Quaternion.Euler(0, 270, 0);
    }

    public void StartRunningOntrack()
    {
        gameManager.onLevelScreen = true;
        cameraFollowPlayer.enabled = true;
        envoirmentMovement.StartMovement();
        mainCharacterAnimator.SetTrigger("isRunningOnTrack"); //Sets the boolean 'isFallOver' parameter in the Animator to false
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void PlayerHasReachedFinishLine() //removes life points from player & UI
    {
        mainCharacterAnimator.SetTrigger("isFinish"); //Sets the boolean 'isFallOver' parameter in the Animator to false
        envoirmentMovement.StopMovement(); //stops movement
        envoirmentMovement.speedModifier = 0; //reset speed modifier
        gameManager.playerIsUp = false; //player is "no longer up" (same as death - no ability to move)
        Debug.Log("finish");
    }

    void PlayerJump() //activates jump 
    {
        mainCharacterAnimator.SetBool("isJumping", true); //Sets the boolean 'isJumping' parameter in the Animator to false
        Physics.gravity = new Vector3(0, -7.5F, 0);
        rb.velocity = new Vector3(0, 1.8f, 0);
        rb.AddForce(new Vector3(0, 200f, 0), ForceMode.Acceleration);
        StartCoroutine(ApplyGravityOnJump());
        Debug.Log("Jump");
    }

    IEnumerator ApplyGravityOnJump() //applies extra gravity when player is jumping - for faster landing
    {
        yield return new WaitForSeconds(0.5f); //wait 
        Physics.gravity = new Vector3(0, -25.8F, 0); //set gravity
    }

    void ReducePlayerLifePoints() //removes life points from player & UI
    {
        playerData.livesLeft -= 1; //reduce life points in playerData

        if (uiHeartsCounter > 0)
        {
            uiHeartsCounter -= 1; ; //remove life point in UI
        }

        uiManager.uiHeartsarr[uiHeartsCounter].SetActive(false); //hides the heart in the UI

        if (playerData.livesLeft > 0)
        {
            mainCharacterAnimator.SetTrigger("isGettingHit"); //Sets the boolean 'isFallOver' parameter in the Animator to false
        }
        else //when life reaches 0
        {
            envoirmentMovement.StopMovement(); //stops player movement
            envoirmentMovement.speedModifier = 0; //resets speed modifier
            gameManager.playerIsUp = false; //player is no longer up
            mainCharacterAnimator.SetTrigger("isFallOver"); //Sets the boolean 'isFallOver' parameter in the Animator to false
            Debug.Log("Lives left:" + playerData.livesLeft);
        }
    }

    public void ActiveObstacles()
    {
        activeObstacles = true;
    }
    public void InActiveObstacles()
    {
        activeObstacles = false;
    }

}
