using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public PlayerController controller;

    private int _collisions = 0;

    private void FixedUpdate()
    {
        if (_collisions > 0)
        {
            controller.IsGrounded = true;
            controller.HasFastFallen = false;
        }
        else
            controller.IsGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            controller.numJump = controller.maxJump;
            _collisions++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            _collisions--;
        }
    }
}
