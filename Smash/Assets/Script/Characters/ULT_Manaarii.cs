using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULT_Manaarii : MonoBehaviour
{
    public GameObject manaarii = null;

    public bool isGoingBack = false;
    public Rigidbody rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoingBack)
        {
            Vector3 dir = new Vector3(manaarii.transform.position.x - transform.position.x,
                manaarii.transform.position.y - transform.position.y,
                manaarii.transform.position.z - transform.position.z).normalized;

            rb.AddForce(dir, ForceMode.Impulse);
        }

        transform.Rotate(0, 1, 0);
    }
}
