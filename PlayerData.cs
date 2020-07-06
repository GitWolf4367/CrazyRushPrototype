using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //PlayerData script does the following:
    //1.Holds the player data (life, coins)

    public int livesLeft;
    public int coinsAmount;

    void Start()
    {
        livesLeft = 3;
        coinsAmount = 0;
    }
 
}
