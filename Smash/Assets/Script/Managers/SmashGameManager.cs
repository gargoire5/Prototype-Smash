using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmashGameManager : MonoBehaviour
{

    private int _currentPlayer = 0;

    [SerializeField]
    private GameObject _player1Character = null;
    [SerializeField]
    private GameObject _player2Character = null;

    private MatchManager _matchManager = null;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacter(GameObject characterPrefabToSet)
    {
        if (_currentPlayer < 2)
        {
            if (_currentPlayer == 0)
            {
                _player1Character = characterPrefabToSet;
                Debug.Log($"Player 1 = {_player1Character.name}");
                _currentPlayer = 1;
            }
            else
            {
                _player2Character = characterPrefabToSet;
                Debug.Log($"Player 2 = {_player2Character.name}");
                _currentPlayer = 0;
            }
        }
    }

    public void StartMatch()
    {
        SceneManager.LoadScene("FightScene", LoadSceneMode.Single);
    }

    public void LosePlayer1()
    {
        Debug.Log("Loser Player1");
        ResetManager();
    }

    public void LosePlayer2()
    {
        Debug.Log("Loser Player2");
        ResetManager();
    }

    private void ResetManager()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Destroy(gameObject);
    }

    public void SetManager(MatchManager managerToSet)
    {
        _matchManager = managerToSet;

        _matchManager.StartMatch(_player1Character, _player2Character);
    }
}
