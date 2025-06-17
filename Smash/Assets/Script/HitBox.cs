using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 10f;
    public CharacterAttack owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner.gameObject)
            return;

        DamageReceiver receiver = other.GetComponent<DamageReceiver>();
        if (receiver != null)
        {
            Vector3 direction = (other.transform.position - owner.transform.position).normalized;
            receiver.TakeDamage(damage, direction);
            Debug.Log("Dégâts infligés à " + other.name);
        }
    }
}