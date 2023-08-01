using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapCamOrient : MonoBehaviour
{
    [SerializeField]
    Transform player;
    private void Update()
    {
        gameObject.transform.localEulerAngles = player.localEulerAngles;
        gameObject.transform.localEulerAngles = new Vector3(90f, gameObject.transform.localEulerAngles.y, 0f);
    }
}
