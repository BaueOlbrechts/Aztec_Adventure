using Museos;
using Museos.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction
{
    [RequireComponent(typeof(Animator))]
    public class Interactible_Scene_Object_Static : Interactible_Scene_Object_Base, ISaveble
    {
        public Static_ScriptableObject StatScrObj = null;
        private Animator _animator;
        private bool _canPlayAnimation = true;
        private string _triggerName;
        private int _saveState = 0;

        internal override void Start()
        {
            base.Start();

            if (StatScrObj != null)
                StatScrObj = Instantiate(StatScrObj);
            else
                Debug.Log("No scriptableObject assigned!");

            if (_saveState == 1)
            {
                if (StatScrObj.RemoveAfterRecieving)
                    Destroy(gameObject);
                else
                {
                    StatScrObj.ItemToRecieve = null;
                    _canPlayAnimation = false;
                }
            }

            _animator = GetComponent<Animator>();

            AnimatorOverrideController aoc = new AnimatorOverrideController(StatScrObj.RuntimeAnimatorController);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips)
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, StatScrObj.AnimationClip));
            aoc.ApplyOverrides(anims);

            _animator.runtimeAnimatorController = aoc;
            _triggerName = _animator.GetParameter(0).name;
            _audioSource.clip = StatScrObj.AudioClip;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_canPlayAnimation)
            {
                base.OnPointerClick(eventData);
                _animator.SetTrigger(_triggerName);
                _audioSource.Play();
                Museos.GameLoop.Instance.gameObject.GetComponent<Tap_Particles>().TappedOnObject(StatScrObj.Material, eventData.pointerCurrentRaycast.worldPosition); //Yes, I know it's sloppy

                if (StatScrObj.ItemToRecieve != null)
                {
                    Debug.Log("Can give item");

                    //This function needs to be added in with the inventory
                    //GameLoop.GameLoop.Instance.CurrentInventory.AddNewItem(StatScrObj.ItemToRecieve);

                    //Remove object from scene depending on settings
                    StartCoroutine(RecieveTimer(_animator.GetCurrentAnimatorStateInfo(0).length, StatScrObj.RemoveAfterRecieving));
                    _canPlayAnimation = false;
                    return;
                }


                StartCoroutine(TapCooldown(_animator.GetCurrentAnimatorStateInfo(0).length));
            }
        }

        private IEnumerator TapCooldown(float waitTime)
        {
            _canPlayAnimation = false;
            yield return new WaitForSeconds(waitTime);
            _canPlayAnimation = true;
        }

        private IEnumerator RecieveTimer(float waitTime, bool getsDeleted)
        {
            yield return new WaitForSeconds(waitTime);
            GameSave.CurrentSave.SaveInventory.AddNewItem(StatScrObj.ItemToRecieve);
            _saveState = 1;
            SaveObject();

            if (getsDeleted)
                Destroy(gameObject);
            else
                StatScrObj.ItemToRecieve = null;  //This is saved to an instantiated scriptable object, so does not get saved out of play

        }


        //ISaveble properties
        public string Identifier { get { return gameObject.name; } }

        public int State { get { return _saveState; } set { _saveState = value; } }

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
