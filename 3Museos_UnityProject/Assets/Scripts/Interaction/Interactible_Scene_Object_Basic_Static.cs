using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction
{
    public class Interactible_Scene_Object_Basic_Static : Interactible_Scene_Object_Base
    {
        public Basic_Static_ScriptableObject StatScrObj = null;
        private bool _canBeTapped = true;
        private float _waitTime = 1.5f;

        internal override void Start()
        {
            base.Start();
            _audioSource.clip = StatScrObj.AudioClip;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_canBeTapped)
            {
                base.OnPointerClick(eventData);
                _audioSource.Play();
                Museos.GameLoop.Instance.gameObject.GetComponent<Tap_Particles>().TappedOnObject(StatScrObj.Material, eventData.pointerCurrentRaycast.worldPosition);
                StartCoroutine(TapCooldown(_waitTime));
            }
        }

        private IEnumerator TapCooldown(float waitTime)
        {
            _canBeTapped = false;
            yield return new WaitForSeconds(waitTime);
            _canBeTapped = true;
        }
    }
}
