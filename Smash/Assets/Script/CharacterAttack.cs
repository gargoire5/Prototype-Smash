using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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

    private PlayerController _pc = null;

    private bool _areInputsSet = false; 

    protected float basicAttackCooldown;

    protected float holdTime = 0f;
    public bool isHoldingAttack = false;

    public bool canUseUltimate = false;
    public bool canUseSkill = true;

    public GameObject selectedHitbox = null;

    [SerializeField]
    public GameObject PlayerTag;

    [Header("Hitboxes")]
    public GameObject hitboxRight;
    public GameObject hitboxLeft;
    public GameObject hitboxUp;
    public bool isAttacking = false;

    public Vector2 lastMoveDirection = Vector2.right;

    public Texture CharacterRender;
    public Texture CharacterSkillIcon;
    public Texture CharacterUltIcon;

    //Fields to modify while making a character
    [Header("Attack Data - Basic Attack")]
    [SerializeField] public float basicAttackDuration;
    [SerializeField] public float basicAttackDelay;
    [SerializeField] public float basicAttackRate;
    [SerializeField] public float basicAttackDamage;
    [SerializeField] public float basicAttackKnockback;

    [Header("Attack Data - Charge Attack")]
    [SerializeField] public float chargeAttackDuration;
    [SerializeField] public float chargeAttackDelay;
    [SerializeField] public float chargeAttackRate;
    [SerializeField] public float chargeAttackDamage;
    [SerializeField] public float chargeAttackKnockback;
    [SerializeField] public float chargeTimeTreshold;

    [Header("Attack Data - Skill")]
    [SerializeField] public float skillDuration;
    [SerializeField] public float skillDelay;
    [SerializeField] public float skillRate;
    [SerializeField] public float skillDamage;
    [SerializeField] public float skillKnockback;

    [Header("Attack Data - Ultimate")]
    [SerializeField] public float ultimateDuration;
    [SerializeField] public float ultimateDelay;
    [SerializeField] public float ultimateRate;
    [SerializeField] public float ultimateDamage;
    [SerializeField] public float ultimateKnockback;


    //0 = Menu SFX
    //1 = Start SFX
    //2 = Death SFX
    //3 = Skill SFX
    //4 = Ult SFX
    [SerializeField] public List<AudioClip> SFXList = new List<AudioClip>();

    [SerializeField] 
    private AudioSource _audioSource;
    private AudioClip _currentAudioClip;


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

    public virtual float BasicKnockback => basicAttackKnockback;
    public virtual float ChargeKnockback => chargeAttackKnockback;
    public virtual float SkillKnockback => skillKnockback;
    public virtual float UltimateKnockback => ultimateKnockback;

    #endregion

    public bool IsTryingToAttackUp()
    {
        return jumpAction.IsPressed() && attackAction.IsPressed();
    }
    protected virtual void Awake()
    {
        TryGetComponent<PlayerInput>(out playerInput);

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput est manquant sur " + gameObject.name);
            return;
        }
    }

    private void Start()
    {
        _currentAudioClip = SFXList[1];
        _audioSource.PlayOneShot(_currentAudioClip);

        _pc = GetComponent<PlayerController>();
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
        if (_areInputsSet)
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
        }
    }

    protected virtual void BasicAttack()
    {
        if (_pc.isMove)
        {
            currentAttackType = AttackType.Basic;

            if (jumpAction.IsPressed())
                selectedHitbox = hitboxUp;
            else if (lastMoveDirection.x > 0)
                selectedHitbox = hitboxRight;
            else
                selectedHitbox = hitboxLeft;
            StartCoroutine(ActivateHitboxCollider(selectedHitbox));
        }
    }

    public IEnumerator ActivateHitboxCollider(GameObject hitbox)
    {

        if (hitbox == null)
        {
            Debug.LogError("La hitbox est NULL !");
            yield break;
        }

        yield return new WaitForSeconds(BasicDelay);

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

        col.enabled = true;

        yield return new WaitForSeconds(BasicDuration);

        col.enabled = false;
        if (renderer != null)
            renderer.enabled = false;
    }

    public IEnumerator UseSkill()
    {
        canUseSkill = false;

        _currentAudioClip = SFXList[3];
        _audioSource.PlayOneShot(_currentAudioClip);

        yield return new WaitForSecondsRealtime(SkillRate);

        canUseSkill = true;
    }

    public IEnumerator UseUlt()
    {
        canUseUltimate = false;

        _currentAudioClip = SFXList[4];
        _audioSource.PlayOneShot(_currentAudioClip);

        yield return new WaitForSecondsRealtime(UltimateRate);

        canUseUltimate = true;
    }

    protected abstract void ChargeAttack();
    protected virtual void SkillAttack()
    {
        if (!canUseSkill || !_pc.isMove)
            return;

        currentAttackType = AttackType.Skill;
        StartCoroutine(UseSkill());
    }
    protected virtual void UltimateAttack()
    {
        if (!_pc.isMove)
            return;

        currentAttackType = AttackType.Ultimate;
        StartCoroutine(UseUlt());
    }
    protected abstract void ParadeAction();

    public void SetupInputActions(InputActionAsset playerInputToSet)
    {
        playerInput.actions = playerInputToSet;
        var actions = playerInput.actions;
        attackAction = actions["Attack"];
        skillAttackAction = actions["Skill"];
        ultimeAttackAction = actions["Ultimate"];
        paradeAction = actions["Parade"];
        moveAction = actions["Move"];
        jumpAction = actions["Jump"];

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

        _areInputsSet = true;
    }
}