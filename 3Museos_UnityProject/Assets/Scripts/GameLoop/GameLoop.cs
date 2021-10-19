using Museos.StateSystem;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Vuforia;
using Museos.Saving;
using UnityEngine.UI;

namespace Museos
{
    public class GameLoop : SingletonMonoBehaviour<GameLoop>
    {
        [Header("World references")]
        public UI_Element UI_NoTracker;
        public UI_Element UI_Inventory;
        public UI_Element UI_HotBar;
        public UI_Element UI_DialogueBox;
        public UI_Element UI_LoadingScreen;
        public UI_Element UI_ARWarning;

        public UnityEngine.UI.Image PictogramImage;
        public Text DialogueTextField;
        public Camera CharacterViewCamera;

        public GameObject TrackingHelper;
        public static TrackingState CurrentTracker = TrackingState.NotFound;

        [Header("Variables")]
        public string TargetTag;

        public List<Item> ItemList = new List<Item>();

        private List<ImageTargetBehaviour> _imageTargets = new List<ImageTargetBehaviour>();
        private StateMachine<BaseState> _stateMachine;
        public StateMachine<BaseState> StateMachine => _stateMachine;

        private void Awake()
        {
            CloseAllUI();
            UI_LoadingScreen.Open();

            //Find all trackers in the scene
            var trackers = FindObjectsOfType<ImageTargetBehaviour>();
            if (trackers != null && trackers.Length > 0)
            {
                _imageTargets = trackers.ToList();
            }
            else
            {
                Debug.LogError("There are no trackers found in the scene! Please check that you have added at least one tracker....");
                Application.Quit();
            }

            //Find all characters
            var cha = GameObject.FindGameObjectsWithTag("Character");

            //Find all static objects
            var stat = GameObject.FindGameObjectsWithTag("Static");


            //Settings the inventory slots
            var inventorySlots = UI_Inventory.gameObject.GetComponentsInChildren<UI_Item_View>().ToList();
            var hotbarSlots = UI_HotBar.gameObject.GetComponentsInChildren<UI_Item_View>().ToList();

            hotbarSlots.AddRange(inventorySlots);

            var saveObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveble>();

            //Load Save data
            GameSave.LoadGame(hotbarSlots, saveObjects.ToArray());


            //Set up state machine
            _stateMachine = new StateMachine<BaseState>();

            PlayState playState = new PlayState(UI_HotBar,UI_DialogueBox, PictogramImage, DialogueTextField, CharacterViewCamera);
            InventoryState inventoryState = new InventoryState(UI_Inventory, UI_HotBar);
            NoTrackerState noTrackerState = new NoTrackerState(TrackingHelper, UI_NoTracker);


            _stateMachine.RegisterState(GameStates.Play, playState);
            _stateMachine.RegisterState(GameStates.Inventory, inventoryState);
            _stateMachine.RegisterState(GameStates.NoTracker, noTrackerState);

            CostumTrackableEventHandler.TrackingChanged += StateChange;

            UI_LoadingScreen.Close();
            UI_ARWarning.Open();
            _stateMachine.MoveTo(GameStates.NoTracker);
            //Test
        }

        public void PressButton1()
        {
            _stateMachine.CurrentState?.Button1();
        }

        public void PressButton2()
        {
            _stateMachine.CurrentState?.Button2();
        }

        private void CloseAllUI()
        {
            UI_NoTracker.Close();
            UI_Inventory.Close();
            UI_HotBar.Close();
            UI_DialogueBox.Close();
            UI_LoadingScreen.Close();
            UI_ARWarning.Close();
        }

        private void StateChange(object sender, TrackingChangedEventArgs e)
        {
            Debug.Log($"New state is: {e.IsTracking}");

            if (e.IsTracking == true)
            {
                _stateMachine.MoveTo(GameStates.Play);
            }
            else if (e.IsTracking == false)
            {
                _stateMachine.MoveTo(GameStates.NoTracker);
            }
        }



#if UNITY_ANDROID || UNITY_IOS
        //Save data when moved to the background
        void OnApplicationFocus(bool pauseStatus)
        {
            if (pauseStatus && Application.platform == RuntimePlatform.Android)
            {
                SavePlayer();
            }
        }
#endif


        private void OnApplicationQuit()
        {
            SavePlayer();
        }

        private void SavePlayer()
        {
            var saveObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveble>();
            GameSave.SaveGame(saveObjects.ToList());
        }
    }

    public enum TrackingState
    {
        NotFound,
        Tracking
    }
}
