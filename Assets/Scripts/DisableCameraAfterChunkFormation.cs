using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCameraAfterChunkFormation : MonoBehaviour
{
    [SerializeField] private GameObject PlayerCam;
    [SerializeField] private float TimeToDisableCam;
    IEnumerator DisableCamea()
    {
        float t = 0;

        while (t < TimeToDisableCam)
        {
            t += Time.deltaTime;
            yield return null;
        }

        PlayerCam.SetActive(true);
        Destroy(gameObject);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisableCamea());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
