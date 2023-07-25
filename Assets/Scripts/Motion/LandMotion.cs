using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class LandMotion : Motion
    {
        #region FIELDS SERIALIZED

        [Tooltip("Reference to the character's FeelManager component.")]
        [SerializeField]
        private FeelManager feelManager;

        [Tooltip("Reference to the character's MovementBehaviour component.")]
        [SerializeField]
        private PlayerMovement movementBehaviour;

        [Tooltip("The character's Animator component.")]
        [SerializeField]
        private Animator characterAnimator;


        [Tooltip("The type of this motion.")]
        [SerializeField]
        private MotionType motionType;

        #endregion

        #region FIELDS

        /// <summary>
        /// The location spring.
        /// </summary>
        private readonly Spring springLocation = new Spring();
        /// <summary>
        /// The rotation spring.
        /// </summary>
        private readonly Spring springRotation = new Spring();

        /// <summary>
        /// Represents the curves currently being played by this component.
        /// </summary>
        private ACurves playedCurves;

        /// <summary>
        /// Time.time at which the character last landed on the ground.
        /// </summary>
        private float landingTime;

        #endregion

        #region METHODS

        /// <summary>
        /// Tick.
        /// </summary>
        public override void Tick()
        {
            //Debug.Log("1");
            //Check References.
            if (feelManager == null || movementBehaviour == null)
            {
                //ReferenceError.
                //Log.ReferenceError(this, gameObject);

                //Return.
                return;
            }

            //Debug.Log("2");

            //Get Feel.
            Feel feel = feelManager.Preset.GetFeel(motionType);
            if (feel == null)
            {
                //ReferenceError.
                // Log.ReferenceError(this, gameObject);

                //Return.
                return;
            }
            //Debug.Log("3");

            //Location.
            Vector3 location = default;
            //Rotation.
            Vector3 rotation = default;

            //We store the landing time.
            if (movementBehaviour.IsPlayerGrounded() && !movementBehaviour.WasGrounded())
            {

                landingTime = Time.time;
                //Debug.Log("4");

            }

            //Debug.Log("5");

            //We start playing the landing curves.
            playedCurves = feel.GetState(characterAnimator).LandingCurves;

            //Time where we evaluate the landing curves.
            //Debug.Log(Time.time +" " + " " + landingTime);
            float evaluateTime = Time.time - landingTime;
            // Debug.Log(evaluateTime);
            //Evaluate Location Curves.
            location += playedCurves.LocationCurves.EvaluateCurves(evaluateTime);
            //Evaluate Rotation Curves.
            rotation += playedCurves.RotationCurves.EvaluateCurves(evaluateTime);

            //Evaluate Location Curves.
            springLocation.UpdateEndValue(location);
            //Evaluate Rotation Curves.
            springRotation.UpdateEndValue(rotation);
        }

        #endregion

        #region FUNCTIONS

        /// <summary>
        /// GetLocation.
        /// </summary>
        public override Vector3 GetLocation()
        {
            //Check References.
            if (playedCurves == null)
                return default;

            //Return.
            return springLocation.Evaluate(playedCurves.LocationSpring);
        }
        /// <summary>
        /// GetEulerAngles.
        /// </summary>
        public override Vector3 GetEulerAngles()
        {
            //Check References.
            if (playedCurves == null)
                return default;

            //Return.
            return springRotation.Evaluate(playedCurves.RotationSpring);
        }

        #endregion
    }
}

