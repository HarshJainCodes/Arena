using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FloorAnimaationV2 : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 finalPosition;

    private float _AnimationSpeed = 0.2f;

    int noOfCoroutineRunning = 0;

    [SerializeField] ChunkScriptV2 _ChunkScriptV2Script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScanGraph()
    {
        foreach (Progress p in AstarData.active.ScanAsync())
        {
            yield return null;
        }
        _ChunkScriptV2Script.IsGridGenerated = true;

        foreach (Transform t1 in transform)
        {
            t1.GetComponent<MeshCombinerScript>().CombineMesh();
        }
    }

    public void AnimateFloorComingToTop()
    {
        foreach (Transform t in transform)
        {
            StartCoroutine(_Animate(t));
        }
    }

    IEnumerator _Animate(Transform t)
    {
        finalPosition = transform.position;
        initialPosition = transform.position - new Vector3(0, 1000, 0);

        float progress = 0;
        float TweenPosition;

        noOfCoroutineRunning++;

        while (progress <= 1)
        {
            TweenPosition = Tween.OutBack(initialPosition.y, finalPosition.y - initialPosition.y, progress);
            t.position = new Vector3(t.position.x, TweenPosition, t.position.z);
            progress += Time.deltaTime * _AnimationSpeed;

            yield return null;
        }
        t.position = finalPosition;

        noOfCoroutineRunning--;

        if (noOfCoroutineRunning == 0)
        {
            StartCoroutine(ScanGraph());
        }
    }
}
