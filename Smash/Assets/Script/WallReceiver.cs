using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallReceiver : MonoBehaviour
{
    [SerializeField] float BounceFactor = 1f;

    public void BounceWall(Rigidbody rb, string hitboxName)
    {
        Vector3 bounceForce = Vector3.zero;
        Vector3 playerVel = rb.velocity;

        if (hitboxName == "BoxCollisionR" || hitboxName == "BoxCollisionL")
        {
            bounceForce.z = BounceFactor;

            if (playerVel.y > 1 || playerVel.y < 1)
            {
                bounceForce.y = playerVel.y;
            }

            if (hitboxName == "BoxCollisionR")
                bounceForce.z *= -1;
        }

        else
        {
            bounceForce.y = BounceFactor;

            if (playerVel.z > 1 || playerVel.z < 1)
            {
                bounceForce.z = playerVel.z;
            }

            if (hitboxName == "BoxCollisionUp")
                bounceForce.y *= -1;
        }

        rb.AddForce(bounceForce, ForceMode.Impulse);
    }
}
