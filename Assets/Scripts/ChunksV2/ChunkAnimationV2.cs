using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChunkAnimationV2 : MonoBehaviour
{
    /// <summary>
    /// Initial position is the offset position some y heights below the original or final position.
    /// </summary>
    private Vector3 initialPosition;
    /// <summary>
    /// Final position is the cached variable of the original position or transform.position
    /// </summary>
    private Vector3 finalPosition;

    /// <summary>
    /// This is used to control the speed of the animation. Maybe we can make it public to have different speed for different objects
    /// </summary>
    private float _AnimationSpeed = 0.2f;

    /// <summary>
    /// This coroutine will lerp the position of the gameobject based on the various easing function provided in the <see cref="Tween"/> class
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateChunkComingToTop()
    {
        finalPosition = transform.position;
        initialPosition = transform.position - new Vector3(0, 1000, 0);

        // this will go linearly from 0 to 1, rate of change controlled by the animation speed variable
        float progress = 0;
        float TweenPosition;

        while (progress <= 1)
        {
            TweenPosition = Tween.OutBack(initialPosition.y, finalPosition.y - initialPosition.y, progress);
            transform.position = new Vector3(transform.position.x, TweenPosition, transform.position.z);
            progress += Time.deltaTime * _AnimationSpeed;

            yield return null;
        }
        transform.position = finalPosition;
    }
}
