using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChunkAnimationV2 : MonoBehaviour
{

    private Vector3 initialPosition;
    private Vector3 finalPosition;

    private float _AnimationSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AnimateChunkComingToTop()
    {
        finalPosition = transform.position;
        initialPosition = transform.position - new Vector3(0, 1000, 0);

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
