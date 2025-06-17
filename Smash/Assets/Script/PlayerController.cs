using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference MoveAction;
    public InputActionReference JumpAction;

    public float speed = 5f;

    public Rigidbody rb;

    private Vector2 inputDirection;

    public float bounceForce = 10f;

    public float maxHeals = 10;
    public float heals;

    public int numJump = 3;

    void Start()
    {
        MoveAction.action.Enable();
        JumpAction.action.Enable();
        heals = maxHeals;
    }

    private void Awake()
    {
        JumpAction.action.started += Jump;
    }


    // Update is called once per frame
    void Update()
    {
        inputDirection = MoveAction.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }


    private void MovePlayer()
    {
        Vector3 move = transform.forward * inputDirection.x;
        Vector3 velocity = move * speed;
        Vector3 rbVelocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        rb.velocity = rbVelocity;
    }

    void Jump(InputAction.CallbackContext obj)
    {
        var attackScript = GetComponent<CharacterAttack>();
        if (attackScript != null && attackScript.IsTryingToAttackUp())
        {
            Debug.Log("Annulation du saut : attaque vers le haut");
            return;
        }

        if (numJump > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            numJump--;
        }
    }
}
