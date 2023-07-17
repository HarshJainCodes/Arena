using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arena
{
    public class LeaningInput : MonoBehaviour
    {

        [Tooltip("The character's CharacterBehaviour component.")]
        [SerializeField]
        private AnimationParameters characterBehaviour;

        [Tooltip("The character's Animator component.")]
        [SerializeField]
        private Animator characterAnimator;

        private float leaningInput;
        /// <summary>
        /// True If Leaning.
        /// </summary>
        private bool isLeaning;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            isLeaning = (leaningInput != 0.0f);

            //Update AHashes.LeaningInput Float.
            characterAnimator.SetFloat(AHashes.LeaningInput, leaningInput);
            //Update AHashes.Leaning Bool.
            characterAnimator.SetBool(AHashes.Leaning, isLeaning);
        }

        public void Lean(InputAction.CallbackContext context)
        {
            /*//Block while the cursor is unlocked.
            if (!characterBehaviour.IsCursorLocked())
            {
                //Zero out the leaning.
                leaningInput = 0.0f;

                //Return.
                return;
            }*/

            //ReadValue.
            leaningInput = context.ReadValue<float>();
        }

        public void SetLeaningInput(int value)
        {
            leaningInput = value;
        }
    }
}

