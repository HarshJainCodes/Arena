using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FloorAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _CombineFloorHolder;
    private int _NoOfCoroutines = 0;

    IEnumerator ScanGraph()
    {
	    foreach (Progress p in AstarData.active.ScanAsync())
	    {
		    yield return null;
	    }
    }

	private IEnumerator _Animate(Transform t)
    {
	    _NoOfCoroutines++;


		t.position = new Vector3(0, -500, 0);
        float riseSpeed = Random.Range(40, 150);

        while (t.position.y < 0)
        {
            t.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
            yield return null;
        }
        t.position = Vector3.zero;
        _NoOfCoroutines--;
        if (_NoOfCoroutines == 0)
        {
	        StartCoroutine(ScanGraph());

	        GetComponent<ChunkCreator>().IsGridGenerated = true;
        }
    }

    public void _AnimateFloor()
    {
        foreach (Transform t in _CombineFloorHolder.transform)
        {
            StartCoroutine(_Animate(t));
        }
    }
}
