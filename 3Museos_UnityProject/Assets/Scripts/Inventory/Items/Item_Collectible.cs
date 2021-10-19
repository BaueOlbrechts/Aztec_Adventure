using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectible Item", fileName = "New collectible")]
public class Item_Collectible : Item
{
    public int CollectibleID = 0;

    public Sprite Uncollected;

    public bool IsCollected = false;

}
