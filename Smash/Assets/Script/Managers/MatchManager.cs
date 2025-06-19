using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public int Player1Lives = 3;
    public int Player2Lives = 3;

    private GameObject _player1;
    private GameObject _player2;

    public CharacterAttack Player1Character;
    public CharacterAttack Player2Character;

    [SerializeField]
    private Transform _player1Spawn; 
    [SerializeField]
    private Transform _player2Spawn;

    private SmashGameManager _manager;

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindFirstObjectByType<SmashGameManager>();
        _manager.SetManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Player1Death()
    {
        Player1Lives--;
        if (Player1Lives != 0)
        {
            _player1.transform.position = _player1Spawn.position;
            _player1.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _player1.GetComponent<DamageReceiver>().damagePercent = 0;
        }
        else
        {
            _manager.LosePlayer1();
        }
    }

    private void Player2Death()
    {
        Player2Lives--;
        if (Player2Lives != 0)
        {
            _player2.transform.position = _player2Spawn.position;
            _player2.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _player2.GetComponent<DamageReceiver>().damagePercent = 0;
        }
        else
        {
            _manager.LosePlayer2();
        }
    }

    public void StartMatch(GameObject player1Prefab, GameObject player2Prefab)
    {
        _player1 = Instantiate(player1Prefab);
        _player1.transform.position = _player1Spawn.position;
        Player1Character = _player1.GetComponent<CharacterAttack>();

        _player2 = Instantiate(player2Prefab);
        _player2.transform.position = _player2Spawn.position;
        Player2Character = _player2.GetComponent<CharacterAttack>();

        Debug.Log("Je commence");
    }

    public void SetManager(SmashGameManager managerToSet)
    {
        _manager = managerToSet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player1)
        {
            Player1Death();
        }
        else if (other.gameObject == _player2)
        {
            Player2Death();
        }
    }
}
