using Museos;
using Museos.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction
{
    [RequireComponent(typeof(Animator))]
    public class Interactible_Scene_Object_Character : Interactible_Scene_Object_Base, IDropHandler, ISaveble
    {
        public Character_ScriptableObject CharScrObj = null;
        private Animator _animator;
        private bool _canPlayAnimation = true;
        private string _triggerName;

        //private GameObject _model = null;

        //private SpriteRenderer _bubble = null;
        //private SpriteRenderer _pictogram = null;

        private int _dialogueNumber = 0;
        private int dialogueNumber
        {
            get
            {
                return _dialogueNumber;
            }
            set
            {
                _dialogueNumber = value;
                SaveObject();
            }
        }




        internal override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();

#if UNITY_EDITOR
            if (gameObject.tag != "Character")
                Debug.LogError($"Tag is wrong or missing on {gameObject.name}");
#endif
            if (CharScrObj != null)
                CharScrObj = Instantiate(CharScrObj);
            else
                Debug.LogError("No scriptable object is assigned");

            //Set up scriptable object stuff
            if (!CharScrObj.AnimationClip.isLooping)
                Debug.LogError("Make sure the character animation is looping!");

            AnimatorOverrideController aoc = new AnimatorOverrideController(CharScrObj.RuntimeAnimatorController);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips)
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, CharScrObj.AnimationClip));
            aoc.ApplyOverrides(anims);

            _animator.runtimeAnimatorController = aoc;
            _triggerName = _animator.GetParameter(0).name;
            _audioSource.clip = CharScrObj.TalkingAudio;


            //_model = transform.GetChild(0).gameObject;

            //Text popup
            //_bubble = transform.GetChild(1).GetComponent<SpriteRenderer>();
            //_pictogram = _bubble.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            //
            //_bubble.gameObject.SetActive(false);
        }



        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_canPlayAnimation)
                StartCoroutine(Dialogue());
        }

        private IEnumerator Dialogue()
        {
            /*
            //Start of dialogue
            _canPlayAnimation = false;
            _audioSource.clip = CharScrObj.TalkingAudio;
            _audioSource.loop = true;
            _audioSource.Play();
            _bubble.gameObject.SetActive(true);
            _animator.SetTrigger(_triggerName);

            //During dialogue
            for (int i = 0; i < CharScrObj.DialogueOptions[_dialogueNumber].DialogueSprites.Length; i++)
            {
                Debug.Log($"Dialogue popup: {i + 1}");
                _pictogram.sprite = CharScrObj.DialogueOptions[_dialogueNumber].DialogueSprites[i];

                yield return new WaitForSeconds(CharScrObj.TimeBetweenDialogue);
            }

            //End of dialogue
            _canPlayAnimation = true;
            _audioSource.loop = false;
            _audioSource.Stop();
            _bubble.gameObject.SetActive(false);
            _animator.SetTrigger(_triggerName);
            */


            //Start of dialogue
            Museos.StateSystem.PlayState state = (Museos.StateSystem.PlayState)Museos.GameLoop.Instance.StateMachine.CurrentState;

            _canPlayAnimation = false;
            _audioSource.clip = CharScrObj.TalkingAudio;
            _audioSource.loop = true;
            _audioSource.Play();
            _animator.SetTrigger(_triggerName);
            state.StartDialogue(transform);

            for (int i = 0; i < CharScrObj.DialogueOptions[dialogueNumber].Dialogue.Length; i++)
            {
                state.UdpateDialogue(CharScrObj.DialogueOptions[dialogueNumber].Dialogue[i].DialogueSprite, CharScrObj.DialogueOptions[dialogueNumber].Dialogue[i].DialogueText);
                yield return new WaitForSeconds(CharScrObj.TimeBetweenDialogue);
            }

            //End of dialogue
            StopTalking();
            _animator.SetTrigger(_triggerName);
            state.StopDialogue();
        }

        public void StopTalking()
        {
            _canPlayAnimation = true;
            _audioSource.loop = false;
            _audioSource.Stop();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var tradeItem = eventData.selectedObject.GetComponent<UI_Item_View>().CurrentItem;

            if (tradeItem == null)
                return;

            //Check if trading is possible
            if (_canPlayAnimation && CharScrObj.DialogueOptions[dialogueNumber].ItemToRecieve != null)
            {
                //Check the dropped item
                if (CharScrObj.DialogueOptions[dialogueNumber].ItemToRecieve.ItemName == tradeItem.ItemName)
                {
                    Debug.Log("Can Trade");
                    _audioSource.clip = CharScrObj.QuestCompleteAudio;
                    _audioSource.Play();
                    StartCoroutine(TalkCooldown(_audioSource.clip.length));

                    //Trade the item with inventory
                    eventData.selectedObject.GetComponent<UI_Item_View>().CurrentItem = null;
                    GameSave.CurrentSave.SaveInventory.AddNewItem(CharScrObj.DialogueOptions[dialogueNumber].ItemToGive);

                    dialogueNumber = Mathf.Clamp(dialogueNumber + 1, 0, CharScrObj.DialogueOptions.Length - 1);
                }
                else
                {
                    Debug.Log("Wrong Item");
                    _audioSource.clip = CharScrObj.WrongItemAudio;
                    _audioSource.Play();
                    StartCoroutine(TalkCooldown(_audioSource.clip.length));
                }
            }
            else
                Debug.Log("In Animation, cannot trade yet");
        }

        private IEnumerator TalkCooldown(float time)
        {
            _canPlayAnimation = false;
            yield return new WaitForSeconds(CharScrObj.TimeBetweenDialogue);
            _canPlayAnimation = true;
        }





        //ISaveble properties
        public string Identifier { get { return gameObject.name; } }

        public int State { get { return dialogueNumber; } set { dialogueNumber = value; } }


        public void SaveObject()
        {
            GameSave.CurrentSave.SaveState(Identifier, State);
        }

        public void LoadObject()
        {
            State = GameSave.CurrentSave.LoadState(Identifier);

            if (State == -1)
                State = 0;
        }
    }
}
