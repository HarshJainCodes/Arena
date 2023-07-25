using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arena
{
    public class LowerWeapon : MonoBehaviour
    {


        [Tooltip("The character's Animator component.")]
        [SerializeField]
        private Animator characterAnimator;

        /*[Tooltip("A WallAvoidance component is required so we can check if the character is facing a wall " +
                 "and lower the weapon automatically. If there's no such component assigned, this will never " +
                 "happen.")]*/
        /*[SerializeField]
        private WallAvoidance wallAvoidance;
    */
        [Tooltip("The character's InventoryBehaviour component.")]
        [SerializeField]
        private Inventory inventoryBehaviour;

        [Tooltip("The character's CharacterBehaviour component.")]
        [SerializeField]
        private AnimationParameters characterBehaviour;


        [Tooltip("If true, the lowered state is stopped when the character starts firing.")]
        [SerializeField]
        private bool stopWhileFiring = true;

        private bool lowered;
        /// <summary>
        /// This becomes true when the player asks for the weapon to be lowered, but may not directly make the weapon
        /// lowered depending on other states that are active.
        /// </summary>
        private bool loweredPressed;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (characterAnimator == null || characterBehaviour == null || inventoryBehaviour == null)
            {
                //ReferenceError.
                //Log.ReferenceError(this, gameObject);

                //Return.
                return;
            }

            //Update the lowered variable.
            /*lowered = (loweredPressed || wallAvoidance != null && wallAvoidance.HasWall) && !characterBehaviour.IsAiming() && !characterBehaviour.IsRunning()
                      && !characterBehaviour.IsInspecting() && !characterBehaviour.IsHolstered();*/

            //Stop the lowered state while firing if necessary.
            //We use this by default, but it could be useful to not have it if your lowered poses are different.
            if (stopWhileFiring && characterBehaviour.IsHoldingButtonFire())
                lowered = false;

            //Make sure that the equipped weapon has a ItemAnimationDataBehaviour.
            var animationData = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationData>();
            if (animationData == null)
                lowered = false;
            else
            {
                //Check that the current weapon equipped has the necessary data for lowering.
                if (animationData.GetLowerData() == null)
                    lowered = false;
            }

            //Update Animator Lowered.
            characterAnimator.SetBool(AHashes.Lowered, lowered);
        }

        public bool IsLowered() => lowered;

        public void Lower(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.

            //No changing the lowered state while doing these, since you can't see it.
            if (characterBehaviour.IsAiming() || characterBehaviour.IsInspecting() ||
                characterBehaviour.IsRunning() || characterBehaviour.IsHolstered())
                return;

            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Toggle Lowered.
                    loweredPressed = !loweredPressed;
                    break;
            }
        }
    }
}

