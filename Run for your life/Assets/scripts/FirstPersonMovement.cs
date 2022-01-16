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
    public LayerMask enemyMask;
    [SerializeField] GameObject hitbox;

    [SerializeField] private Animator swordAnim;
    [SerializeField] private int damage;
    [SerializeField] private bool canAttack;

    //stamina
    private float maxStamina = 100.0f;
    private float currentStamina;
    public bool hasRegenerated = true;
    public bool sprinting = false;
    public bool canSprint = false;
    [SerializeField] float staminaDrain;
    [SerializeField] float staminaGain;
    [SerializeField] float timer=0;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        canAttack = true;
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible = false;
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
        if(Input.GetKeyDown(KeyCode.Mouse0)&&canAttack)
        {
            Attack();
        }
        if(!canAttack)
        {
            timer += Time.deltaTime;
            if(timer>1f)
            {
                timer = 0;
                canAttack = true;
            }
        }
    }

    void Attack()
    {
        swordAnim.SetTrigger("Attack");
        canAttack = false;
        
        if(Physics.CheckSphere(this.transform.position,2f,enemyMask))
        {
            Collider[] enemies = Physics.OverlapSphere(this.transform.position, 2f, enemyMask);
            foreach(Collider enemy in enemies)
            {
                enemy.gameObject.GetComponent<EnemyAI>().GetDamage(damage);
            }
        }
    }

    IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(swordAnim.GetCurrentAnimatorStateInfo(0).length + swordAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        canAttack = true;
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
