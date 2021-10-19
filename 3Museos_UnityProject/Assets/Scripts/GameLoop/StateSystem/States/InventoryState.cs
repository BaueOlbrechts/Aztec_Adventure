using UnityEngine;

namespace Museos.StateSystem
{
    public class InventoryState : BaseState
    {
        private UI_Element _inventory;
        private UI_Element _hotbar;

        public InventoryState(UI_Element inventoryUI, UI_Element hotbarUI)
        {
            _inventory = inventoryUI;
            _hotbar = hotbarUI;
        }

        

        protected sealed override void CleanUpState()
        {
            Debug.Log($"Exited state: {GameStates.Inventory}");
            _inventory.Close();
            _hotbar.Close();
        }

        protected sealed override void SetupScene()
        {
            Debug.Log($"Entered state: {GameStates.Inventory}");
            _inventory.Open();
            _hotbar.Open();
        }

        public override void Button1()
        {
            StateMachine.MoveTo(GameStates.Play);
        }

        public override void Button2()
        {
            StateMachine.MoveTo(GameStates.Settings);
        }
    }
}
