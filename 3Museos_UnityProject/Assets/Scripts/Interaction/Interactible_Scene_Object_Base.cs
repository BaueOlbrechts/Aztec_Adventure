using UnityEngine;
using UnityEngine.EventSystems;

namespace Interaction
{
    [RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Collider))]
    public abstract class Interactible_Scene_Object_Base : MonoBehaviour, IPointerClickHandler
    {
        internal AudioSource _audioSource;

        internal virtual void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Object tapped");
        }
    }
}
