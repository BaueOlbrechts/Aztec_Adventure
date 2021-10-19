using UnityEngine;

[CreateAssetMenu(fileName = "Scr_Obj_Character_Name", menuName = "ScriptableObjects/Character")]
public class Character_ScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Quest
    {
        //public Sprite[] DialogueSprites = null;
        public Dialogue[] Dialogue = null;

        public Item ItemToRecieve = null;
        public Item ItemToGive = null;
    }

    [System.Serializable]
    public class Dialogue
    {
        public Sprite DialogueSprite = null;
        public string DialogueText = "";
    }

    [Header("Dialogue")]
    [Tooltip("The last dialogue option should not have trading!")]
    public Quest[] DialogueOptions = null;

    [Header("Settings")]
    public float TimeBetweenDialogue = 3f;
    public AudioClip TalkingAudio = null;
    public AudioClip QuestCompleteAudio = null;
    public AudioClip WrongItemAudio = null;
    public RuntimeAnimatorController RuntimeAnimatorController = null;
    public AnimationClip AnimationClip = null;
}
