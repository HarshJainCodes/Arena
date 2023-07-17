using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotionType { Camera, Item }

[RequireComponent(typeof(MotionApplier))]
public abstract class Motion : MonoBehaviour
{
    public float Alpha => alpha;

    [Tooltip("The Motion's alpha. Used to more easily control how much of the motion is applied.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float alpha = 1.0f;

    

    [Tooltip("The MotionApplier that will apply this Motion's values.")]
    [SerializeField]
    protected MotionApplier motionApplier;
    protected virtual void Awake()
    {
        //Try to get the applier if we haven't assigned it.
        if (motionApplier == null)
            motionApplier = GetComponent<MotionApplier>();

        //Subscribe.
        if (motionApplier != null)
            motionApplier.Subscribe(this);
    }

    public abstract void Tick();
    public abstract Vector3 GetLocation();

    public abstract Vector3 GetEulerAngles();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
