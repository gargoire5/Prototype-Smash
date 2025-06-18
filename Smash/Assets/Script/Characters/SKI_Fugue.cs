using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKI_Fugue : MonoBehaviour
{
    private float _speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(90, -90, 90));
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, transform.position.z + _speed * Time.deltaTime), transform.rotation);
    }

    public void SetDirection(int direction)
    {
        _speed *= direction;
    }
}
