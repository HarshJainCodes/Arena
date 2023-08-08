using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    public Animator _jumpCheck;
    public GameObject _mainModel;
    public GameObject _chargeExplosionParticleSystem;
    public GameObject _explodeParticleSystem;
    [SerializeField] private float _explosionDamageRadius=10f;
    bool _Triggered = false;
    private PlayerHealth _target;
    [SerializeField] private AIDestinationSetter _targetSetter;


    private void Awake()
    {
        _chargeExplosionParticleSystem.SetActive(false);
        _explodeParticleSystem.SetActive(false);
        ///_target = GameObject.Find("PLayer").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if(_jumpCheck.GetBool("Explode") && !_Triggered)
        {
            _Triggered = true;
            StartCoroutine(damageTarget());
            StartCoroutine(particleTrigger());
            
        }
    }

    IEnumerator particleTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        _chargeExplosionParticleSystem.SetActive(true); 
        yield return new WaitForSeconds(1f);
        _chargeExplosionParticleSystem.SetActive(false);
        _mainModel.SetActive(false);
        _explodeParticleSystem.SetActive(true);
        yield return new WaitForSeconds(1f);
        _explodeParticleSystem.SetActive(false);
        Destroy(gameObject);
    }

    IEnumerator damageTarget()
    {
        yield return new WaitForSeconds(2.2f);
        _target=_targetSetter.target.GetComponent<PlayerHealth>() ;
        if(Vector3.Magnitude(_target.gameObject.transform.position-transform.position)<_explosionDamageRadius)
        {
            Debug.LogError("Reaching");
            _target.DamagePlayer(20f);
        }
        yield return null;
    }
}
