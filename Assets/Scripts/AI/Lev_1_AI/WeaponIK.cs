using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    public Transform TargetTransform;
    public Transform AimTransform;
    public Transform Bone;
    private AiAgent _AiAgent;
    public int Iterations = 10;
    public float AngleLimit = 90f;
    public float DistanceLimit = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
	    _AiAgent = GetComponent<AiAgent>();
	    TargetTransform = _AiAgent.playerTransform;
    }

    Vector3 GetTargetPosition()
    {
	    Vector3 targetDirection = TargetTransform.position - AimTransform.position;
        Vector3 aimDirection = AimTransform.forward;
        float blendout = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > AngleLimit)
	        blendout += (targetAngle = AngleLimit) / 50.0f;

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < DistanceLimit)
	        blendout += DistanceLimit - targetDistance;

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendout);
        return AimTransform.position + direction;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < Iterations; i++)
        {
			AimAtTarget(Bone, targetPosition + Random.insideUnitSphere * _AiAgent.Inaccuracy);
        }

    }

    void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
		Vector3 AimDirection = AimTransform.forward;
        Vector3 TargetDirection = targetPosition - AimTransform.position;
        Quaternion TargetRotation = Quaternion.FromToRotation(AimDirection, TargetDirection);
        bone.rotation = TargetRotation * bone.rotation;
	}
}
