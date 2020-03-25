using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IDropHandler
{
    public int InventoryIndex = -1;
    public int EquipedIndex = -1;

    public Action<int, Item> OnObjectDropped;
    public Action<int, Item> OnObjectEquipped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemUI itemUI = eventData.pointerDrag.GetComponent<ItemUI>();
            if(itemUI != null)
            {
                itemUI.ChangeParent(transform);

                if(InventoryIndex > -1)
                    OnObjectDropped?.Invoke(InventoryIndex, itemUI.ItemReference);
                else OnObjectEquipped?.Invoke(EquipedIndex, itemUI.ItemReference);
            }
        }
    }
}
