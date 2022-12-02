using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Components
    private Rigidbody2D rb;
    #endregion

    #region Variables
    public float moveSpeed = 3f;
    public float setMoveSpeed = 3f;
    private float moveX;
    private float moveY;
    private Vector2 moveDirection;
    private PlayerHealth playerHealthManager;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerHealthManager = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerHealthManager.isPlayerDead)
        {
            ProcessInputs();
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        if (!(moveX == 0) && !(moveY == 0))
        {
            moveSpeed = setMoveSpeed * (1 / Mathf.Sqrt(2));
        }
        else
        {
            moveSpeed = setMoveSpeed;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2 (moveX * moveSpeed, moveY * moveSpeed);
    }

}
