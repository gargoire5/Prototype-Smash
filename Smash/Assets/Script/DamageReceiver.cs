using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamageReceiver : MonoBehaviour
{
    public float damagePercent = 0f;
    public float knockbackMultiplier = 1.5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        damagePercent += amount;
        float force = amount * (1 + damagePercent) * knockbackMultiplier;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log($"{gameObject.name} a reçu {amount} dégâts. Total: {damagePercent}%");
    }
}