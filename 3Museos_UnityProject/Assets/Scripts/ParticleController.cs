using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    #region Variabels
    [Header("Proporties")]
    [SerializeField] private bool _destroyAfterDuration = false;

    [Header("Components")]
    [SerializeField] private ParticleSystem PS;
    #endregion

    #region Initialization
    private void Start()
    {
        #region Get components
        if (PS == null)
            PS = GetComponentInChildren<ParticleSystem>();
        #endregion

        //destroy after duration
        if (_destroyAfterDuration)
            Destroy(this.gameObject, PS.main.duration);
    }
    #endregion

    #region Functions
    public void PlayParticle()
    {
        if (!PS.isPlaying)
            PS.Play();
        else
            PS.Stop();
    }
    public void StopParticle()
    {
        PS.Stop();
    }
    #endregion
}
