using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    [SerializeField] public int HealthGain = 20;

    Transform _PlayerTransform;

    float _InitialPosX;
    float _InitialPosY;
    float _InitialPosZ;

    float progress;

    // Start is called before the first frame update
    void Start()
    {
        _InitialPosX = transform.position.x;
        _InitialPosY = transform.position.y;
        _InitialPosZ = transform.position.z;

        _PlayerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime;

        if (progress > 1)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 finalPos = new Vector3();

        finalPos.x = Tween.Linear(_InitialPosX, _PlayerTransform.position.x - _InitialPosX, progress);
        finalPos.y = Tween.Linear(_InitialPosY, _PlayerTransform.position.y -  _InitialPosY, progress);
        finalPos.z = Tween.Linear(_InitialPosZ, _PlayerTransform.position.z - _InitialPosZ, progress);

        transform.position = finalPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("player picked up the health");
            //Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
