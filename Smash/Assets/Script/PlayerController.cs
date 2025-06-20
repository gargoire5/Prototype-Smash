using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction JumpAction;

    private bool _areInputsSet = false;

    public float speed = 5f;
    public float moveForce = 10f;

    public Rigidbody rb;

    public Vector2 inputDirection;

    public float bounceForce = 20f;

    public float maxHeals = 10;
    public float heals;

    public int maxJump = 3;
    public int numJump = 3;
    private float lastDirc;

    public float gravityMulti = 3f;

    void Start()
    {
        MoveAction.Enable();
        JumpAction.Enable();
        heals = maxHeals;
    }

    private void Awake()
    {
        //JumpAction.started += Jump;
    }


    // Update is called once per frame
    void Update()
    {
        inputDirection = MoveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 extraGravity = Physics.gravity * (gravityMulti - 1f);
        rb.AddForce(extraGravity, ForceMode.Acceleration);

        if (_areInputsSet)
            MovePlayer();
    }


    private void MovePlayer()
    {
        Vector3 move = transform.forward * inputDirection.x;

        if (inputDirection.x != 0)
        {
            rb.AddForce(move * moveForce, ForceMode.Force);
            lastDirc = inputDirection.x;
        }   
    }

    void Jump(InputAction.CallbackContext obj)
    {
        CharacterAttack attackScript = GetComponent<CharacterAttack>();
        if (attackScript != null && attackScript.IsTryingToAttackUp())
        {
            Debug.Log("Saut annulé : attaque vers le haut");
            return;
        }

        if (numJump > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            numJump--;
        }
    }

    public void SetupInputActions(InputActionAsset playerInputToSet)
    {
        var actions = playerInputToSet;

        gameObject.GetComponent<PlayerInput>().actions = actions;

        MoveAction = actions["Move"];
        JumpAction = actions["Jump"];

        JumpAction.started += Jump;
        _areInputsSet = true;

        gameObject.GetComponent<CharacterAttack>().SetupInputActions(playerInputToSet);
    }
}
