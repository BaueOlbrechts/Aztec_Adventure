using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Interaction
{
    [CreateAssetMenu(fileName = "Scr_Obj_Basic_Static_Name", menuName = "ScriptableObjects/Basic Static Scene Object")]
    public class Basic_Static_ScriptableObject : ScriptableObject
    {
        [Header("Settings")]

        [HideInInspector]
        public string Material = "";

        public AudioClip AudioClip = null;
    }
}
