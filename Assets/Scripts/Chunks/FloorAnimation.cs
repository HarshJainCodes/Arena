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

        //this indicates that the A* has scanned all the arena
        // after this we can merge our floors
        CombineFloors.Instance.CombineFloorSegments();
    }

	private IEnumerator _Animate(Transform t)
    {
	    _NoOfCoroutines++;

        float randomPos = UnityEngine.Random.Range(-300, -500);
        float progressScale = UnityEngine.Random.Range(4, 6);
        t.position = new Vector3(0, randomPos, 0);

        float progress = 0;
        while (progress <= 1)
        {
            progress += Time.deltaTime / progressScale;

            float blockPos = Tween.InOutBack(randomPos, 0 - randomPos, progress);

            t.position = new Vector3(0, blockPos, 0);
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
