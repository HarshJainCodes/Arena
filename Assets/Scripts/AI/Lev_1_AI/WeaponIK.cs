using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
	// The transform of the target to aim at (usually the player)
	public Transform TargetTransform;

	// The transform representing the aim direction of the weapon
	public Transform AimTransform;

	// The bone or transform that will be rotated to aim at the target
	public Transform Bone;

	private AiAgent _AiAgent;

	// Number of iterations for the aiming process
	public int Iterations = 10;

	// Angle limit for aiming, beyond which no further rotation is allowed
	public float AngleLimit = 90f;

	// Distance limit for aiming, beyond which no further movement is allowed
	public float DistanceLimit = 1.5f;

	// Start is called before the first frame update
	void Start()
	{
		// Get a reference to the AiAgent component attached to this GameObject
		_AiAgent = GetComponent<AiAgent>();

		// Set the initial target transform to the player's transform
		TargetTransform = _AiAgent.PlayerTransform;
	}

	// Calculate the target position to aim at based on angle and distance limits
	Vector3 GetTargetPosition()
	{
		// Get the direction from the weapon's aim position to the target
		Vector3 targetDirection = TargetTransform.position - AimTransform.position;

		// Get the forward direction of the weapon
		Vector3 aimDirection = AimTransform.forward;

		// Variable to control the blend out of aiming based on angle and distance
		float blendout = 0.0f;

		// Calculate the angle between the target direction and the aim direction
		float targetAngle = Vector3.Angle(targetDirection, aimDirection);

		// If the target angle is greater than the angle limit, adjust the blendout
		if (targetAngle > AngleLimit)
			blendout += (targetAngle = AngleLimit) / 50.0f;

		// Calculate the distance between the target and the aim position
		float targetDistance = targetDirection.magnitude;

		// If the target is closer than the distance limit, adjust the blendout
		if (targetDistance < DistanceLimit)
			blendout += DistanceLimit - targetDistance;

		// Blend the target direction and the aim direction based on blendout
		Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendout);

		// Calculate the final target position based on the aim position and the blended direction
		return AimTransform.position + direction;
	}

	// LateUpdate is called once per frame, after Update
	void LateUpdate()
	{
		// Get the target position to aim at
		Vector3 targetPosition = GetTargetPosition();

		// Iterate multiple times to adjust the bone rotation for smoother aiming
		for (int i = 0; i < Iterations; i++)
		{
			// Adjust the bone rotation to aim at the target position
			AimAtTarget(Bone, targetPosition);
		}
	}

	// Rotate the bone to aim at the target position
	void AimAtTarget(Transform bone, Vector3 targetPosition)
	{
		// Get the current aim direction of the weapon
		Vector3 AimDirection = AimTransform.forward;

		// Get the direction from the aim position to the target position
		Vector3 TargetDirection = targetPosition - AimTransform.position;

		// Calculate the rotation needed to align the aim direction with the target direction
		Quaternion TargetRotation = Quaternion.FromToRotation(AimDirection, TargetDirection);

		// Apply the rotation to the bone to aim at the target
		bone.rotation = TargetRotation * bone.rotation;
	}
}
