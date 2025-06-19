using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text Player1Percent;

    [SerializeField]
    TMPro.TMP_Text Player1Lives;

    [SerializeField]
    RawImage Player1Sprite;

    [SerializeField]
    TMPro.TMP_Text Player2Percent;

    [SerializeField]
    TMPro.TMP_Text Player2Lives;

    [SerializeField]
    RawImage Player2Sprite;

    private MatchManager _matchManager = null;

    private DamageReceiver _player1Dmg;
    private DamageReceiver _player2Dmg;

    // Update is called once per frame
    void Update()
    {
        if (_matchManager != null)
        {
            Player1Percent.text = _player1Dmg.damagePercent.ToString();
            Player2Percent.text = _player2Dmg.damagePercent.ToString();

            Player1Lives.text = _matchManager.Player1Lives.ToString();
            Player2Lives.text = _matchManager.Player2Lives.ToString();
        }
    }

    public void SetManager(MatchManager matchManagerToSet)
    {
        _matchManager = matchManagerToSet;
        _player1Dmg = _matchManager.Player1Character.gameObject.GetComponent<DamageReceiver>();
        _player2Dmg = _matchManager.Player2Character.gameObject.GetComponent<DamageReceiver>();
    }

}
