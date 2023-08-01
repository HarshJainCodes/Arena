using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapcammovement : MonoBehaviour
{
    [SerializeField]
    Transform CamTransform;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles = CamTransform.eulerAngles;
        gameObject.transform.eulerAngles = new Vector3(90f, gameObject.transform.eulerAngles.y-90, 0f);
    }
}
