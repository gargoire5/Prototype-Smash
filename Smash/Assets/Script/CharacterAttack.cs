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

    [Header("Hitboxes")]
    public GameObject hitboxRight;
    public GameObject hitboxLeft;
    public GameObject hitboxUp;
    public bool isAttacking = false;

    private Vector2 lastMoveDirection = Vector2.right;

    //Fields to modify while making a character
    [Header("Attack Data - Basic Attack")]
    public float basicAttackDuration = 0.2f;
    public float basicAttackDelay = 0.0f;
    public float basicAttackRate = 0.2f;
    public float basicAttackDamage = 5.0f;

    [Header("Attack Data - Charge Attack")]
    public float chargeAttackDuration = 0.2f;
    public float chargeAttackDelay = 0.0f;
    public float chargeAttackRate = 0.2f;
    public float chargeAttackDamage = 7.5f;
    public float chargeTimeTreshold = 0.5f;

    [Header("Attack Data - Skill")]
    public float skillDuration = 0.2f;
    public float skillDelay = 0.0f;
    public float skillRate = 0.2f;
    public float skillDamage = 10.0f;

    [Header("Attack Data - Ultimate")]
    public float ultimateDuration = 0.2f;
    public float ultimateDelay = 30.0f;
    public float ultimateRate = 0.2f;
    public float ultimateDamage = 20.0f;

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

        attackAction.performed += _ => BasicAttack();
    }

    protected virtual void OnDisable()
    {
        if (skillAttackAction != null) skillAttackAction.performed -= _ => SkillAttack();
        if (ultimeAttackAction != null) ultimeAttackAction.performed -= _ => UltimeAttack();
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

        GameObject selectedHitbox = null;

        if (jumpAction.IsPressed())
            selectedHitbox = hitboxUp;
        else if (lastMoveDirection.x > 0)
            selectedHitbox = hitboxRight;
        else
            selectedHitbox = hitboxLeft;

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
        if (renderer !=null)
            renderer.enabled = false;
        Debug.Log("Collider désactivé : " + hitbox.name);
    }

    protected abstract void ChargeAttack();
    protected abstract void SkillAttack();
    protected abstract void UltimeAttack();
    protected abstract void ParadeAction();
}