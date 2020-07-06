using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour {

    /*Touch Input Manager Script does the following:
 * --------------------------------------------------
 * 1.Detects the touch Input from the player (Tap, Swipe Left, Swipe Right etc..)
 * 2.Triggers player movements in MainCharacterController Script accordingly (Run, Jump etc..)
 * 3.Controls Mouse and Keyboard Inputs (A, S, W, D, Space / Left mouse button)
 * 4.Originaly Written By Michael "N3K EN", Edited by Joseph Wolf (yossig91@gmail.com)
 */

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown; //booleans for whether the action is being performed at the moment
    private bool isDragging = false; //checks if the player is dragging finger on the screen at the moment

    private Vector2 startTouch, swipeDelta; //stores the starting touch point, as well as the swipeDelta 

    private int touchDeadZone = 25; //circle radius where touch dosen't qualify as swipe - check 'TDZNOTE' note below for details

    public MainCharacterController mainCharController;
    GameManager gameManager; //refference the GamaManager script component 

    //getters & setters (accessors), used to retrive the variables from outside of this script 
    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } } 
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    void Start() {

        mainCharController = GetComponent<MainCharacterController>(); 
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        ResetTouchInputs(); // Resets the touch inputs booleans everyframe to avoid over-use

        //Mouse Inputs for PC (Internal testing phase) 
        #region Standalone PC Inputs 

        if (Input.GetMouseButtonDown(0)) { //if the player pressed-down the left mouse button
            tap = true; //tap has occured
            isDragging = true; //player is dragging the mouse on the screen at the moment
            startTouch = Input.mousePosition; //sets the staring touch point the the current mouse vector inputs
        }
        else if (Input.GetMouseButtonUp(0))  //if the player released-up the left mouse button
        {
            isDragging = false; // player is not dragging finger on the screen any longer
            Reset(); //resets the values at the end of touch/mouse drag.
        }
        #endregion

        /* Touch Inputs for Mobile devices (Prototype phase)
         * Touch Input distance is calculated as long the finger is down
         */
        #region Mobile Inputs 
        if (Input.touches.Length > 0) //if theres at least 1 touch on the screen
        {
            if (Input.touches[0].phase == TouchPhase.Began) //if the touch phase just began
            {
                isDragging = true; //player is dragging the finger on the screen at the moment
                tap = true; //tap has occured
                startTouch = Input.touches[0].position; //set this to start position
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) //if we are currently not touching or touch is cancelled
            { 
                isDragging = false; //player is no longer dragging the finger on the screen
                Reset(); //resets the values for touch/mouse action
            } 
        }
        #endregion // //Touch Input distance is calculated as long the finger is down

        swipeDelta = Vector2.zero; //Sets the delta to zero at the beginning of the frame
        if(isDragging) //player is dragging the finger on the screen at the moment
        {
            if (Input.touches.Length > 0 && gameManager.playerIsUp) //if theres at least 1 touch on the screen
                swipeDelta = Input.touches[0].position - startTouch; //calcuate the swipe delta and assign it
                   
            else if (Input.GetMouseButton(0)) //if mouse button is currently being held down
                swipeDelta = (Vector2)Input.mousePosition - startTouch; //calcuate the 'mouse-swipe' delta and assign it
        }

        /* NOTE: may use later for isolating tap event - delete after
        if (Tap)
        {
            Debug.Log("Tap");
        }
        */

        /* TDZNOTE: Touch dead zone means the finger is on the screen but not moving enough in any direction to be considered a swipe
         * increasing this value will require moving the finger further to be considered as a swipe and vise versa.
         */

        //did we cross the dead zone?
        if (swipeDelta.magnitude > touchDeadZone && gameManager.playerIsUp && gameManager.onLevelScreen) //means we have a swipe
        {
        //a swipe has occured but in which direction? find out from the SwipeDelta values

            float x = swipeDelta.x;
            float y= swipeDelta.y;

            //we check which one is bigger x or y value?

            if(Mathf.Abs(x) > Mathf.Abs(y))
                //check if X is bigger than Y, it means swiping on Horizontal axis - left or right 
                //uses Absoulute on value - negatives only matter from the next line)
            {
                //left or right? 
                if (x < 0) //if the X value is negative
                {
                    swipeLeft = true; //Swipe left has occured
                    Debug.Log("Swipe Left");
                }
                else { //if the X value is positive
                    swipeRight = true; //Swipe right has occured
                    Debug.Log("Swipe Right");
                } 
            }
            else //if Y is bigger than X it means swipe on Vertical Axis - Up or down
            {
                //up or down? 
                if (y < 0)
                {
                    swipeDown = true;
                    Debug.Log("Swipe Down");
                }
                else {
                    swipeUp = true;
                    Debug.Log("Swipe up");
                }
            }

            Reset(); //reset at the end of touch phase
        }

        } //end of Update method

    private void ResetTouchInputs() // Resets the touch inputs booleans every frame to avoid over-use
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }

    private void Reset() //Used to reset the dragging values for touch/mouse action
    {
        startTouch = swipeDelta = Vector2.zero; //sets the starting touch and swipeDetla to (0, 0);
        isDragging = false; //player is not dragging mouse/finger on the screen any longer
    }



}












