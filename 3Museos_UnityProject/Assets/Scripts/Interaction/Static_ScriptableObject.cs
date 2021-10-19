using UnityEngine;

[CreateAssetMenu(fileName = "Scr_Obj_Static_Name", menuName = "ScriptableObjects/Static Scene Object")]
public class Static_ScriptableObject : ScriptableObject
{
    [Header("Settings")]

    [HideInInspector]
    public string Material = "";

    public AudioClip AudioClip = null;
    public RuntimeAnimatorController RuntimeAnimatorController = null;
    public AnimationClip AnimationClip = null;
    public Item ItemToRecieve = null;
    public bool RemoveAfterRecieving = false;

    // Need to find a way to get a list of possible strings to choose from
}
