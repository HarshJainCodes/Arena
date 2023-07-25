using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{

    [SerializeField] private GameObject _HealthPack;
    [SerializeField] private GameObject _AmmoPack;

    public enum EnemyDrops{
        HealthPack,
        AmmoPack
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop(EnemyDrops e)
    {
        switch (e)
        {
            case EnemyDrops.HealthPack:
                GameObject healthPack = Instantiate(_HealthPack, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                healthPack.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.VelocityChange);
                healthPack.GetComponentInChildren<Rigidbody>().AddRelativeTorque(new Vector3(0, 5, 0), ForceMode.Impulse);
                break;
            case EnemyDrops.AmmoPack:
                GameObject ammoPack = Instantiate(_AmmoPack, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                ammoPack.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.VelocityChange);
                ammoPack.GetComponentInChildren<Rigidbody>().AddRelativeTorque(new Vector3(0, 5, 0), ForceMode.Impulse);
                break;
        }
    }
}
