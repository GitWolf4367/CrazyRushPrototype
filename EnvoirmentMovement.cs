using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvoirmentMovement : MonoBehaviour
{
    /* Envoirment Movement Script does the following:
     * 1.Move the scene towards the player on Z-axis (generats running effect)
     */

    public float movementSpeed;
    public float speedModifier; //ability to change the speed from debug menu

    GameManager gameManager;

    public GameObject player;

    void Start()
    {
        gameManager = player.GetComponent<GameManager>();
        
    }

    void Update()
    {
        if (gameManager.onLevelScreen) //if the player is currently running inside the level 
        transform.Translate( new Vector3(0, 0, movementSpeed + speedModifier) * Time.deltaTime); 
    }

    //Note - optimize the code below later

    public void fastMovement()
    {
        speedModifier = 5;
    }
    public void slowMovement()
    {
        speedModifier = -3;
    }
    public void normalMovement()
    {
        speedModifier = 3;
    }

    public void StopMovement()
    {
        movementSpeed = 0;
    }

    public void StartMovement()
    {
        movementSpeed = 10;
    }
}
