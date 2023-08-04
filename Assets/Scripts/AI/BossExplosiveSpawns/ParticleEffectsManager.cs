using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    public Animator _jumpCheck;
    public GameObject _mainModel;
    public GameObject _chargeExplosionParticleSystem;
    public GameObject _explodeParticleSystem;
    bool _Triggered = false;

    private void Awake()
    {
        _chargeExplosionParticleSystem.SetActive(false);
        _explodeParticleSystem.SetActive(false);
    }

    private void Update()
    {
        if(_jumpCheck.GetBool("Jump") && !_Triggered)
        {
            _Triggered = true;
            StartCoroutine(particleTrigger());
        }
    }

    IEnumerator particleTrigger()
    {
        //yield return new WaitForSeconds(1f);
        _chargeExplosionParticleSystem.SetActive(true);
        yield return new WaitForSeconds(1f);
        _chargeExplosionParticleSystem.SetActive(false);
        _mainModel.SetActive(false);
        _explodeParticleSystem.SetActive(true);
        yield return new WaitForSeconds(1f);
        _explodeParticleSystem.SetActive(false);
        Destroy(gameObject);
    }
}
