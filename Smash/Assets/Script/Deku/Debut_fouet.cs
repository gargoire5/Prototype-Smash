using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debut_fouet : MonoBehaviour
{
    public Rigidbody rb;
    public bool iscollid;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!iscollid)
        {
            Destroy(rb);
            transform.parent = collision.gameObject.transform;
            iscollid = true;
        }
    }

}
