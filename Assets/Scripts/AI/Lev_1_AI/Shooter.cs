using cowsins;
using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Shooter : MonoBehaviour
{
	public Weapon_SO weapon; // Reference to the weapon scriptable object
	public Transform[] firePoint; // Array of fire points for shooting
	private AiAgent _AiAgent; // Reference to the AiAgent component
	private int bulletsPerFire; // Number of bullets per fire
	RaycastHit hit; // Stores the information about the hit point
	[Tooltip("What objects should be hit")] public LayerMask hitLayer; // Layer mask for the objects that should be hit by the bullets
	public Effects effects; // Reference to the Effects scriptable object

	public bool canShoot; // Indicates whether the shooter can currently shoot
	public bool hitPlayer; // Indicates if the player was hit by the bullet

	private void Start()
	{
		canShoot = true;
		_AiAgent = GetComponent<AiAgent>();
	}

	public void Shoot()
	{
		int shootstyle = (int)weapon.shootStyle;
		// Hitscan or projectile
		if (shootstyle == 0 || shootstyle == 1)
		{
			foreach (var p in firePoint)
			{
				canShoot = false; // since you have already shot, you will have to wait in order to being able to shoot again
				bulletsPerFire = weapon.bulletsPerFire;
				StartCoroutine(HandleShooting());
			}

			if (weapon.timeBetweenShots == 0)
			{
				AudioManagerServices.instance.PlayOneShot(weapon.audioSFX.firing, new AudioSettings(1, 0, true));
			}

			//SoundManager.Instance.PlaySound(weapon.audioSFX.firing, 0, weapon.pitchVariationFiringSFX, 0);}
			Invoke("CanShoot", weapon.fireRate);
		}
		else if (shootstyle == 2) // Melee
		{
			canShoot = false;
			StartCoroutine(HandleShooting());
			Invoke("CanShoot", weapon.attackRate);
		}
		else // Check if this is a custom method weapon
		{
			// If we want to use fire Rate
			if (!weapon.continuousFire)
			{
				canShoot = false;
				Invoke("CanShoot", weapon.fireRate);
			}

			// Continuous fire
			// CustomShotMethod();
		}
	}

	private IEnumerator HandleShooting()
	{
		// Determine whether we are sending a raycast (hitscan weapon), we are spawning a projectile, or melee attacking
		int style = (int)weapon.shootStyle;

		// Determine weapon class / style
		// Dual shooting will be introduced coming soon
		switch (style)
		{
			case 0: // Hitscan
				int i = 0;
				while (i < bulletsPerFire)
				{
					HitscanShot();
					foreach (var p in firePoint)
					{
						if (weapon.muzzleVFX != null)
						{
							GameObject t = Instantiate(weapon.muzzleVFX, p.position, p.transform.rotation); // VFX
							Destroy(t, 1f);
						}
					}

					if (weapon.useProceduralShot) ProceduralShot.Instance.Shoot(weapon.proceduralShotPattern);


					if (weapon.timeBetweenShots != 0) SoundManager.Instance.PlaySound(weapon.audioSFX.firing, 0, weapon.pitchVariationFiringSFX, 0);

					// ProgressRecoil();

					yield return new WaitForSeconds(weapon.timeBetweenShots);
					i++;
				}
				yield break;
			case 2:
				// MeleeAttack(weapon.attackRange, weapon.damagePerHit);
				break;
		}
	}

	private void HitscanShot()
	{
		// events.OnShoot.Invoke();

		Transform hitObj;

		//This defines the first hit on the object
		Vector3 dir = CowsinsUtilities.GetSpreadDirection(weapon.spreadAmount, firePoint[0]) + Random.insideUnitSphere * _AiAgent.Inaccuracy;
		Ray ray = new Ray(firePoint[0].transform.position, dir);
		GetComponentInParent<Animator>().Play("ShootAnim");
		hitPlayer = Physics.Raycast(ray, out hit, weapon.bulletRange, hitLayer);
		if (hitPlayer)
		{
			// float damageMultiplier = 1.5f;
			// float dmg = .1f * damageMultiplier;
			float dmg = weapon.damagePerBullet * weapon.criticalDamageMultiplier;
			Hit(hit.collider.gameObject.layer, dmg, hit, true);
			hitObj = hit.collider.transform;

			// added by harsh 
			hit.collider.gameObject.transform.parent.GetComponent<PlayerHealth>().DamagePlayer(-dmg);

			//Handle Penetration
			Ray newRay = new Ray(hit.point, ray.direction);
			RaycastHit newHit;

			if (Physics.Raycast(newRay, out newHit, weapon.penetrationAmount, hitLayer))
			{
				if (hitObj != newHit.collider.transform)
				{
					float dmg_ = weapon.damagePerBullet * weapon.criticalDamageMultiplier * weapon.penetrationDamageReduction;
					Hit(newHit.collider.gameObject.layer, dmg_, newHit, true);
				}
			}
		}
	}

	private void Hit(LayerMask layer, float damage, RaycastHit h, bool damageTarget)
	{
		GameObject impact = null, impactBullet = null;

		if (h.collider != null && impactBullet != null)
		{
			impactBullet.transform.rotation = Quaternion.LookRotation(h.normal);
			impactBullet.transform.SetParent(h.collider.transform);
		}

		// Apply damage
		if (!damageTarget) return;
		if (h.collider.gameObject.CompareTag("Critical")) h.collider.transform.parent.GetComponent<IDamageable>().Damage(damage * weapon.criticalDamageMultiplier * GetDistanceDamageReduction(h.collider.transform));
		else if (h.collider.GetComponent<IDamageable>() != null) h.collider.GetComponent<IDamageable>().Damage(damage * GetDistanceDamageReduction(h.collider.transform));
	}

	private float GetDistanceDamageReduction(Transform target)
	{
		if (!weapon.applyDamageReductionBasedOnDistance) return 1;
		if (Vector3.Distance(target.position, transform.position) > weapon.minimumDistanceToApplyDamageReduction)
			return (weapon.minimumDistanceToApplyDamageReduction / Vector3.Distance(target.position, transform.position)) * weapon.damageReductionMultiplier;
		else return 1;
	}

	private void CanShoot() => canShoot = true;
}

