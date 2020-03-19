using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 _baseCamPos;

    private bool _isMoving = false;
    private bool _isRotating = false;
    private Vector3 _movement;

    private Vector3 _nextPosition;
    private Quaternion _nextRotation;
    private float _actualMoveTime = 0;
    private float _actualRotateTime = 0;

    private void Awake()
    {
        GameManager.Instance.Player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _baseCamPos = PlayerCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"),
            Input.GetAxis("Pivot")
        );
        
        if(_movement != Vector3.zero && !_isRotating && !_isMoving)
        {
            if(Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
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
            else if(_movement.y != 0)
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

        if (_isMoving)
        {
            _actualMoveTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, _nextPosition, MoveCurve.Evaluate(_actualMoveTime / MoveTime));

            //Vector3 camera = _baseCamPos;
            //camera.y = Mathf.Lerp(
            //    _baseCamPos.y,
            //    _baseCamPos.y,
            //    CameraCurve.Evaluate(_actualMoveTime / MoveTime)
            //);
            //PlayerCamera.transform.localPosition = camera;

            if (_actualMoveTime >= MoveTime) _isMoving = false;
        }
        else if (_isRotating)
        {
            _actualRotateTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, _nextRotation, MoveCurve.Evaluate(_actualRotateTime / RotateTime));
            if (_actualRotateTime >= RotateTime) _isRotating = false;
        }
    }
}
