using UnityEngine;
using System.Collections;

public class MovingMaze2 : MonoBehaviour
{

    // Before rendering each frame..
    void Update()
    {
        //Wait for 5 seconds
        new WaitForSeconds(5);

        // Rotate the game object that this script is attached to by 15 in the X axis,
        // 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
        // rather than per frame.
        if (transform.position.x < 5 && transform.position.x > -14)
        {
            transform.position = new Vector3(-2, 0, 0) * Time.deltaTime;

        }
        else
        {
            transform.position = new Vector3(2, 0, 0) * Time.deltaTime;
        }
    }
}