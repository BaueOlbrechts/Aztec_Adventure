using Museos;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scr_Obj_Item_Name", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    [JsonRequired]
    public string ItemName = "item name";

    [JsonIgnore]
    public Sprite _icon;


    [JsonIgnore]
    public Sprite Icon {
        get
        {
            if (_icon == null)
            { 
                foreach (Item item in GameLoop.Instance.ItemList)
                {
                    if (item.ItemName == ItemName)
                    {
                        _icon = item._icon;
                    }
                }
            }

            return _icon;
        }
    }

    
}
