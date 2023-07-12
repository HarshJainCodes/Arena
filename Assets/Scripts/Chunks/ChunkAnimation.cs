using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _CombinedCubeHolder;

    private IEnumerator _Animate(Transform t)
    {
        t.position = new Vector3(0, -500, 0);
        float riseSpeed = Random.Range(40, 150);

        while (t.position.y < 0)
        {
            t.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
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
