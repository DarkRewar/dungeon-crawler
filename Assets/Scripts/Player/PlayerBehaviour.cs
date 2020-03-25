using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public Stats Datas;

    [Header("Movement")]
    public float MoveTime = 1f;
    public float RotateTime = 1f;
    public AnimationCurve MoveCurve;

    [Header("Head")]
    public Camera PlayerCamera;
    public AnimationCurve CameraCurve;

    [Header("Inventaire")]
    private Item[] _inventory = new Item[12];
    private Item[] _equipedItems = new Item[2];

    public Action<int, int> OnItemMoved;

    public bool InventoryIsFull
    {
        get
        {
            foreach(var v in _inventory)
            {
                if (v == null) return false;
            }
            return true;
        }
    }

    private Vector3 _baseCamPos;

    private bool _isMoving = false;
    private bool _isRotating = false;
    private Vector3 _movement;

    private Vector3 _previousPosition;
    private Vector3 _nextPosition;

    private Quaternion _previousRotation;
    private Quaternion _nextRotation;
    private float _actualMoveTime = 0;
    private float _actualRotateTime = 0;

    private PlayerActions _inputActions;

    private void Awake()
    {
        GameManager.Instance.Player = this;

        _inputActions = new PlayerActions();
        _inputActions.Enable();
        _inputActions.Main.Movement.performed += OnPlayerMove;
        _inputActions.Main.Rotation.performed += OnPlayerRotate;
        _inputActions.Main.Inventory.performed += OnPlayerInventory;
    }

    private void OnEnable()
    {
        _inputActions.Main.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Main.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _baseCamPos = PlayerCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //_movement = new Vector3(
        //    Input.GetAxis("Horizontal"),
        //    Input.GetAxis("Vertical"),
        //    Input.GetAxis("Pivot")
        //);

        void CheckInput()
        {
            if (_movement != Vector3.zero && !_isRotating && !_isMoving)
            {
                _previousPosition = transform.position;
                _previousRotation = transform.rotation;

                if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
                {
                    _nextPosition = transform.position + transform.right * Mathf.Sign(_movement.x);

                    if (!Physics.Raycast(
                        PlayerCamera.transform.position,
                        transform.right * Mathf.Sign(_movement.x),
                        1.5f
                    ))
                    {
                        _isMoving = true;
                        _actualMoveTime = 0;

                        GameManager.OnPlayerMove?.Invoke(transform.position);
                    }
                }
                else if (_movement.y != 0)
                {
                    _nextPosition = transform.position + transform.forward * Mathf.Sign(_movement.y);

                    if (!Physics.Raycast(
                        PlayerCamera.transform.position,
                        transform.forward * Mathf.Sign(_movement.y),
                        1.1f
                    ))
                    {
                        _isMoving = true;
                        _actualMoveTime = 0;

                        GameManager.OnPlayerMove?.Invoke(transform.position);
                    }
                }
                else
                {
                    _actualRotateTime = 0;
                    _isRotating = true;
                    _nextRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * 90 * Mathf.Sign(_movement.z));

                    //GameManager.OnPlayerMove?.Invoke(transform.position);
                }
            }
        }

        CheckInput();

        if (_isMoving)
        {
            _actualMoveTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_previousPosition, _nextPosition, MoveCurve.Evaluate(_actualMoveTime / MoveTime));
            //transform.position = Vector3.Lerp(_previousPosition, _nextPosition, Mathf.Clamp01(_actualMoveTime / MoveTime));

            Vector3 camera = _baseCamPos;
            camera.y = _baseCamPos.y * CameraCurve.Evaluate(_actualMoveTime / MoveTime);
            PlayerCamera.transform.localPosition = camera;

            if (_actualMoveTime >= MoveTime) _isMoving = false;
        }
        else if (_isRotating)
        {
            _actualRotateTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_previousRotation, _nextRotation, MoveCurve.Evaluate(_actualRotateTime / RotateTime));
            if (_actualRotateTime >= RotateTime) _isRotating = false;
        }
    }

    #region Player Inputs

    private void OnPlayerMove(InputAction.CallbackContext obj)
    {
        Vector2 value = obj.ReadValue<Vector2>();
        _movement.x = value.x;
        _movement.y = value.y;
    }

    private void OnPlayerRotate(InputAction.CallbackContext obj)
    {
        float pivot = obj.ReadValue<float>();
        _movement.z = pivot;
    }

    private void OnPlayerInventory(InputAction.CallbackContext obj)
    {
        float value = obj.ReadValue<float>();
        GameManager.Instance.DisplayInventory();
    }

    #endregion

    #region Inventory

    internal void AddItem(int index, Item item)
    {
        int previous = -1;
        for (int i = 0; i < _inventory.Length; ++i)
        {
            if (_inventory[i] == item)
            {
                previous = i;
                break;
            }
        }

        int indexFromEquipped = -1;
        if (_equipedItems[0] == item) indexFromEquipped = 0;
        else if (_equipedItems[1] == item) indexFromEquipped = 1;

        if (indexFromEquipped > -1) _equipedItems[indexFromEquipped] = null;

        if(_inventory[index] != null && previous >= 0)
        {
            var temp = _inventory[index];
            _inventory[index] = item;
            _inventory[previous] = temp;
            OnItemMoved?.Invoke(index, previous);
        }
        else if(_inventory[index] != null && !InventoryIsFull)
        {
            for(int i = 0; i < _inventory.Length; ++i)
            {
                if(_inventory[i] == null)
                {
                    _inventory[i] = _inventory[index];
                    _inventory[index] = item;
                    OnItemMoved?.Invoke(index, i);
                    break;
                }
            }
        }
        else if(previous >= 0)
        {
            _inventory[previous] = null;
            _inventory[index] = item;
        }
        else
        {
            _inventory[index] = item;
        }
    }

    internal void RemoveItem(int index)
    {
        _inventory[index] = null;
    }

    internal void RemoveItem(Item item)
    {
        for (int i = 0; i < _inventory.Length; ++i)
        {
            if (_inventory[i] == item)
            {
                _inventory[i] = null;
                break;
            }
        }
    }

    internal void EquipItem(int index, Item item)
    {
        int previous = GetInventoryIndex(item);

        if(previous > -1)
        {
            RemoveItem(item);
        }

        _equipedItems[index] = item;
    }

    private int GetInventoryIndex(Item item)
    {
        for (int i = 0; i < _inventory.Length; ++i)
        {
            if (_inventory[i] == item) return i;
        }
        return -1;
    }

    internal Item GetEquippedItem(int index)
    {
        return _equipedItems[index];
    }

    #endregion
}
