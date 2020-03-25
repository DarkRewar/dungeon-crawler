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

    public static Action OnInventoryDisplay;

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
        Item item = Player.GetEquippedItem(number);

        if (item == null) return;

        Ray ray = new Ray(Player.PlayerCamera.transform.position, Player.PlayerCamera.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, item.Datas.Range))
        {
            EnemyBehaviour enemy = hit.collider.gameObject.GetComponent<EnemyBehaviour>();
            if(enemy != null)
            {
                enemy.Datas.Lifepoints -= Player.Datas.Strength + item.Datas.Strength - enemy.Datas.Defense;
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

    internal void DisplayInventory()
    {
        OnInventoryDisplay?.Invoke();
    }
}
