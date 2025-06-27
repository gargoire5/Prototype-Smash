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
            bounceForce.z = BounceFactor * 10;

            if (playerVel.y > 1 || playerVel.y < 1)
            {
                bounceForce.y = playerVel.y/2;
            }

            if (hitboxName == "BoxCollisionR")
                bounceForce.z *= -1;
        }

        else
        {
            bounceForce.y = BounceFactor * 10;

            if (playerVel.z > 1 || playerVel.z < 1)
            {
                bounceForce.z = playerVel.z / 2;
            }

            if (hitboxName == "BoxCollisionR")
                bounceForce.y *= -1;
        }

        rb.AddForce(bounceForce, ForceMode.Impulse);
    }
}
