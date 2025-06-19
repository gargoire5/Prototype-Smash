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

    public MatchManager _matchManager = null;

    private DamageReceiver _player1Dmg;
    private DamageReceiver _player2Dmg;

    private bool _doOnce = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_matchManager != null)
        { 
            _player1Dmg = _matchManager.Player1Character.gameObject.GetComponent<DamageReceiver>();
            _player2Dmg = _matchManager.Player2Character.gameObject.GetComponent<DamageReceiver>();

            Player1Percent.text = _player1Dmg.damagePercent.ToString() + "%";
            Player2Percent.text = _player2Dmg.damagePercent.ToString() + "%";

            Player1Lives.text = _matchManager.Player1Lives.ToString();
            Player2Lives.text = _matchManager.Player2Lives.ToString();

            if (_doOnce == false)
            {
                Player1Sprite.texture = _matchManager.Player1Character.CharacterRender;
                Player2Sprite.texture = _matchManager.Player2Character.CharacterRender;

                _doOnce = true;
            }

        }
    }

    public void SetManager(MatchManager matchManagerToSet)
    {
        _matchManager = matchManagerToSet;
    }

}
