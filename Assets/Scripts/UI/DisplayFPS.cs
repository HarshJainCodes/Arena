using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFPS : MonoBehaviour
{
    private float _Count;

    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            _Count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 40, 100, 25), "FPS : " + Mathf.Round(_Count));
    }
}
