using UnityEngine;

namespace Museos.StateSystem
{
    public class SettingsState : BaseState
    {

        private UI_Element _settingsUI;
        private UI_Element _hotbarUI;

        public SettingsState(UI_Element settingsUI, UI_Element hotbarUI)
        {
            _settingsUI = settingsUI;
            _hotbarUI = hotbarUI;
        }


        protected sealed override void CleanUpState()
        {
            Debug.Log($"Exited state: {GameStates.Settings}");
            _settingsUI.Close();
            _hotbarUI.Close();
        }

        protected sealed override void SetupScene()
        {
            Debug.Log($"Entered state: {GameStates.Settings}");
            _settingsUI.Open();
            _hotbarUI.Open();
        }

        public override void Button1()
        {
            StateMachine.MoveTo(GameStates.Inventory);
        }
    }
}
