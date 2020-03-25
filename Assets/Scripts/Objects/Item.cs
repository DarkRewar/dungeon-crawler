using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "DungeonCrawler/New Item")]
public class Item : ScriptableObject
{
    public string ItemName;

    public Sprite Sprite;

    public Stats Datas;
}
