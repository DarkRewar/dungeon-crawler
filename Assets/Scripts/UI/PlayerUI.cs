using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text PlayerLife;

    public Button AttackOne;
    public Button AttackTwo;

    // Start is called before the first frame update
    void Start()
    {
        AttackOne.onClick.AddListener(() => { GameManager.Instance.TriggerAttack(1); });
        AttackTwo.onClick.AddListener(() => { GameManager.Instance.TriggerAttack(2); });
    }

    private void LateUpdate()
    {
        string hearts = "";
        for(int i = 0; i < GameManager.Instance.Player.Datas.Lifepoints; ++i) 
            hearts += "<sprite=16 color=FF3333/>";
        PlayerLife.text = hearts;
    }
}
