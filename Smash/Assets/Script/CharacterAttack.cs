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

    public float basicAttackRate = 0.2f;
    protected float basicAttackCooldown;

    public float chargeTimeTreshold = 0.5f;
    protected float holdTime = 0f;
    public bool isHoldingAttack = false;

    public bool canUseUltimate = false;

    [Header("Hitboxes")]
    public GameObject hitboxRight;
    public GameObject hitboxLeft;
    public GameObject hitboxUp;
    public float attackDuration = 0.2f;
    public bool isAttacking = false;

    private Vector2 lastMoveDirection = Vector2.right;

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

        /*if (isHoldingAttack)
        {
            holdTime += Time.deltaTime;

            if (holdTime < chargeTimeTreshold && Time.time >= basicAttackCooldown)
            {
                basicAttackCooldown = Time.time + basicAttackRate;
                BasicAttack();
            }
        }*/
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

    private IEnumerator ActivateHitboxCollider(GameObject hitbox)
    {
        Debug.Log($"Activation demandée pour : {hitbox?.name}");

        if (hitbox == null)
        {
            Debug.LogError("La hitbox est NULL !");
            yield break;
        }

        Collider col = hitbox.GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("Aucun Collider trouvé sur " + hitbox.name);
            yield break;
        }

        Debug.Log("Collider trouvé, activation !");
        col.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        col.enabled = false;
        Debug.Log("Collider désactivé : " + hitbox.name);
    }

    protected abstract void ChargeAttack();
    protected abstract void SkillAttack();
    protected abstract void UltimeAttack();
    protected abstract void ParadeAction();
}