using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text PlayerLife;

    [Header("Attacks")]
    public Button AttackOne;
    public Button AttackTwo;

    [Header("Inventory")]
    public GridLayoutGroup Grid;

    private SlotUI[] _slots;

    private SlotUI _slotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        AttackOne.onClick.AddListener(() => { GameManager.Instance.TriggerAttack(0); });
        AttackTwo.onClick.AddListener(() => { GameManager.Instance.TriggerAttack(1); });

        AttackOne.GetComponent<SlotUI>().OnObjectEquipped += OnObjectEquipped;
        AttackTwo.GetComponent<SlotUI>().OnObjectEquipped += OnObjectEquipped;

        _slotPrefab = Grid.GetComponentInChildren<SlotUI>();

        if(_slotPrefab != null)
        {
            _slots = new SlotUI[12];
            for (int i = 0; i < 12; ++i)
            {
                SlotUI s = Instantiate(_slotPrefab, Grid.transform);
                s.gameObject.name = $"Slot[{i}]";
                s.InventoryIndex = i;
                s.OnObjectDropped += ChangeInventory;

                _slots[i] = s;
            }
        }

        _slotPrefab.gameObject.SetActive(false);

        GameManager.Instance.Player.OnItemMoved += OnItemMoved;
        GameManager.OnInventoryDisplay += () =>
        {
            Grid.gameObject.SetActive(!Grid.gameObject.activeSelf);
        };
    }

    private void LateUpdate()
    {
        string hearts = "";
        for(int i = 0; i < GameManager.Instance.Player.Datas.Lifepoints; ++i) 
            hearts += "<sprite=16 color=FF3333/>";
        PlayerLife.text = hearts;
    }

    private void ChangeInventory(int index, Item item)
    {
        GameManager.Instance.Player.AddItem(index, item);
    }

    private void OnItemMoved(int previousIndex, int currentIndex)
    {
        Transform t = _slots[previousIndex].transform.GetChild(0);
        t.GetComponent<ItemUI>().ChangeParent(_slots[currentIndex].transform);
        //t.SetParent(_slots[currentIndex].transform);
        //t.localPosition = Vector2.zero;

    }

    private void OnObjectEquipped(int index, Item item)
    {
        GameManager.Instance.Player.EquipItem(index, item);
    }
}
