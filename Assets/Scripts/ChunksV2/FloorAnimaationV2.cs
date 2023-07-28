using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FloorAnimaationV2 : MonoBehaviour
{
    /// <summary>
    /// Initial position is y offset position below the normal level, so that this can be lerped to the final position to animate them
    /// </summary>
    private Vector3 initialPosition;
    /// <summary>
    /// Final position is where the floors would be after the animating of the floors is done.
    /// </summary>
    private Vector3 finalPosition;

    /// <summary>
    /// Controls how fast/long the animation is played
    /// </summary>
    private float _AnimationSpeed = 0.2f;

    /// <summary>
    /// This is used to check when all of the floors are animated so that the A* can scan the graph, 
    /// <para> If the A* scans while the floors are beinng animated then it will result in incorrect scanning of the graph </para>
    /// </summary>
    int noOfCoroutineRunning = 0;

    /// <summary>
    /// Reference to the chunk script
    /// </summary>
    [SerializeField] ChunkScriptV2 _ChunkScriptV2Script;

    /// <summary>
    /// Scan the graph Asynchronously so that it dosnt result in drop in FPS, after the graph is scanned then only we combine the tiles in the individual floors
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// This calls the coroutine that animates the chunks 
    /// </summary>
    public void AnimateFloorComingToTop()
    {
        foreach (Transform t in transform)
        {
            StartCoroutine(_Animate(t));
        }
    }

    /// <summary>
    /// This will use one of the lerping functions provided by the <see cref="Tween"/> Library, most probably you would want to use <see cref="Tween.InOutBack(float, float, float)"/> or <see cref="Tween.OutBack(float, float, float)"/>
    /// <para> t is the block transform so that it can be changed to animate the floors</para>
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
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

        // after all the blocks are animated then only we start scanning the graph otherwise it would result in incorrect scanning of the graph
        if (noOfCoroutineRunning == 0)
        {
            StartCoroutine(ScanGraph());
        }
    }
}
