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

    [SerializeField]
    private RawImage _p1SkillImage;

    [SerializeField]
    private RawImage _p2SkillImage;

    [SerializeField]
    private RawImage _p1UltImage;

    [SerializeField]
    private RawImage _p2UltImage;

    [SerializeField]
    private RawImage _p1SkillIcon;

    [SerializeField]
    private RawImage _p2SkillIcon;

    [SerializeField]
    private RawImage _p1UltIcon;

    [SerializeField]
    private RawImage _p2UltIcon;

    Color redish = new Color(1, 0.36f, 0.32f, 1);
    Color blueish = new Color(0.32f, 0.62f, 1, 1);

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_matchManager != null)
        {
            if (_doOnce == false)
            {
                Player1Sprite.texture = _matchManager.Player1Character.CharacterRender;
                Player2Sprite.texture = _matchManager.Player2Character.CharacterRender;

                _p1SkillIcon.texture = _matchManager.Player1Character.CharacterSkillIcon;
                _p2SkillIcon.texture = _matchManager.Player2Character.CharacterSkillIcon;

                _p1UltIcon.texture = _matchManager.Player1Character.CharacterUltIcon;
                _p2UltIcon.texture = _matchManager.Player2Character.CharacterUltIcon;

                _doOnce = true;
            }

            _player1Dmg = _matchManager.Player1Character.gameObject.GetComponent<DamageReceiver>();
            _player2Dmg = _matchManager.Player2Character.gameObject.GetComponent<DamageReceiver>();

            Player1Percent.text = _player1Dmg.damagePercent.ToString() + "%";
            Player2Percent.text = _player2Dmg.damagePercent.ToString() + "%";

            Player1Lives.text = _matchManager.Player1Lives.ToString();
            Player2Lives.text = _matchManager.Player2Lives.ToString();

            if (_matchManager.P1CanSkill)
                _p1SkillImage.color = redish;
            else
                _p1SkillImage.color = Color.white;

            if (_matchManager.P2CanSkill)
                
                _p2SkillImage.color = blueish;
            else
                _p2SkillImage.color = Color.white;

            if (_matchManager.P1CanUlt)
                _p1UltImage.color = redish;
            else
                _p1UltImage.color = Color.white;

            if (_matchManager.P1CanSkill)
                _p2UltImage.color = blueish;
            else
                _p2UltImage.color = Color.white;
        }
    }

    public void SetManager(MatchManager matchManagerToSet)
    {
        _matchManager = matchManagerToSet;
    }

}
