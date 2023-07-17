using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactScript : MonoBehaviour
{
    [Header("Impact Despawn Timer")]
    //How long before the impact is destroyed
    public float despawnTimer = 10.0f;
    // Start is called before the first frame update
    private void Start()
    {
        // Start the despawn timer
        StartCoroutine(DespawnTimer());

        //Get a random impact sound from the array
        
    }

    private IEnumerator DespawnTimer()
    {
        //Wait for set amount of time
        yield return new WaitForSeconds(despawnTimer);
        //Destroy the impact gameobject
        Destroy(gameObject);
    }
}
