using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
	public class BoltBehaviour : StateMachineBehaviour
	{
		private AnimationParameters playerCharacter;

		/// <summary>
		/// Player Inventory.
		/// </summary>
		private Inventory playerInventoryBehaviour;



		

		/// <summary>
		/// On State Enter.
		/// </summary>
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//We need to get the character component.
			playerCharacter = FindObjectOfType<AnimationParameters>();

			//Get Inventory.
			playerInventoryBehaviour ??= playerCharacter.GetInventory();

			//Try to get the equipped weapon's Weapon component.
			if (!(playerInventoryBehaviour.GetEquipped() is { } weaponBehaviour))
				return;

			//Get the weapon animator.
			var weaponAnimator = weaponBehaviour.gameObject.GetComponent<Animator>();
			//Play Bolt Action Animation.
			weaponAnimator.Play("Bolt Action");
		}

	}
}
