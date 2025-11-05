using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "ItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    [Header("Plant")]
    public int plantCost = 50;
    
}


