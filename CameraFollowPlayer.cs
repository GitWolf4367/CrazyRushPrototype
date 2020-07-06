using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    /* CameraFollowPlayer Script does the following:
     * 1.Follows the player smoothly (using lerp) in a predefined offset and speed.
     */

    public Transform target; //target to follow, player
    Vector3 targetOffset; //distance between camera and player
    float followSpeed = 0.1f; //speed to follow the player movements, higher will make the camera follow the player faster (less delay).
    float cameraModifier = -2f; //modifies the camera position (from debug menu options)

    void Start()
    {
        targetOffset = (transform.position - target.position) + new Vector3(0, 0, cameraModifier); //sets the offset to the distance between camera and player position
    }

    void Update()
    {
        if (target) //if target is defined
        {
            //camera position will move from current position to target position (divided by number) in a set amount of speed
            //reason for the division in that I wanted the camera movement to follow *slightly* after the player position,decrease/remove to follow more agressivley.
            transform.position = Vector3.Lerp(transform.position, (target.position/4) + targetOffset, followSpeed);
        }
    }

    public void closeCamrera()
    {
        cameraModifier = -3.0f;
    }
    public void normalCamera()
    {
        cameraModifier = -2.0f;
    }
    public void farCamera()
    {
        cameraModifier = -1.0f;
    }
}

