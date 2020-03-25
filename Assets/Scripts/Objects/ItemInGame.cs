using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInGame : MonoBehaviour
{
    public Item ItemReference;

    public ItemUI Prefab;

    private ItemUI _itemBinded;

    // Start is called before the first frame update
    void Start()
    {
        _itemBinded = Instantiate(Prefab, Prefab.transform.parent);
        _itemBinded.ItemReference = Instantiate(ItemReference);
        _itemBinded.ObjectInGame = this;
        _itemBinded.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_itemBinded.IsDragging)
            _itemBinded.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnBecameVisible()
    {
        _itemBinded.gameObject.SetActive(true);
    }

    private void OnBecameInvisible()
    {
        _itemBinded.gameObject.SetActive(false);
    }
}
