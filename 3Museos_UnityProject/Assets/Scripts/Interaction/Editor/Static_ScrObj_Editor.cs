using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Interaction.Editor
{
    [CustomEditor(typeof(Static_ScriptableObject))]
    public class Static_ScrObj_Editor : UnityEditor.Editor
    {
        //public static string[] MaterialList = new[] { "None", "Vegetation", "Stone", "Ground" };

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            //DrawDefaultInspector();
            //_materialIndex = EditorGUILayout.Popup("Material for particles",_materialIndex, MaterialList);
            //var stat_ScrObj = target as Static_ScriptableObject;
            //stat_ScrObj.Material = MaterialList[_materialIndex];
            //EditorUtility.SetDirty(target);
            string[] MaterialList = Tap_Particles.MaterialList;

            DrawDefaultInspector();
            var materialSP = serializedObject.FindProperty("Material");
            var oldIdx = Array.IndexOf(MaterialList, materialSP.stringValue);
            var newIdx = EditorGUILayout.Popup("Material for particles", oldIdx, MaterialList);

            if (oldIdx != newIdx)
                materialSP.stringValue = MaterialList[newIdx];

            serializedObject.ApplyModifiedProperties();
        }
    }
}
