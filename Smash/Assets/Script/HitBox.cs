using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 10f;
    public CharacterAttack owner;

    private void Start()
    {
        if (owner == null)
        {
            if (transform.parent != null)
                transform.parent.TryGetComponent<CharacterAttack>(out owner);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver receiver = other.GetComponent<DamageReceiver>();

        if (owner != null)
        {
            if (other.gameObject == owner.gameObject)
                return;

            if (receiver != null)
            {
                Vector3 direction = (other.transform.position - owner.transform.position).normalized;
                receiver.TakeDamage(damage, direction);
                Debug.Log("Dégâts infligés à " + other.name);
            }
        }
        else
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            receiver.TakeDamage(damage, direction);
        }
    }
}