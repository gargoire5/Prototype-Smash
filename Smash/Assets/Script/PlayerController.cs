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
    public InputAction ParadeAction;

    private bool _areInputsSet = false;

    public float speed = 5f;
    public float moveForce = 10f;

    public Rigidbody rb;
    public DamageReceiver dr;

    public Vector2 inputDirection;

    public float bounceForce = 20f;

    public float maxHeals = 10;
    public float heals;

    public int maxJump = 3;
    public int numJump = 3;

    private float lastDirc;

    public bool isMove = true;
    public bool IsGrounded = false;
    public bool HasFastFallen = false;

    public float gravityMulti = 3f;

    void Start()
    {
        MoveAction.Enable();
        JumpAction.Enable();
        ParadeAction.Enable();
        heals = maxHeals;

        dr = GetComponent<DamageReceiver>();
    }

    private void Awake()
    {

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

        isMove = !dr.isBlocking;

        if (_areInputsSet && isMove)
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

        if (numJump > 0 && isMove)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            numJump--;
        }
    }

    void Parade(InputAction.CallbackContext obj)
    {
        if (IsGrounded == false)
        {
            if (HasFastFallen == false)
            {
                rb.AddForce(Vector3.up * -bounceForce * 2, ForceMode.Impulse);
                HasFastFallen = true;
            }
        }
        else
        {
            dr.Parade();
        }
    }

    void StopParade(InputAction.CallbackContext obj)
    {
        if (IsGrounded == true)
        {
            dr.StopParade(); 
        }
    }

    public void SetupInputActions(InputActionAsset playerInputToSet)
    {
        var actions = playerInputToSet;

        gameObject.GetComponent<PlayerInput>().actions = actions;

        MoveAction = actions["Move"];
        JumpAction = actions["Jump"];
        ParadeAction = actions["Parade"];

        JumpAction.started += Jump;
        ParadeAction.performed += Parade;
        ParadeAction.canceled += StopParade;

        _areInputsSet = true;

        gameObject.GetComponent<CharacterAttack>().SetupInputActions(playerInputToSet);
    }
}
