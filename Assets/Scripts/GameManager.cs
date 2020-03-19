using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager Instance;

    public void Awake()
    {
        Instance = this;
    }
    #endregion

    public static Action<Vector3> OnPlayerMove;

    public PlayerBehaviour Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAttack(int number)
    {
        Ray ray = new Ray(Player.PlayerCamera.transform.position, Player.PlayerCamera.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, number))
        {
            EnemyBehaviour enemy = hit.collider.gameObject.GetComponent<EnemyBehaviour>();
            if(enemy != null)
            {
                enemy.Datas.Lifepoints -= Player.Datas.Strength + (number == 1 ? 2 : 1) - enemy.Datas.Defense;
            }
            
            if(enemy.PlayerIsInRange)
            {
                Player.Datas.Lifepoints -= enemy.Datas.Strength - Player.Datas.Defense;
            }
            else
            {
                OnPlayerMove?.Invoke(Player.transform.position);
            }
        }
        else
        {

        }
    }
}
