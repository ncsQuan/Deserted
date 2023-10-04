using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.ComponentModel;
using System;

public class AimController : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera aimCamera;
    public GameObject crosshair;

    private PlayerController playerCtrl;

    [Description("Which layers should the raycast consider when calculating the aim target")]
    public LayerMask crosshairColliderMask = new();

    [NonSerialized]
    public Vector3 interceptPosition;

    void Awake()
    {
        playerCtrl = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        setAimCamera(playerCtrl.aim);
        findAimIntercept();
    }

    void setAimCamera(bool isAiming)
    {
        aimCamera.gameObject.SetActive(isAiming);
        crosshair.gameObject.SetActive(isAiming);
    }


    void findAimIntercept()
    {
        //Reset intercept position in case we haven't found anything to intercept
        interceptPosition = Vector3.zero;
        if (playerCtrl.aim)
        {

            //Find the middle of the screen, which is here the crosshair is present
            //Replace this later with actual transform from crosshair
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            //Cast a raycast from the camera through the center of the screen
            Ray cameraRaycastToScreenCenter = Camera.main.ScreenPointToRay(screenCenter);

            RaycastHit hit;
            if (Physics.Raycast(cameraRaycastToScreenCenter, out hit, 999f, crosshairColliderMask))
            {
                interceptPosition = hit.point;
            }

            //transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
    }
}
