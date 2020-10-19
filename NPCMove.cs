using UnityEngine;
using System.Collections;

public class BotMove : MonoBehaviour 
{
    private BotState botState;
    public Animator animator;
    private Vector3 moveDirection;//Вектор для определения движения (направление движения используется)
    public Vector3 aiDirection;
    public float speedWalk = 30;//скорость движения вперед. Используется в функции Move
    public float speedRotate = 100;
    public Quaternion rotateDirection;
    private CharacterController characterController;
    public LayerMask moveCollisionLayerMask;


    void Start()
    {
        botState = GetComponent<BotState>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (botState.botStatus != BotStatus.Dead)
        {
            if (aiDirection != new Vector3(0f, 0f, 0f))
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDirection, speedRotate * Time.deltaTime);
            }
        }
    }

    void FixedUpdate()
    {
        if (botState.botStatus != BotStatus.Dead)
        {
            MoveBot();
        }
    }

    void MoveBot()
    {
        if (aiDirection != new Vector3(0f, 0f, 0f))
        {
            if (botState.startRepear != false) { botState.startRepear = false; animator.SetBool("repear", false); }
            if (botState.startReload != false) { botState.startReload = false; animator.SetBool("repear", false); }
        }
        
        if (botState.botStatus != BotStatus.PlayerControl)
        {
            CheckCollisionCapsul();
        }

        moveDirection = aiDirection;
        AnimateMove(moveDirection);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speedWalk;
        moveDirection += Physics.gravity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void CheckCollisionCapsul()
    {
        RaycastHit hitCollision;
        Vector3  tmpPosition1 = transform.position + (Vector3.up * (characterController.radius + 1));
        Vector3  tmpPosition2 = tmpPosition1 + (Vector3.up * characterController.height);
        Vector3  tmpDirection = transform.TransformDirection(aiDirection);

        if (Physics.CapsuleCast(tmpPosition1, tmpPosition2, characterController.radius, tmpDirection, out hitCollision, 2f, moveCollisionLayerMask))
        {
            aiDirection = new Vector3(0f, 0f, 0f);
            RandomPosition();

            tmpPosition1 = transform.position + (Vector3.up * (characterController.radius + 1));
            tmpPosition2 = tmpPosition1 + (Vector3.up * characterController.height);
            tmpDirection = transform.TransformDirection(aiDirection);

            if (Physics.CapsuleCast(tmpPosition1, tmpPosition2, characterController.radius, tmpDirection, out hitCollision, 2f, moveCollisionLayerMask))
            {
                aiDirection = new Vector3(0f, 0f, 0f);
                //botAi.myTime += 3;
                //botAi.myRandomCheckTime += 3;
            }
        }   
    }

    void RandomPosition()
    {
        int rnd = Random.Range(0, 8);

        if (rnd == 0)
        {
            aiDirection = new Vector3(0f, 0f, -0.1f);
        }

        if (rnd == 1)
        {
            aiDirection = new Vector3(-0.1f, 0f, 0f);
        }

        if (rnd == 2)
        {
            aiDirection = new Vector3(0.1f, 0f, 0f);
        }

        if (rnd == 3)
        {
            aiDirection = new Vector3(0f, 0f, 0.1f);
        }

        if (rnd == 4)
        {
            aiDirection = new Vector3(-0.1f, 0f, 0f);
        }

        if (rnd == 5)
        {
            aiDirection = new Vector3(0.1f, 0f, 0f);
        }

        if (rnd == 6)
        {
            aiDirection = new Vector3(0f, 0f, 0.1f);
        }

        if (rnd == 7)
        {
            aiDirection = new Vector3(0.1f, 0f, 0f);
        }

    }

    void AnimateMove(Vector3 direction)
    {
        if (direction == new Vector3(0f, 0f, 0.1f))
        {
            animator.SetInteger("walk", 1);
        }

        if (direction == new Vector3(0f, 0f, -0.1f))
        {
            animator.SetInteger("walk", 3);
        }

        if (direction == new Vector3(0.1f, 0f, 0f))
        {
            animator.SetInteger("walk", 2);
        }

        if (direction == new Vector3(-0.1f, 0f, 0f))
        {
            animator.SetInteger("walk", 4);
        }

        if (direction == new Vector3(0f, 0f, 0f))
        {
            animator.SetInteger("walk", 0);
        }
    }