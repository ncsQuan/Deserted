using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform cameraTranform;
    private void Start()
    {
        cameraTranform = Camera.main.transform;
    }

    //Ensure the UI is always facing the player camera
    //Use late update to account for objects that moved during Update()
    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTranform.forward);
    }
}

