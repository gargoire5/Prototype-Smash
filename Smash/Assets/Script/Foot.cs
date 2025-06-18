using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public PlayerController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            controller.numJump = controller.maxJump;
        }
    }
}
