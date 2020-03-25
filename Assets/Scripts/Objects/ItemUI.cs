using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Item ItemReference;
    public ItemInGame ObjectInGame;

    public bool IsDragging = false;

    private Vector3 _origin;

    private Image _image;

    private Transform _previousParent;

    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.localPosition;

        _image = GetComponent<Image>();
        _image.sprite = ItemReference.Sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void ChangeParent(Transform t)
    {
        if(ObjectInGame != null)
            DestroyImmediate(ObjectInGame.gameObject);

        gameObject.SetActive(true);
        transform.SetParent(t);
        transform.localPosition = Vector2.zero;
        _origin = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        _image.raycastTarget = false;

        _previousParent = transform.parent;
        transform.SetParent(transform.root);
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == transform.root)
        {
            transform.SetParent(_previousParent);
        }

        IsDragging = false;
        _image.raycastTarget = true;
        transform.localPosition = _origin;
    }
}
