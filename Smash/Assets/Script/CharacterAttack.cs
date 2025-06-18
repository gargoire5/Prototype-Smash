using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterAttack : MonoBehaviour
{
    protected PlayerInput playerInput;
    protected InputAction attackAction;
    protected InputAction skillAttackAction;
    protected InputAction ultimeAttackAction;
    protected InputAction paradeAction;
    protected InputAction moveAction;
    public InputAction jumpAction;

    protected float basicAttackCooldown;

    protected float holdTime = 0f;
    public bool isHoldingAttack = false;

    public bool canUseUltimate = false;
    public bool canUseSkill = true;

    public GameObject selectedHitbox = null;

    [Header("Hitboxes")]
    public GameObject hitboxRight;
    public GameObject hitboxLeft;
    public GameObject hitboxUp;
    public bool isAttacking = false;

    private Vector2 lastMoveDirection = Vector2.right;

    //Fields to modify while making a character
    [Header("Attack Data - Basic Attack")]
    public float basicAttackDuration;
    public float basicAttackDelay;
    public float basicAttackRate;
    public float basicAttackDamage;

    [Header("Attack Data - Charge Attack")]
    public float chargeAttackDuration;
    public float chargeAttackDelay;
    public float chargeAttackRate;
    public float chargeAttackDamage;
    public float chargeTimeTreshold;

    [Header("Attack Data - Skill")]
    public float skillDuration;
    public float skillDelay;
    public float skillRate;
    public float skillDamage;

    [Header("Attack Data - Ultimate")]
    public float ultimateDuration;
    public float ultimateDelay;
    public float ultimateRate;
    public float ultimateDamage;

    public bool IsTryingToAttackUp()
    {
        return jumpAction.IsPressed() && attackAction.IsPressed();
    }
    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput est manquant sur " + gameObject.name);
            return;
        }

        var actions = playerInput.actions;
        attackAction = actions["Attack"];
        skillAttackAction = actions["Skill"];
        ultimeAttackAction = actions["Ultimate"];
        paradeAction = actions["Parade"];
        moveAction = actions["Move"];
        jumpAction = actions["Jump"];
    }

    protected virtual void OnEnable()
    {
        skillAttackAction.performed += _ =>
        {
            if (canUseSkill)
                SkillAttack();
        };
        ultimeAttackAction.performed += _ =>
        {
            if (canUseUltimate)
                UltimateAttack();
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

        attackAction.performed += _ => BasicAttack();
    }

    protected virtual void OnDisable()
    {
        if (skillAttackAction != null) skillAttackAction.performed -= _ => SkillAttack();
        if (ultimeAttackAction != null) ultimeAttackAction.performed -= _ => UltimateAttack();
        if (attackAction != null)
        {
            attackAction.started -= _ => { };
            attackAction.canceled -= _ => { };
        }
    }

    protected virtual void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput.x != 0)
        {
            lastMoveDirection = new Vector2(Mathf.Sign(moveInput.x), 0);

            if (jumpAction.IsPressed())
                selectedHitbox = hitboxUp;
            else if (lastMoveDirection.x > 0)
                selectedHitbox = hitboxRight;
            else
                selectedHitbox = hitboxLeft;
        }

        if (isHoldingAttack)
        {
            holdTime += Time.deltaTime;

            if (holdTime < chargeTimeTreshold && Time.time >= basicAttackCooldown)
            {
                basicAttackCooldown = Time.time + basicAttackRate;
                BasicAttack();
            }
        }
    }

    protected virtual void BasicAttack()
    {
        Debug.Log("BasicAttack appelée");

        StartCoroutine(ActivateHitboxCollider(selectedHitbox));
    }

    public IEnumerator ActivateHitboxCollider(GameObject hitbox)
    {
        Debug.Log($"Activation demandée pour : {hitbox?.name}");

        if (hitbox == null)
        {
            Debug.LogError("La hitbox est NULL !");
            yield break;
        }

        yield return new WaitForSeconds(basicAttackDelay);

        Collider col = hitbox.GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("Aucun Collider trouvé sur " + hitbox.name);
            yield break;
        }

        MeshRenderer renderer = hitbox.GetComponent<MeshRenderer>() ?? hitbox.GetComponentInChildren<MeshRenderer>();

        if (renderer == null)
        {
            Debug.LogWarning("Aucun mesh");
        }
        col.enabled = true;
        if (renderer != null)
            renderer.enabled = true;

        Debug.Log("Collider trouvé, activation !");
        col.enabled = true;

        yield return new WaitForSeconds(basicAttackDuration);

        col.enabled = false;
        if (renderer != null)
            renderer.enabled = false;
        Debug.Log("Collider désactivé : " + hitbox.name);
    }

    public IEnumerator UseSkill()
    {
        canUseSkill = false;

        yield return new WaitForSecondsRealtime(skillRate);

        canUseSkill = true;
    }

    public IEnumerator UseUlt()
    {
        canUseUltimate = false;

        yield return new WaitForSecondsRealtime(ultimateRate);

        canUseUltimate = true;
    }

    protected abstract void ChargeAttack();
    protected virtual void SkillAttack()
    {
        Debug.Log("Skill");

        StartCoroutine(UseSkill());
    }
    protected virtual void UltimateAttack()
    {
        Debug.Log("Ultimate");

        StartCoroutine(UseUlt());
    }
    protected abstract void ParadeAction();
}