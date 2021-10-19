using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Element : MonoBehaviour
{
    public static List<UI_Element> AllUI = new List<UI_Element>();

    private void Start()
    {
        AllUI.Add(this);
    }


    public void Open()
    {
        gameObject.SetActive(true);
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }

    public static void CloseAll()
    {
        foreach(UI_Element ui in AllUI)
        {
            ui.Close();
        }
    }


    private void OnDestroy()
    {
        if(AllUI.Contains(this))
            AllUI.Remove(this);
    }
}
