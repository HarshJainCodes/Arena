using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class LeaningMotion : Motion
    {
        [Tooltip("The character's InventoryBehaviour component.")]
        [SerializeField]
        private Inventory inventoryBehaviour;

        [Tooltip("The character's CharacterBehaviour component.")]
        [SerializeField]
        private AnimationParameters characterBehaviour;

        [Tooltip("The character's Animator component.")]
        [SerializeField]
        private Animator characterAnimator;


        [Tooltip("The type of motion we want this component to apply.")]
        [SerializeField]
        private MotionType motionType;

        private readonly Spring springLocation = new Spring();
        /// <summary>
        /// Spring Rotation. Used to get the GameObject leaning.
        /// </summary>
        private readonly Spring springRotation = new Spring();

        /// <summary>
        /// Leaning curves to play.
        /// </summary>
        private ACurves leaningCurves;

        public override void Tick()
        {
            //Check for reference errors.
            if (inventoryBehaviour == null || characterBehaviour == null || characterAnimator == null)
            {
                //ReferenceError.
                //Log.ReferenceError(this, gameObject);

                //Return.
                return;
            }

            //Try to get an ItemAnimationDataBehaviour from the equipped weapon.
            var animationDataBehaviour = inventoryBehaviour.GetEquipped().GetComponent<ItemAnimationData>();
            //If there's none, then we don't even need to run this script at all, basically.
            if (animationDataBehaviour == null)
                return;

            //Try to get the LeaningData.
            LeaningData leaningData = animationDataBehaviour.GetLeaningData();
            if (leaningData == null)
                return;

            //This returns the correct leaning curves to use based on whether the character is aiming.
            leaningCurves = leaningData.GetCurves(motionType, characterBehaviour.IsAiming());
            //Check Reference.
            if (leaningCurves == null)
            {
                //Reset.
                springLocation.UpdateEndValue(default);
                springRotation.UpdateEndValue(default);

                //Return.
                return;
            }

            //Grab the leaning value from the character's Animator.
            float leaning = characterAnimator.GetFloat(AHashes.LeaningInput);

            //Update Location.
            springLocation.UpdateEndValue(leaningCurves.LocationCurves.EvaluateCurves(leaning) * leaningCurves.LocationMultiplier);
            //Update Rotation.
            springRotation.UpdateEndValue(leaningCurves.RotationCurves.EvaluateCurves(leaning) * leaningCurves.RotationMultiplier);
        }




        /// <summary>
        /// GetLocation.
        /// </summary>
        public override Vector3 GetLocation()
        {
            //Check Reference.
            if (leaningCurves == null)
                return default;

            //Return.
            return springLocation.Evaluate(leaningCurves.LocationSpring);
        }
        /// <summary>
        /// GetEulerAngles.
        /// </summary>
        public override Vector3 GetEulerAngles()
        {
            //Check Reference.
            if (leaningCurves == null)
                return default;

            //Return.
            return springRotation.Evaluate(leaningCurves.RotationSpring);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

