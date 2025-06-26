using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamageReceiver : MonoBehaviour
{
    public float damagePercent = 0f;
    public float knockbackMultiplier = 1.5f;

    private float _paradeTime = 2f;

    private Rigidbody rb;

    public bool isCounter = false;
    public bool isBlocking = false;

    [SerializeField] GameObject Shield;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isBlocking && _paradeTime > 0)
        {
            _paradeTime -= Time.deltaTime;

            float ratio = _paradeTime / 2;
            Shield.transform.localScale = new Vector3(ratio + 1, ratio + 1, ratio + 1);
        }
        else if (isBlocking && _paradeTime <= 0)
        {
            StopParade();
        }
        else if (_paradeTime < 2 && isBlocking == false)
        {
            _paradeTime += Time.deltaTime / 2;
        }

        if (_paradeTime > 2)
            _paradeTime = 2;
        if (_paradeTime < 0)   
            _paradeTime = 0;
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        if (isBlocking == false)
        {
            damagePercent += amount;
            float force = amount * (1 + damagePercent / 100) * knockbackMultiplier;
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            Debug.Log($"{gameObject.name} a reçu {amount} dégâts. Total: {damagePercent}%");
        }
        else
        {
            _paradeTime -= amount / 10;
        }
    }

    public void Parade()
    {
        if (_paradeTime > 0)
        {
            isBlocking = true;
            Shield.SetActive(true);
        }
    }

    public void StopParade()
    {
        isBlocking = false;
        Shield.SetActive(false);
    }
}