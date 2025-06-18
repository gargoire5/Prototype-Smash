using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corde : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private GameObject _player;
    private GameObject _bout;


    void Update()
    {
        lineRenderer.SetPosition(0, _player.transform.position);
        lineRenderer.SetPosition(1, _bout.transform.position);
    }

    public void Setbout(GameObject player,GameObject bout)
    {
        _player = player;
        _bout = bout;
    }
}
