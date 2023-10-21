using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float friction = 10.0f;
    public float pushForce = 10.0f;

    //toggle for ice level friction
    public bool isOnIce = false;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;
    
    public AudioClip[] footstepSounds;
    public float footstepIntervalWalk = 0.5f; // time interval for walking footstep sounds
    public float footstepIntervalRun = 0.3f;  // time interval for running footstep sounds

    private float lastFootstepTime = 0f;
    
    private AudioSource playerAudioSource;



    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        playerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // movements
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // apply friction to the player's movement, unless they are on ice
        if (characterController.isGrounded && moveDirection.magnitude > 0 && !isOnIce)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, friction * Time.deltaTime);
        }

        // apply friction if the player is not moving
        if (curSpeedX == 0 && curSpeedY == 0)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, friction * Time.deltaTime);
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }


        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // player & camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        
        float footstepInterval = isRunning ? footstepIntervalRun : footstepIntervalWalk; // choose the appropriate footstep interval

        // check for if the player is moving fast enough to play footstep sounds
        if (moveDirection.z !=0 && characterController.isGrounded && movementDirectionY <= 0)
        {
            // check if enough time has passed since the last footstep sound
            if (Time.time - lastFootstepTime > footstepInterval)
            {
                PlayRandomFootstepSound();

                // update the last footstep time
                lastFootstepTime = Time.time;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            PushGameObjects();
        }
    }
    
    void PlayRandomFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            playerAudioSource.PlayOneShot(footstepSounds[randomIndex]);
        }
    }


    void PushGameObjects()
    {
        // set the sphere's radius and position to match your desired push range and location
        float pushRadius = 5.0f;
        Vector3 pushPosition = transform.position + transform.forward * pushRadius;

        // get a list of colliders within the sphere
        Collider[] colliders = Physics.OverlapSphere(pushPosition, pushRadius);

        // filter out colliders that don't have a rigidbody component
        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rigidbodies.Add(rb);
            }
        }


        // apply a force to each of the rigidbodies
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
        }
    }

}