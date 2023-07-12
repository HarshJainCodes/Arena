using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _CombinedCubeHolder;

    private IEnumerator _Animate(Transform t)
    {
        float RandomPos = UnityEngine.Random.Range(-300, -500);
        float progressScale = UnityEngine.Random.Range(4, 6);
        t.position = new Vector3(0, RandomPos, 0);

        float progress = 0;
        while (progress <= 1)
        {
            progress += Time.deltaTime / progressScale;

            // linear 
            float blockPosition = Tween.InOutBack(RandomPos, 0 - RandomPos, progress);

            //quad
            //float tValue = Mathf.Pow(progress, 2);

            // smoothstep
            //float tValue = progress * progress * (3 - 2 * progress);

            t.position = new Vector3(0, blockPosition, 0);
            yield return null;
        }
        t.position = Vector3.zero;
    }

    public void _AnimateCube()
    {
        foreach (Transform t in _CombinedCubeHolder.transform)
        {
            StartCoroutine(_Animate(t));
        }
    }
}
