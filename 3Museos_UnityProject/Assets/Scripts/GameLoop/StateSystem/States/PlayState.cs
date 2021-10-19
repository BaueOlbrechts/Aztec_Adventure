using Interaction;
using UnityEngine;
using UnityEngine.UI;

namespace Museos.StateSystem 
{ 
    public class PlayState : BaseState
    {
        private UI_Element _hotbar;
        private UI_Element _dialogueBox;
        private Image _pictogram;
        private Text _textField;
        private AudioSource _currentAudioSource;
        private Interactible_Scene_Object_Character _character;
        private Camera _camera;
        public Item SelectedItem { get; internal set; }

        public PlayState(UI_Element hotbar, UI_Element dialogueBox, Image pictogram, Text textField, Camera camera)
        {
            _hotbar = hotbar;
            _dialogueBox = dialogueBox;
            _pictogram = pictogram;
            _textField = textField;
            _camera = camera;
        }

        protected sealed override void CleanUpState()
        {
            Debug.Log($"Exited state: {GameStates.Play}");
            _hotbar.Close();
            _dialogueBox.Close();
            _character?.StopTalking();
        }

        protected sealed override void SetupScene()
        {
            Debug.Log($"Entered state: {GameStates.Play}");
            _hotbar.Open();
        }

        public override void SelectItem(Item item)
        {
            SelectedItem = item;
        }

        public override void Button1()
        {
            StateMachine.MoveTo(GameStates.Inventory);
        }

        public void StartDialogue(Transform character)
        {
            _hotbar.Close();
            _dialogueBox.Open();

            _character = character.GetComponent<Interactible_Scene_Object_Character>();

            _camera.transform.position = character.position + character.forward * character.localScale.x * 2.5f + Vector3.up * character.localScale.x * 1.2f;
            _camera.transform.rotation = character.rotation;
            _camera.transform.Rotate(Vector3.up, 180);
            _camera.transform.rotation = Quaternion.Euler(5, _camera.transform.rotation.eulerAngles.y, 0);
        }

        public void UdpateDialogue(Sprite pictogram, string dialogueText)
        {
            _pictogram.sprite = pictogram;
            _textField.text = dialogueText;
        }

        public void StopDialogue()
        {
            _hotbar.Open();
            _dialogueBox.Close();
            _character = null;
        }
    }
}
