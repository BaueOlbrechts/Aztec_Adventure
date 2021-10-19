using UnityEngine;

namespace Museos.StateSystem
{
    public class NoTrackerState : BaseState
    {
        private UI_Element _noTrackerUI;
        private GameObject _trackingGuide;

        public NoTrackerState(GameObject trackingGuide, UI_Element noTrackerUi)
        {
            _noTrackerUI = noTrackerUi;
            _trackingGuide = trackingGuide;
        }

        protected override void CleanUpState()
        {
            _noTrackerUI.Close();
            _trackingGuide.SetActive(false);

            Debug.Log($"Exited state: {GameStates.NoTracker}");
        }

        protected override void SetupScene()
        {
            _noTrackerUI.Open();
            _trackingGuide.SetActive(true);

            Debug.Log($"Entered state: {GameStates.NoTracker}");
        }
    }
}
