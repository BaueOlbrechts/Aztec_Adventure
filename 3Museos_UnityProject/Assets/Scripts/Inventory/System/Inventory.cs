using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Museos.Saving
{
    [JsonObject]
    public class Inventory
    {
        [JsonRequired]
        public List<Item> _myItems = new List<Item>();
        [JsonIgnore]
        private static UI_Item_View[] _itemSlots;

        public event EventHandler<InventoryEventArgs> InventoryChanged;

        public Inventory()
        {
            //Setup the inventory
            _myItems = new List<Item>();
        }

        public void LoadInventory(Inventory inventory)
        {
            for (int i = 0; i < inventory._myItems.Count; i++)
            {
                _myItems.Add(inventory._myItems[i]);
                _itemSlots[i].LoadItem(inventory._myItems[i]);
            }
        }

        //Add a puzzle piece to the active inventory
        public void AddItem(Item item)
        {
            if (!_myItems.Contains(item))
            {
                _myItems.Add(item);

                CallInventoryChanged();
            }
        }


        //Removing a puzzle piece from the active collection
        public void TakeItem(Item item)
        {
            if (_myItems.Contains(item))
            {
                _myItems.Remove(item);

                CallInventoryChanged();
            }
        }


        private void CallInventoryChanged(Item changed = null)
        {
            var e = InventoryChanged;
            e?.Invoke(this, new InventoryEventArgs(changed, this));
        }

        public void AddItemSlots(UI_Item_View[] slots)
        {
            _itemSlots = slots;
        }

        //public void GetItemSlots()
        //{
        //    _itemSlots = GameObject.FindObjectsOfType<UI_Item_View>();
        //}

        public void AddNewItem(Item item)
        {
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                if(_itemSlots[i].CurrentItem == null)
                {
                    _itemSlots[i].CurrentItem = item;
                    return;
                }
            }

            //give warning when slots are full
            Debug.LogError("All slots are filled");
        }

        //The old save file system

        ////Clears the save file of this inventory instance
        //private void ClearSavedInventory()
        //{
        //    if(_savePath != null)
        //    {
        //        if(File.Exists(_savePath))
        //            File.Delete(_savePath);
        //    }
        //}


        ////Clears the save file of a given path;
        //static public void ClearSavedInventory(string path)
        //{
        //    if(File.Exists(path + _inventoryFileName))
        //    File.Delete(path + _inventoryFileName);
        //}


        //static public void SaveInventory(Inventory toSave, string path)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Create(path + _inventoryFileName);
        //    bf.Serialize(file, toSave);
        //    file.Close();
        //}


        //static public Inventory LoadInventory(string path)
        //{
        //    if (File.Exists(path + _inventoryFileName))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        FileStream file = File.Open(path + _inventoryFileName, FileMode.Open);
        //        Inventory inventory = (Inventory)bf.Deserialize(file);
        //        file.Close();

        //        return inventory;
        //    }

        //    return null;
        //}
    }

    [Serializable]
    public class CollectionLibrary
    {
        public List<Item_Collectible> AllCollectibles = new List<Item_Collectible>();

        public void AddItem(Item_Collectible collectible)
        {
            if(AllCollectibles.Contains(collectible) == false && CheckOnID(collectible) == false )
            {
                AllCollectibles.Add(collectible);
            }
        }

        private bool CheckOnID(Item_Collectible collectible)
        {
            foreach(Item_Collectible item in AllCollectibles)
            {
                if(item.CollectibleID == collectible.CollectibleID)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetOnID(Item_Collectible collectible)
        {
            for (int i = 0; i < AllCollectibles.Count; i++)
            {
                if(AllCollectibles[i].CollectibleID == collectible.CollectibleID)
                {
                    return i;
                }
            }

            return int.MinValue;
        }
    
        public void CollectItem(Item_Collectible collectible)
        {
            if (AllCollectibles.Contains(collectible) == true || CheckOnID(collectible) == true)
            {
                int indx = GetOnID(collectible);
                if (indx != int.MinValue)
                {
                    AllCollectibles[indx].IsCollected = true;
                }
            }
        }
    }



    public class InventoryEventArgs : EventArgs
    {
        public Item ChangedItem;
        public Inventory TargetInventory { get; private set; }
        public InventoryEventArgs(Item item, Inventory targetInventory)
        {
            ChangedItem = item;
            TargetInventory = targetInventory;
        }
    }
}