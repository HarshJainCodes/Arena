using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class PlaySoundCharacterBehaviour : StateMachineBehaviour
    {
        private enum SoundType
        {
            //Character Actions.
            GrenadeThrow, Melee,
            //Holsters.
            Holster, Unholster,
            //Normal Reloads.
            Reload, ReloadEmpty,
            //Cycled Reloads.
            ReloadOpen, ReloadInsert, ReloadClose,
            //Firing.
            Fire, FireEmpty,
            //Bolt.
            BoltAction
        }


        [Tooltip("Delay at which the audio is played.")]
        [SerializeField]
        private float delay;

        [Tooltip("Type of weapon sound to play.")]
        [SerializeField]
        private SoundType soundType;


        [Tooltip("Audio Settings.")]
        [SerializeField]
        private AudioSettings audioSettings = new AudioSettings(1.0f, 0.0f, true);

        private AnimationParameters animParams;
        private  Inventory inventory;
        private IAudioManagerService audioManagerService;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //We need to get the character component.
            animParams = FindObjectOfType<AnimationParameters>();

            //Get Inventory.
            inventory = animParams.GetInventory();

            //Try to get the equipped weapon's Weapon component.
            if (!(inventory.GetEquipped() is { } weaponBehaviour))
                return;

            //Try grab a reference to the sound managing service.

            //Switch.
            AudioClip clip = soundType switch
            {
                //Grenade Throw.
                //SoundType.GrenadeThrow => animParams.GetAudioClipsGrenadeThrow().GetRandom(),
                //Melee.
                //SoundType.Melee => animParams.GetAudioClipsMelee().GetRandom(),

                //Holster.
                SoundType.Holster => weaponBehaviour.GetAudioClipHolster(),
                //Unholster.
                SoundType.Unholster => weaponBehaviour.GetAudioClipUnholster(),

                //Reload.
                SoundType.Reload => weaponBehaviour.GetAudioClipReload(),
                //Reload Empty.
                SoundType.ReloadEmpty => weaponBehaviour.GetAudioClipReloadEmpty(),

                //Reload Open.
                SoundType.ReloadOpen => weaponBehaviour.GetAudioClipReloadOpen(),
                //Reload Insert.
                SoundType.ReloadInsert => weaponBehaviour.GetAudioClipReloadInsert(),
                //Reload Close.
                SoundType.ReloadClose => weaponBehaviour.GetAudioClipReloadClose(),

                //Fire.
                SoundType.Fire => weaponBehaviour.GetAudioClipFire(),
                //Fire Empty.
                SoundType.FireEmpty => weaponBehaviour.GetAudioClipFireEmpty(),

                //Bolt Action.
                SoundType.BoltAction => weaponBehaviour.GetAudioClipBoltAction(),

                //Default.
                _ => default
            };


            AudioManagerServices.instance.PlayOneShotDelayed(clip, audioSettings, delay);
            //Play with some delay. Granted, if the delay is set to zero, this will just straight-up play!
            //audioManagerService. PlayOneShotDelayed(clip, audioSettings, delay);
        }
    }
}
