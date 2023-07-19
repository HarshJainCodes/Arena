using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class PlaySoundBehaviour : StateMachineBehaviour
    {

        [Tooltip("AudioClip to play!")]
        [SerializeField]
        private AudioClip clip;


        [Tooltip("Audio Settings.")]
        [SerializeField]
        private AudioSettings settings = new AudioSettings(1.0f, 0.0f, true);

        /// <summary>
        /// Audio Manager Service. Handles all game audio.
        /// </summary>


    

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Try grab a reference to the sound managing service.
            

            //Play!
            AudioManagerServices.instance.PlayOneShot(clip, settings);
        }
    }
}
