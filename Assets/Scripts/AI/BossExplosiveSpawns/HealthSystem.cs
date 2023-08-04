using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] AIPath _setter;
    int _health;
    int _maxHealth;
    bool _deathTriggered=false;
    // Start is called before the first frame update
    void Start()
    {
        _maxHealth = 30;
        _health = _maxHealth;
    }

    // Update is called once per frame

    public void decreaseHealth(int damage)
    {
        _health = (_health - damage) < 0 ? 0 : _health - damage;
        if(_health==0)
        {
            StartCoroutine(death());
        }
    }

    IEnumerator death()
    {
        _setter.enabled = false;
        yield return new WaitForSeconds(0f);
        _animator.SetBool("Jump",true);
        //Destroy(gameObject.GetComponentInParent<Transform>().gameObject);
    }
}
