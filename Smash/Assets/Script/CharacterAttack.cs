using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterAttack : MonoBehaviour
{
    protected PlayerInput playerInput;
    protected InputAction attackAction;
    protected InputAction skillAttackAction;
    protected InputAction ultimeAttackAction;

    public float basicAttackRate = 0.2f;
    protected float basicAttackCooldown;

    public float chargeTimeTreshold = 0.5f;
    protected float holdTime = 0f;
    protected bool isHoldingAttack = false;

    public bool canUseUltimate = false;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        attackAction = playerInput.actions["Attack"];
        skillAttackAction = playerInput.actions["Skill"];
        ultimeAttackAction = playerInput.actions["Ultimate"];

    }
    
    protected virtual void OnEnable()
    {
        skillAttackAction.performed += _ => SkillAttack();
        ultimeAttackAction.performed += _ =>
        {
            if (canUseUltimate)
            {
                UltimeAttack();
                canUseUltimate = false;
            }
        };

        attackAction.started += _ =>
        {
            isHoldingAttack = true;
            holdTime = 0f;
        };
        attackAction.canceled += _ =>
        {
            isHoldingAttack = false;
            if (holdTime >= chargeTimeTreshold)
                ChargeAttack();
            else
                holdTime = 0f;
        };
    }

    protected virtual void OnDisable()
    {
        skillAttackAction.performed -= _ => SkillAttack();
        ultimeAttackAction.performed -= _ => UltimeAttack();
        attackAction.started -= _ => { };
        attackAction.canceled -= _ => { };
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isHoldingAttack)
        {
            holdTime += Time.deltaTime;

            if(holdTime < chargeTimeTreshold && Time.time >= basicAttackCooldown)
            {
                basicAttackCooldown = Time.time + basicAttackRate;
                BasicAttack();
            }
        }
    }

    protected abstract void BasicAttack();
    protected abstract void ChargeAttack();
    protected abstract void SkillAttack();
    protected abstract void UltimeAttack();
}
