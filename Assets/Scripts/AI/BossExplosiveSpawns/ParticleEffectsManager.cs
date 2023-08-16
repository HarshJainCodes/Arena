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
    [SerializeField] private float _explosionDamageRadius=20f;
    bool _Triggered = false;
    private Transform _target;
    [SerializeField] private AIDestinationSetter _targetSetter;


    private void Awake()
    {

        _chargeExplosionParticleSystem.SetActive(false);
        _explodeParticleSystem.SetActive(false);
        //_target = GameObject.Find("PLayer").GetComponent<PlayerHealth>();
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
        _target = _targetSetter.target;
        yield return new WaitForSeconds(0.5f);
       
        _target= _targetSetter.target;
        if(Vector3.Magnitude(_target.position-GetComponentInChildren<Transform>().position)<20f)
        {
            GetComponentInParent<MinionDamageStore>().setDamage(10f);
        }
        //_target=_targetSetter.target.GetComponent<PlayerHealth>() ;
        
  
        Debug.LogError("Reaching");
        yield return null;
    }
}
