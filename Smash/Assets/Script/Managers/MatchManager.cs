using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviour
{
    public int Player1Lives = 3;
    public int Player2Lives = 3;

    private GameObject _player1;
    private GameObject _player2;

    public GameObject _player1Tag;
    public GameObject _player2Tag;

    public CharacterAttack Player1Character;
    public CharacterAttack Player2Character;

    [SerializeField]
    private Transform _player1Spawn; 
    [SerializeField]
    private Transform _player2Spawn;

    private SmashGameManager _manager;

    public InputActionAsset Player1Input;
    public InputActionAsset Player2Input;

    public Material Tag1Texture;
    public Material Tag2Texture;

    public bool P1CanSkill;
    public bool P2CanSkill;

    public bool P1CanUlt;
    public bool P2CanUlt;

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindFirstObjectByType<SmashGameManager>();
        _manager.SetManager(this);

        FindFirstObjectByType<UIManager>().SetManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        P1CanSkill = Player1Character.canUseSkill;
        P2CanSkill = Player2Character.canUseSkill;
        P1CanUlt = Player1Character.canUseUltimate;
        P2CanUlt = Player2Character.canUseUltimate;
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
        Player1Character.PlayerTag.GetComponent<MeshRenderer>().material = Tag1Texture;

        _player1.GetComponent<PlayerController>().SetupInputActions(Player1Input);

        _player2 = Instantiate(player2Prefab);
        _player2.transform.position = _player2Spawn.position;
        Player2Character = _player2.GetComponent<CharacterAttack>();
        Player2Character.PlayerTag.GetComponent<MeshRenderer>().material = Tag2Texture;

        _player2.GetComponent<PlayerController>().SetupInputActions(Player2Input);
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
