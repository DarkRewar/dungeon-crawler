using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Stats Datas;

    public bool PlayerIsInRange => 
        _target != null &&
        Vector3.Distance(transform.position, _target.transform.position) <= 1.1f;

    private Seeker _seeker;
    private IAstarAI _ia;

    private PlayerBehaviour _target;

    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _ia = GetComponent<IAstarAI>();

        _target = GameManager.Instance.Player;
        _ia.canMove = false;
    }

    public void OnEnable()
    {
        GameManager.OnPlayerMove += DoMovement;
    }

    public void OnDisable()
    {
        GameManager.OnPlayerMove -= DoMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if(_target != null)
        {
            var pos = _target.PlayerCamera.transform.position;
            pos.y = transform.position.y;
            transform.LookAt(pos);
        }

        if(Datas.Lifepoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void DoMovement(Vector3 playerPos)
    {
        if (!PlayerIsInRange)
        {
            _seeker.StartPath(transform.position, playerPos, OnPathComplete);
        }
        else
        {
            // attack
        }
    }

    private void OnPathComplete(Path p)
    {
        StartCoroutine(ExecuteMovement(p));
    }

    private IEnumerator ExecuteMovement(Path p)
    {
        float temp = 0, time = 0.5f;
        Vector3 start = p.vectorPath[0], end = p.vectorPath[1];

        while(temp < time)
        {
            temp += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, Mathf.Clamp01(temp / time));
            yield return null;
        }
    }
}
