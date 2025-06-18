using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AttackType
{
    Basic,
    Skill,
    Ultimate
}

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

    public Vector2 lastMoveDirection = Vector2.right;

    //Fields to modify while making a character
    [Header("Attack Data - Basic Attack")]
    [SerializeField] public float basicAttackDuration;
    [SerializeField] public float basicAttackDelay;
    [SerializeField] public float basicAttackRate;
    [SerializeField] public float basicAttackDamage;

    [Header("Attack Data - Charge Attack")]
    [SerializeField] public float chargeAttackDuration;
    [SerializeField] public float chargeAttackDelay;
    [SerializeField] public float chargeAttackRate;
    [SerializeField] public float chargeAttackDamage;
    [SerializeField] public float chargeTimeTreshold;

    [Header("Attack Data - Skill")]
    [SerializeField] public float skillDuration;
    [SerializeField] public float skillDelay;
    [SerializeField] public float skillRate;
    [SerializeField] public float skillDamage;

    [Header("Attack Data - Ultimate")]
    [SerializeField] public float ultimateDuration;
    [SerializeField] public float ultimateDelay;
    [SerializeField] public float ultimateRate;
    [SerializeField] public float ultimateDamage;


    public AttackType currentAttackType = AttackType.Basic;

    #region Virtual Properties - Override in Character Children

    public virtual float BasicDamage => basicAttackDamage;
    public virtual float BasicDuration => basicAttackDuration;
    public virtual float BasicDelay => basicAttackDelay;
    public virtual float BasicRate => basicAttackRate;

    public virtual float ChargeDamage => chargeAttackDamage;
    public virtual float ChargeDuration => chargeAttackDuration;
    public virtual float ChargeDelay => chargeAttackDelay;
    public virtual float ChargeRate => chargeAttackRate;

    public virtual float SkillDamage => skillDamage;
    public virtual float SkillDuration => skillDuration;
    public virtual float SkillDelay => skillDelay;
    public virtual float SkillRate => skillRate;

    public virtual float UltimateDamage => ultimateDamage;
    public virtual float UltimateDuration => ultimateDuration;
    public virtual float UltimateDelay => ultimateDelay;
    public virtual float UltimateRate => ultimateRate;

    public virtual float BasicKnockback => 5f;
    public virtual float ChargeKnockback => 7f;
    public virtual float SkillKnockback => 10f;
    public virtual float UltimateKnockback => 15f;

    #endregion

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
                UltimeAttack();
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
        if (!canUseSkill)
            return;
        Debug.Log("Skill");

        StartCoroutine(UseSkill());
    }
    protected virtual void UltimeAttack()
    {
        Debug.Log("Ultimate");

        StartCoroutine(UseUlt());
    }
    protected abstract void ParadeAction();
}