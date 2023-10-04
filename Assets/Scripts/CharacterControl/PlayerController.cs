using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    [Header ("Aim Controls")]
    [Range(0, 360)]
    public int aimVerticalAngleMin = 15;
    [Range(0, 360)]
    public int aimVerticalAngleMax = 340;
    public float aiminSpeed = 10;
    [Range(0.1f, 1f)]
    public float horizontalAimSensitivity = 1f;
    [Range(0.1f, 1f)]
    public float verticalAimSensitivity = 1f;

    private GameObject Player;
    public float speed = 5f;
    public Transform fireStartPoint;
    public GameObject firePrefab;
    public float fireSpeed = 10f;
    public Slider energy;


    private int flarePiecesTotal = 4;
    private int flarePiecesFound = 0;
    public bool flareComplete = false;
    public bool shipInReach = false;
    private GameTimeManager gameTimeManager;
    public GameObject winText;
    public GameObject shipNotInReachText;

    Rigidbody body;


    public int timedelay = 2000;

    //Audio Variables
    public AudioSource footSteps;

    //UI Control variables
    public UIManager uiManager;

    // Player Movement Variables    
    private Animator playerAnim;

    //Aiming Variables
    public bool aim;
    private Vector2 aimMovementVector;
    public GameObject lookTransform;

    [NonSerialized]
    public PlayerInput playerInput;
    public Vector2 currentMovementVector;
    private Vector2 prevMovement = new();

    private SpellManager spellManager;

    public CinemachineFreeLook cinemachineVirtualCamera;


    float vel_x;
    float vel_z;

    float hMovement = 0f;
    float vMovement = 0f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerAnim = GetComponent<Animator>();
        spellManager = GetComponent<SpellManager>();


        /*       playerInput.PlayerInputController.Locomotion.performed += ctx =>
               {
                   currentMovementVector = ctx.ReadValue<Vector2>();

                   // make coordinates circular
                   //based on http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html
                   float x = currentMovementVector.x;
                   float z = currentMovementVector.y;
                   x = x * Mathf.Sqrt(1f - 0.5f * z * z);
                   z = z * Mathf.Sqrt(1f - 0.5f * x * x);

                   currentMovementVector = new Vector2(x, z);
               };*/

        /*        playerInput.PlayerInputController.Locomotion.performed += ctx =>
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                };*/

        playerInput.PlayerInputController.BasicAttack.performed += ctx =>
        {
            System.Random generator = new System.Random();
            int number = generator.Next(2);

            string[] states = { "basicAttack", "basicAttackMirror" };
            playerAnim.SetTrigger(states[number]);
        };

        playerInput.PlayerInputController.Pause.performed += ctx =>
        {
            uiManager.PauseGame();
        };

        playerInput.PlayerInputController.DecreaseSensitivity.performed += ctx =>
        {
            cinemachineVirtualCamera.m_XAxis.m_MaxSpeed -= 0.1f;
            cinemachineVirtualCamera.m_YAxis.m_MaxSpeed -= 0.001f;
            cinemachineVirtualCamera.m_XAxis.m_MaxSpeed = Mathf.Max(0.3f, cinemachineVirtualCamera.m_XAxis.m_MaxSpeed);
            cinemachineVirtualCamera.m_YAxis.m_MaxSpeed = Mathf.Max(0.003f, cinemachineVirtualCamera.m_YAxis.m_MaxSpeed);
        };

        playerInput.PlayerInputController.IncreaseSensitivity.performed += ctx =>
        {
            cinemachineVirtualCamera.m_XAxis.m_MaxSpeed += 0.1f;
            cinemachineVirtualCamera.m_YAxis.m_MaxSpeed += 0.001f;
            cinemachineVirtualCamera.m_XAxis.m_MaxSpeed = Mathf.Min(0.7f, cinemachineVirtualCamera.m_XAxis.m_MaxSpeed);
            cinemachineVirtualCamera.m_YAxis.m_MaxSpeed = Mathf.Min(0.007f, cinemachineVirtualCamera.m_YAxis.m_MaxSpeed);
        };

        playerInput.PlayerInputController.TimeSpell.performed += ctx =>
        {
            if (spellManager.CanCast(Spells.time)){
                playerAnim.SetTrigger("time_spell");
            }
            //timeStopped = !timeStopped;
            //TimeSp.Slow(timeStopped);
        };


        playerInput.PlayerInputController.FlareSpell.performed += ctx =>
        {

            Debug.Log("Flare");
            if (flareComplete & gameTimeManager.ship)
            {
                playerAnim.SetTrigger("flare_spell");
                //winText.SetActive(true);
                Debug.Log("Flare spell fired");
            }
            else if (flareComplete)
            {
                Debug.Log("Flare: Ship not in reach");
                //shipNotInReachText.SetActive(true);
            }
            else
            {
                Debug.Log("Flare spell not complete");
            }

        };

        playerInput.PlayerInputController.JumpSpell.started += ctx =>
        {
            if (spellManager.CanCast(Spells.jump))
            {
                playerAnim.SetTrigger("jump_spell");
            }
            //Debug.Log("animation set");
            //StartCoroutine(DelayJump(1f));
        };

        playerInput.PlayerInputController.Aim.performed += ctx => { 
            aim = true;
            Sprint(false);
        };
        playerInput.PlayerInputController.Aim.canceled += ctx => { aim = false; };

        playerInput.PlayerInputController.Sprint.performed += ctx => {
            if (GetComponent<PlayerInterface>().getCurrentMana() <= 0) { return; }

            Sprint(true); };
        playerInput.PlayerInputController.Sprint.canceled += ctx => { Sprint(false); };
    }

    // Start is called before the first frame update
    void Start()
    {
        //timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        body = GetComponent<Rigidbody>();
        gameTimeManager = GameObject.FindGameObjectWithTag("GameTimeManager").GetComponent<GameTimeManager>();
        winText.SetActive(false);
        shipNotInReachText.SetActive(false);
    }

    void Sprint(bool enable)
    {
        playerAnim.SetBool("sprint", enable);
    }

    void FixedUpdate()
    {
        currentMovementVector = playerInput.PlayerInputController.Locomotion.ReadValue<Vector2>();

        Vector2 newAimVector = playerInput.PlayerInputController.Camera.ReadValue<Vector2>();
        aimMovementVector = Vector2.Lerp(aimMovementVector, newAimVector, aiminSpeed * Time.deltaTime);



        float x = currentMovementVector.x;
        float z = currentMovementVector.y;
        x = x * Mathf.Sqrt(1f - 0.5f * z * z);
        z = z * Mathf.Sqrt(1f - 0.5f * x * x);

        currentMovementVector = new Vector2(x, z);
        prevMovement = Vector2.Lerp(prevMovement, currentMovementVector, 10 * Time.deltaTime);
        movePlayer();
        rotatePlayer();
        reportDistanceToGround();
        reportAim();
        aimCameraRotation();

        //var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //transform.Translate(dir * speed * Time.deltaTime);


        //Fire Spell (Left CLick)
        /*
        if (Input.GetButtonDown("Fire1") & energy.value >= 20)
        {
            energy.value = energy.value - 20;
            Debug.Log("FireSpell");
            var fire = Instantiate(firePrefab, fireStartPoint.position, fireStartPoint.rotation);
            fire.GetComponent<Rigidbody>().velocity = fireStartPoint.forward * fireSpeed;
        }

        //Time Stop
        if (Input.GetKeyDown("t") & !timeStopped) //& energy.value >= 40)
        {
            Player.GetComponent<Animator>().Play("Time Spell Animation");
            timedelay = 2000; //Change this to determine how long time is paused
            energy.value = energy.value - 40;
            Debug.Log("Time Stop");
            timemanager.StopTime();
            timeStopped = true;

        }
        if (timeStopped){
            timedelay--;
            if (timedelay < 1)
            {
                Debug.Log("Time Continue");

                timemanager.ContinueTime();
                timeStopped = false;
            }
        }*/


    }

    private void reportDistanceToGround()
    {

        //Get Current animation state
        //AnimatorStateInfo currentState =  playerAnim.GetCurrentAnimatorStateInfo(1);

        //if (!currentState.IsName("Hover")){return;}

        RaycastHit hit;
        bool grounDetected = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity);
        
        if (!grounDetected) { return; }

        float distanceToGround = hit.distance;
        playerAnim.SetFloat("distanceToGround", distanceToGround);
    }
    private IEnumerator DelayJump(float delay)
    {
        Debug.Log("Delay begin");
        yield return new WaitForSeconds(delay);
        Debug.Log("Jump after Delay");
        body.AddForce(Vector3.up * 450f, ForceMode.Impulse);
    }

    public void flare(){
        if (flareComplete & gameTimeManager.ship)
        {
            Debug.Log("Flare spell fired");
        }
        else if(flareComplete)
        {
            Debug.Log("Ship not in reach");
        }
        else
        {
            Debug.Log("Flare spell not complete");
        }
	}
	
	public void jump(){
        Debug.Log("High jump spell");
        body.AddForce(Vector3.up * 15f, ForceMode.Impulse);
	}

    public void collectFlarePiece()
    {
        flarePiecesFound = flarePiecesFound + 1;
        Debug.Log("Collected flare piece. Now: " + flarePiecesFound);
        if (flarePiecesFound == flarePiecesTotal)
        {
            flareComplete = true;
            Debug.Log("Collected all flare pieces");
        }
    }

    void movePlayer()
    {
        //Get Current animation state
        AnimatorStateInfo currentState = playerAnim.GetCurrentAnimatorStateInfo(1);

        if (currentState.IsName("Land")){
            return;
        }

        if (currentState.IsName("Hover")) {

            //Vector3 newVelocity = body.velocity + (new Vector3(prevMovement.x, 0, prevMovement.y) * 5f);
            body.velocity += transform.forward * prevMovement.y * 300f * Time.deltaTime;
           // body.velocity = Vector3.Lerp(body.velocity, newVelocity, Time.deltaTime * 15f);
            //body.AddForce(transform.forward * 50f, ForceMode.Impulse);

            return;
        }
        playerAnim.SetFloat("vel_x", prevMovement.x);
        playerAnim.SetFloat("vel_z", prevMovement.y);
    }

    void rotatePlayer()
    {
        //Get Current animation state
        AnimatorStateInfo currentState = playerAnim.GetCurrentAnimatorStateInfo(2);

        if (currentState.IsName("Death"))
        {
            return;
        }

        if (currentMovementVector.magnitude > 0f && !aim)
        {
            hMovement = Mathf.Lerp(hMovement, currentMovementVector.x, Time.deltaTime * 10f);

            vMovement = Mathf.Lerp(vMovement, currentMovementVector.y,
                Time.deltaTime * 10f);

            Vector3 relativeMovement = ConvertToCameraSpace(new Vector3(currentMovementVector.x, 0, currentMovementVector.y));

            Quaternion currentRotation = transform.rotation;

            Quaternion targetRotation = Quaternion.LookRotation(relativeMovement);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 5f * Time.deltaTime);
        }
    }


    //Code referenced from Unity's Third Person Camera demo 
    //https://www.youtube.com/watch?v=537B1kJp9YQ&t=316s
    void aimCameraRotation()
    {

            #region Horizontal Rotation
            lookTransform.transform.rotation *= Quaternion.AngleAxis(aimMovementVector.x * horizontalAimSensitivity, Vector3.up);
            #endregion

            #region Vertical Rotation
            //Inverted vertical rotation to feel more natural to third person shooters
            lookTransform.transform.rotation *= Quaternion.AngleAxis(aimMovementVector.y * verticalAimSensitivity, Vector3.left);

            // Clamp the values of the up and down rotations
            Vector3 angles = lookTransform.transform.localEulerAngles;
            angles.z = 0;

            float x_angle = angles.x;

            if (x_angle > 180 && x_angle < aimVerticalAngleMax)
            {
                angles.x = aimVerticalAngleMax;
            }
            else if (x_angle < 180 && x_angle > aimVerticalAngleMin)
            {
                angles.x = aimVerticalAngleMin;
            }

            lookTransform.transform.localEulerAngles = angles;
        #endregion
        if (aim)
        {
            transform.rotation = Quaternion.Euler(0, lookTransform.transform.rotation.eulerAngles.y, 0);
            lookTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }
    }

    void reportAim()
    {
        playerAnim.SetBool("aim", aim);
    }

    public void reportDeath()
    {
        playerAnim.SetTrigger("death");
        playerInput.Disable();
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void PlayFootstepAudio()
    {
        footSteps.Play() ;
    }

    public Vector3 ConvertToCameraSpace(Vector3 rotationVector)
    {
        float rotationY = rotationVector.y;
        Vector3 forwardDir = Camera.main.transform.forward;
        Vector3 rightDir = Camera.main.transform.right;

        //Zero out the Y direction to ignore up/down camera angles
        forwardDir.y = 0;
        rightDir.y = 0;

        //Normalize again to ensure they have a magnitude of 1
        forwardDir = forwardDir.normalized;
        rightDir = rightDir.normalized;

        //Rotate X and Z components of the rotation Vector to be in the camera space
        Vector3 forwardDirZProduct = rotationVector.z * forwardDir;
        Vector3 rightDirXProduct = rotationVector.x * rightDir;

        Vector3 cameraSpaceRotationVector = forwardDirZProduct + rightDirXProduct;

        return cameraSpaceRotationVector;
    }

    private void OnEnable()
    {
        playerInput.PlayerInputController.Enable();
    }

    private void OnDisable()
    {
        playerInput.PlayerInputController.Disable();
    }

    private void TriggerDeathEvent() {
        EventManager.TriggerEvent<PlayerDeathEvent>();
    }

    // Update is called once per frame
    /*    void Update()
        {
            var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.Translate(dir * speed * Time.deltaTime);

            //Fire Spell (Left CLick)
            if (Input.GetButtonDown("Fire1") & energy.value >= 20)
            {
                energy.value = energy.value - 20;
                Debug.Log("FireSpell");
                var fire = Instantiate(firePrefab, fireStartPoint.position, fireStartPoint.rotation);
                fire.GetComponent<Rigidbody>().velocity = fireStartPoint.forward * fireSpeed;
            }

            //Time Stop
            if (Input.GetKeyDown("t") & !timeStopped) //& energy.value >= 40)
            {
                Player.GetComponent<Animator>().Play("Time Spell Animation");
                timedelay = 2000; //Change this to determine how long time is paused
                energy.value = energy.value - 40;
                Debug.Log("Time Stop");
                timemanager.StopTime();
                timeStopped = true;

            }
            if (timeStopped)
            {
                timedelay--;
                if (timedelay < 1)
                {
                    Debug.Log("Time Continue");

                    timemanager.ContinueTime();
                    timeStopped = false;
                }
            }

        }*/



    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log("moving");
    }

}
