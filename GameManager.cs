using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /* GameManager script does the following:
     * 1.Handle the state of the game and the player (player is alive, on which screen, etc..
     * 2.Resets the game
     */

    public bool OnMenuScreen;
    public bool onLevelScreen;
    public bool playerIsUp;

    public EnvoirmentMovement envoirmentMovement; //Refference to the envoirmentMovement script
    public MainCharacterController mainCharController; //Refference to the MainCharacterController script;
    Animator mainCharacterAnimator; //Refference to Player Animator component

    public void restartGame() //when player is touching the home button 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reset scene
    }

}
