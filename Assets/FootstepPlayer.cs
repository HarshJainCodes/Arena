using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class FootstepPlayer : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement playerMovement;

        [SerializeField]
        private Animator playerAnimator;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private float minVelocityMagnitude = 1.0f;

        [SerializeField]
        private AudioClip audioClipWalking;

        [SerializeField]
        private AudioClip audioClipRunning;

        private void Awake()
        {
            if(audioSource!=null)
            {
                audioSource.clip = audioClipWalking;
                audioSource.loop = true;
            }
        }


        private void Update()
        {
            if (playerAnimator == null || playerMovement == null || audioSource == null)
            {
             
                return;
            }

            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (playerMovement.IsPlayerGrounded() && playerMovement.GetVelocity().sqrMagnitude > minVelocityMagnitude)
            {
                //Select the correct audio clip to play.
                audioSource.clip = playerAnimator.GetBool(AHashes.Running) ? audioClipRunning : audioClipWalking;
                //Play it!
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            //Pause it if we're doing something like flying, or not moving!
            else if (audioSource.isPlaying)
                audioSource.Pause();
        }
    }
}
