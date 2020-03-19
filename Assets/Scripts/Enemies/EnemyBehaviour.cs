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
        Vector3.Distance(transform.position, _target.transform.position) >= 1;

    private Seeker _seeker;
    private IAstarAI _ia;

    private PlayerBehaviour _target;

    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _ia = GetComponent<IAstarAI>();

        _target = GameManager.Instance.Player;
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
        if (PlayerIsInRange)
        {
            StartCoroutine(ExecuteMovement());
        }
        else
        {
            // attack
        }
    }

    private IEnumerator ExecuteMovement()
    {
        _ia.canMove = true;
        yield return new WaitForSeconds(0.5f);
        _ia.canMove = false;
    }
}
