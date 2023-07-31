using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnChange : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,300,gameObject.transform.position.z);
    }
}
