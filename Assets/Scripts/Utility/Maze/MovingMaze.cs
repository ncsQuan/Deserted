using UnityEngine;
using System.Collections;

public class MovingMaze : MonoBehaviour
{

    // Before rendering each frame..
    void Update()
    {
        //Wait for 5 seconds
        new WaitForSeconds(5);

        // Rotate the game object that this script is attached to by 15 in the X axis,
        // 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
        // rather than per frame.
        transform.Rotate(new Vector3(0, 5, 0) * Time.deltaTime);
        
    }
}