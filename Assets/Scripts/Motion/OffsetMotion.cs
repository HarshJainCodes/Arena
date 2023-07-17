using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class OffsetMotion : Motion
    {
        [Tooltip("The character's FeelManager component.")]
        [SerializeField]
        private FeelManager feelManager;

        [Tooltip("The character's Animator component.")]
        [SerializeField]
        private Animator characterAnimator;

        [Tooltip("The character's CharacterBehaviour component.")]
        [SerializeField]
        private AnimationParameters characterBehaviour;

        [Tooltip("The character's InventoryBehaviour component.")]
        [SerializeField]
        private Inventory inventoryBehaviour;


        [Tooltip("The type of motion we want this component to apply.")]
        [SerializeField]
        private MotionType motionType;

        private readonly Spring springLocation = new Spring();
        /// <summary>
        /// springRotation. Handles all rotation interpolation.
        /// </summary>
        private readonly Spring springRotation = new Spring();

        /// <summary>
        /// FeelState.
        /// </summary>
        private FeelState feelState;
        public override void Tick()
        {
            //Debug.Log("hi");

            //Check for reference errors.
            if (feelManager == null || characterBehaviour == null || inventoryBehaviour == null
                || characterAnimator == null)
            {
                //ReferenceError.
                // Log.ReferenceError(this, gameObject);

                //Return.
                return;
            }

            FeelPreset feelPreset = feelManager.Preset;
            if (feelPreset == null)
                return;

            //Get Feel.
            Feel feel = feelPreset.GetFeel(motionType);
            if (feel == null)
                return;

            //Try to get the equipped weapon. With the current asset setup, this should always return the correct thing.
            Weapon weaponBehaviour = inventoryBehaviour.GetEquipped();
            if (weaponBehaviour == null)
                return;

            //Get the equipped item's (in this case weapon) data component. It holds a lot of data needed for this.
            var itemAnimationDataBehaviour = weaponBehaviour.GetComponent<ItemAnimationData>();
            if (itemAnimationDataBehaviour == null)
                return;

            //Get the WeaponAttachmentManager component, which we will need to offset things even more.
            WeaponAttachmentManager weaponAttachmentManagerBehaviour = weaponBehaviour.GetAttachmentManager();
            if (weaponAttachmentManagerBehaviour == null)
                return;

            //Get the equipped scope.
            Scope scopeBehaviour = weaponAttachmentManagerBehaviour.GetEquippedScope();
            if (scopeBehaviour == null)
                return;

            //Grab the ItemOffsets.
            ItemOffsets itemOffsets = itemAnimationDataBehaviour.GetItemOffsets();
            if (itemOffsets == null)
                return;

            //Location.
            Vector3 location = default;
            //Rotation.
            Vector3 rotation = default;

            //Running.
            if (characterAnimator.GetBool(AHashes.Running))
            {
                //Add Offsets.
                location += itemOffsets.RunningLocation;
                rotation += itemOffsets.RunningRotation;

                //Set feelState.
                feelState = feel.Running;
            }
            else
            {
                //Aiming.
                if (characterAnimator.GetBool(AHashes.Aim))
                {
                    //Add Offsets.
                    location += itemOffsets.AimingLocation;
                    rotation += itemOffsets.AimingRotation;

                    //Add Scope Offsets.
                    location += scopeBehaviour.GetOffsetAimingLocation();
                    rotation += scopeBehaviour.GetOffsetAimingRotation();

                    //Set feelState.
                    feelState = feel.Aiming;
                }
                else
                {
                    //Crouching.
                    if (characterAnimator.GetBool(AHashes.Crouching))
                    {
                        //Add Offsets.
                        location += itemOffsets.CrouchingLocation;
                        rotation += itemOffsets.CrouchingRotation;

                        //Set feelState.
                        feelState = feel.Crouching;
                    }
                    //Standing.
                    else
                    {
                        //Add Offsets.
                        location += itemOffsets.StandingLocation;
                        rotation += itemOffsets.StandingRotation;

                        //Set feelState.
                        feelState = feel.Standing;
                    }
                }
            }

            //This animation value is used to determine when to not use offsets.
            float alphaActionOffset = characterAnimator.GetFloat(AHashes.AlphaActionOffset);

            //Add Action Values. These values are applied while throwing a grenade, and while meleeing.
            location += itemOffsets.ActionLocation * alphaActionOffset;
            rotation += itemOffsets.ActionRotation * alphaActionOffset;

            //Apply Offsets.
            location += feelState.Offset.OffsetLocation;
            rotation += feelState.Offset.OffsetRotation;

            //Update End Values.
            springLocation.UpdateEndValue(location);
            springRotation.UpdateEndValue(rotation);
        }


        /// <summary>
        /// GetLocation.
        /// </summary>
        public override Vector3 GetLocation()
        {
            //Check References.
            if (feelState.Offset == null)
                return default;

            //Return.
            return springLocation.Evaluate(feelState.Offset.SpringSettingsLocation);
        }
        /// <summary>
        /// GetEulerAngles.
        /// </summary>
        public override Vector3 GetEulerAngles()
        {
            //Check References.
            if (feelState.Offset == null)
                return default;

            //Return.
            return springRotation.Evaluate(feelState.Offset.SpringSettingsRotation);
        }

    }
}

