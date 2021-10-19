using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Museos;
using Museos.Saving;

public class TestSavingObject : MonoBehaviour, ISaveble
{
    public string Identifier { get { return gameObject.name; }}
    public int States;
    public int State { get { return States; } set { States = value; } }

    public void LoadObject()
    {
        State = GameSave.CurrentSave.LoadState(Identifier);
    }

    public void SaveObject()
    {
        GameSave.CurrentSave.SaveState(Identifier, State);
    }


    // Start is called before the first frame update
    void Start()
    {
        SaveObject();
    }
}
