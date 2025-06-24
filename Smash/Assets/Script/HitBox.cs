using System.Collections;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 10f;
    public CharacterAttack owner = null;

    [SerializeField]
    private Vector3 direction = Vector3.zero;

    private void Start()
    {
        if (owner == null && transform.parent != null)
            transform.parent.TryGetComponent(out owner);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null || other.gameObject == owner.gameObject)
        {
            Debug.Log("Pas de parent");
            return;
        }

        if (other.TryGetComponent(out DamageReceiver receiver))
        {
            // Dégâts dynamiques selon le type d’attaque
            float actualDamage = damage;
            float knockbackForce = 0f;

            if (receiver.isCounter)
            {
                GetComponent<DamageReceiver>().TakeDamage(actualDamage * 2, -direction * knockbackForce);
            }

            switch (owner.currentAttackType)
            {
                case AttackType.Basic:
                    actualDamage = owner.BasicDamage;
                    knockbackForce = owner.BasicKnockback;
                    break;
                case AttackType.Skill:
                    actualDamage = owner.SkillDamage;
                    knockbackForce = owner.SkillKnockback;
                    break;
                case AttackType.Ultimate:
                    actualDamage = owner.UltimateDamage;
                    knockbackForce = owner.UltimateKnockback;
                    break;
            }

            if (direction == Vector3.zero)
                direction = (other.transform.position - owner.transform.position).normalized;

            receiver.TakeDamage(actualDamage, direction * knockbackForce);
            Debug.Log($"{owner.name} inflige {actualDamage} dmg et {knockbackForce} knockback à {other.name}");
        }

        else if (other.TryGetComponent(out WallReceiver receiver1))
        {
            receiver1.BounceWall(transform.parent.GetComponent<Rigidbody>(), gameObject.name);
        }

        /*
        if (owner.currentAttackType == AttackType.Skill && owner is Player1 p1 && p1.IsDashing())
        {
            owner.StartCoroutine(FreezeAndTeleportBehind(other.transform, p1.GetDashDirection()));
        }*/
    }

    private IEnumerator FreezeAndTeleportBehind(Transform target, Vector3 dashDirection)
    {
        if (!target.TryGetComponent(out Rigidbody rb))
            yield break;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        float zOffset = (dashDirection.z > 0) ? 1f : -1f;
        Vector3 newPosition = target.position + new Vector3(0, 0, zOffset);
        owner.transform.position = newPosition;

        Debug.Log($"{owner.name} se déplace derrière {target.name}");
    }
}
