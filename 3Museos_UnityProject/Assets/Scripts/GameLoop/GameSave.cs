using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Museos.Saving;
using Museos;
using System.Linq;

namespace Museos.Saving
{
    public class GameSave
    {
        [JsonRequired]
        private Dictionary<string, int> ObjectSaves = new Dictionary<string, int>();

        //Save directories
        private static string _savePath = Application.persistentDataPath + "/Save.json";
        private static string _inventorySavePath = Application.persistentDataPath + "/Inventory.json";

        public static GameSave CurrentSave { get; private set; }

        [JsonIgnore]
        private Inventory _saveInventory;
        [JsonIgnore]
        public Inventory SaveInventory
        {
            get
            {
                return _saveInventory;
            }
            set
            {
                _saveInventory = value;
            }
        }

        public GameSave()
        {
            _saveInventory = new Inventory();
        }

        private void OnInventoryChanged(object sender, InventoryEventArgs e)
        {
            SaveSave(this);
        }

        #region Scene object save states

        public int LoadState(string key)
        {
            if (ObjectSaves.TryGetValue(key, out var val))
            {
                return val;
            }
            return -1;
        }

        public bool SaveState(string key, int val)
        {
            if (ObjectSaves.ContainsKey(key))
            {
                ObjectSaves[key] = val;
            }
            else
            {
                ObjectSaves.Add(key, val);
            }

            return true;
        }

        #endregion


        public static void LoadGame(List<UI_Item_View> itemSlots, ISaveble[] saveObjects)
        {
            foreach (var slot in itemSlots)
                slot.Init();

            LoadGameSave();
            LoadGameInventory(itemSlots);
            LoadGameObjects(saveObjects);
        }

        public static void SaveGame(List<ISaveble> savebles)
        {
            foreach (ISaveble thisSave in savebles)
            {
                thisSave.SaveObject();
            }

            SaveSave(CurrentSave);
        }




        private static void LoadGameSave()
        {
            GameSave save = new GameSave();



            if (System.IO.File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);

                Inventory inv = save.SaveInventory;

                save = JsonConvert.DeserializeObject<GameSave>(json);
            }

            CurrentSave = save;
        }

        private static void LoadGameInventory(List<UI_Item_View> views)
        {
            CurrentSave.SaveInventory = new Inventory();
            CurrentSave.SaveInventory.AddItemSlots(views.ToArray());

            if (System.IO.File.Exists(_inventorySavePath))
            {
                string json = System.IO.File.ReadAllText(_inventorySavePath);
                CurrentSave.SaveInventory.LoadInventory(JsonConvert.DeserializeObject<Inventory>(json));
            }
                CurrentSave.SaveInventory.InventoryChanged += CurrentSave.OnInventoryChanged;
        }

        private static List<ISaveble> _saveObject;

        private static void LoadGameObjects(ISaveble[] saveObjects)
        {
            _saveObject = saveObjects.ToList();

            foreach (ISaveble save in saveObjects)
            {
                save.LoadObject();
            }
        }

        public static bool SaveSave(GameSave toSave)
        {
            Debug.Log($"Saving file to: {_savePath}");

            string json = JsonConvert.SerializeObject(toSave, Formatting.None);
            Debug.Log(json);

            if (System.IO.File.Exists(_savePath))
            {
                File.Delete(_savePath);
            }
            else
            {
                FileStream file = File.Create(_savePath);
                file.Close();
                File.WriteAllText(_savePath, json);
            }


            System.IO.File.WriteAllText(_savePath, json);

            SaveMyInventory(toSave);

            return true;
        }

        private static void SaveMyInventory(GameSave toSave)
        {
            string jsonInventory = JsonConvert.SerializeObject(toSave._saveInventory);

            Debug.Log(jsonInventory);

            if (System.IO.File.Exists(_inventorySavePath))
            {
                File.Delete(_inventorySavePath);
            }
            else
            {
                FileStream file = File.Create(_inventorySavePath);
                file.Close();
                File.WriteAllText(_inventorySavePath, jsonInventory);
            }


            System.IO.File.WriteAllText(_inventorySavePath, jsonInventory);
        }
    }
}
