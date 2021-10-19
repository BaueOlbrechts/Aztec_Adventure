using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveble 
{
    string Identifier { get; }

    int State { get; set; }

    void SaveObject();

    void LoadObject();
}
