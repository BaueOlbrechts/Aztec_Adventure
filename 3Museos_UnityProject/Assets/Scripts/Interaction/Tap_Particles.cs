using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Interaction
{
    public class Tap_Particles : MonoBehaviour
    {
        public static string[] MaterialList = new[] { "None", "Vegetation", "Stone", "Ground", "Water" };
        public GameObject[] ParticlePrefabs = new GameObject[MaterialList.Length];
        //private float _newScale = 0.3f;
        private Dictionary<string, GameObject> _dictionary = new Dictionary<string, GameObject>();

        private void Start()
        {
            //transform.localScale = new Vector3(_newScale, _newScale, _newScale);

            for (int i = 0; i < MaterialList.Length; i++)
            {
                if (ParticlePrefabs[i] == null)
                    continue;

                var go = Instantiate(ParticlePrefabs[i]);
                go.transform.SetParent(gameObject.transform);
                //go.GetComponent<ParticleSystem>().gravityModifier *= _newScale;
                //go.transform.localScale = Vector3.one;
                _dictionary.Add(MaterialList[i], go);
            }
        }

        public void TappedOnObject(string materialName, Vector3 position)
        {
            if (materialName == MaterialList[0])
                return;

            _dictionary.TryGetValue(materialName, out GameObject particleObject);
            particleObject.transform.position = position;
            particleObject.GetComponent<ParticleSystem>().Play();
        }
    }
}
