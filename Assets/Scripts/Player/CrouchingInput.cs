using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arena
{
    public class CrouchingInput : MonoBehaviour
    {


        [Tooltip("The character's CharacterBehaviour component.")]
        [SerializeField]
        private AnimationParameters characterBehaviour;

        [Tooltip("The character's MovementBehaviour component.")]
        [SerializeField]
        private PlayerMovement movementBehaviour;


        [Tooltip("If true, the crouch button has to be held to keep crouching.")]
        [SerializeField]
        private bool holdToCrouch;



        private bool holding;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (holdToCrouch)
                movementBehaviour.TryCrouch(holding);
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            //Check that all our references are correctly assigned.
            if (characterBehaviour == null || movementBehaviour == null)
            {
                //ReferenceError.
                //Log.ReferenceError(this, this.gameObject);

                //Return.
                return;
            }

            //Block while the cursor is unlocked.
            //if (!characterBehaviour.IsCursorLocked())
            //  return;

            //Switch.
            switch (context.phase)
            {
                //Started.
                case InputActionPhase.Started:
                    holding = true;
                    break;
                //Performed.
                case InputActionPhase.Performed:
                    //TryToggleCrouch.
                    if (!holdToCrouch)
                        movementBehaviour.TryToggleCrouch();
                    break;
                //Canceled.
                case InputActionPhase.Canceled:
                    holding = false;
                    break;
            }
        }
    }

}
