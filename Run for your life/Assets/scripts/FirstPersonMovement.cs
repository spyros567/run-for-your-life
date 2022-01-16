using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.18f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;

    //stamina
    private float maxStamina = 100.0f;
    private float currentStamina;
    public bool hasRegenerated = true;
    public bool sprinting = false;
    public bool canSprint = false;
    [SerializeField] float staminaDrain;
    [SerializeField] float staminaGain;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }
    void Update()
    {
        Movement();
        Jump();
        Debug.Log("Current Stamina : " + currentStamina);
        if(currentStamina>0)
        {
            canSprint = true;
        }
        else
        {
            canSprint = false;
        }

        if(currentStamina<0)
        {
            currentStamina = 0;
        }
    }

    void Movement() 
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && canSprint &&hasRegenerated)
        {
            speed = 20f;
            UseStamina();
        }
        else
        {
            RegenStamina();
            speed = 12f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        //check if grounded
        //isGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, groundLayer);

        //set velocity.y after touching the ground to avoid inf
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = gravity;
        }

        //jump
        if (controller.isGrounded && Input.GetKey(KeyCode.Space))
        {
            //anim.SetBool("Jump", true);
            velocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
        }

        //apply gravity
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        //move controller
        controller.Move(velocity * Time.deltaTime);
    }


    public void UseStamina()
    {
        if (hasRegenerated)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            if (currentStamina <= 0 )
            {
                hasRegenerated = false;
            }
        }

    }

    public void RegenStamina()
    {
        if (currentStamina < maxStamina - 0.01)
        {

            currentStamina += staminaGain * Time.deltaTime;
            if (currentStamina >= maxStamina-20f)
            {
                hasRegenerated = true;
            }
        }

    }
}
