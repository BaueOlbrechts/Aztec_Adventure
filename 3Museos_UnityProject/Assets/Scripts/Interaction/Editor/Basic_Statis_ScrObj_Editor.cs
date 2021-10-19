using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Interaction.Editor
{
    [CustomEditor(typeof(Basic_Static_ScriptableObject))]
    public class Basic_Statis_ScrObj_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            string[] MaterialList = Tap_Particles.MaterialList;
            DrawDefaultInspector();
            var materialSP = serializedObject.FindProperty("Material");
            var oldIdx = Array.IndexOf(MaterialList, materialSP.stringValue);
            oldIdx = (oldIdx < 0) ? 0 : oldIdx;
            var newIdx = EditorGUILayout.Popup("Material for particles", oldIdx, MaterialList);

            if (oldIdx != newIdx)
                materialSP.stringValue = MaterialList[newIdx];

            serializedObject.ApplyModifiedProperties();
        }
    }
}
