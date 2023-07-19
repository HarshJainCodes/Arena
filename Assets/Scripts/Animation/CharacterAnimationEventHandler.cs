using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class CharacterAnimationEventHandler : MonoBehaviour
    {
        [SerializeField]
        private AnimationParameters animParams;

        private void OnEjectCasing()
        {
            //Notify the character.
            if (animParams != null)
                animParams.EjectCasing();
        }

        /// <summary>
        /// Fills the character's equipped weapon's ammunition by a certain amount, or fully if set to 0. This function is called
        /// from a Animation Event.
        /// </summary>
        private void OnAmmunitionFill(int amount = 0)
        {
            //Notify the character.
            if (animParams != null)
                animParams.FillAmmunition(amount);
        }
        /// <summary>
        /// Sets the character's knife active value. This function is called from an Animation Event.
        /// </summary>
        /*private void OnSetActiveKnife(int active)
        {
            //Notify the character.
            if (animParams != null)
                animParams.SetActiveKnife(active);
        }*/

        /// <summary>
        /// Spawns a grenade at the correct location. This function is called from an Animation Event.
        /// </summary>
        /*private void OnGrenade()
        {
            //Notify the character.
            if (animParams != null)
                animParams.Grenade();
        }*/
        /// <summary>
        /// Sets the equipped weapon's magazine to be active or inactive! This function is called from an Animation Event.
        /// </summary>
        private void OnSetActiveMagazine(int active)
        {
            //Notify the character.
            if (animParams != null)
                animParams.SetActiveMagazine(active);
        }

        /// <summary>
        /// Bolt Animation Ended. This function is called from an Animation Event.
        /// </summary>
        private void OnAnimationEndedBolt()
        {
            //Notify the character.
            if (animParams != null)
                animParams.AnimationEndedBolt();
        }
        /// <summary>
        /// Reload Animation Ended. This function is called from an Animation Event.
        /// </summary>
        private void OnAnimationEndedReload()
        {
            //Notify the character.
            if (animParams != null)
                animParams.AnimationEndedReload();
        }

        /// <summary>
        /// Grenade Throw Animation Ended. This function is called from an Animation Event.
        /// </summary>
        private void OnAnimationEndedGrenadeThrow()
        {
            //Notify the character.
            if (animParams != null)
                animParams.AnimationEndedGrenadeThrow();
        }
        /// <summary>
        /// Melee Animation Ended. This function is called from an Animation Event.
        /// </summary>
        private void OnAnimationEndedMelee()
        {
            //Notify the character.
            if (animParams != null)
                animParams.AnimationEndedMelee();
        }

        /// <summary>
        /// Inspect Animation Ended. This function is called from an Animation Event.
        /// </summary>
        private void OnAnimationEndedInspect()
        {
            //Notify the character.
            if (animParams != null)
                animParams.AnimationEndedInspect();
        }
        /// <summary>
        /// Holster Animation Ended. This function is called from an Animation Event.
        /// </summary>
        private void OnAnimationEndedHolster()
        {
            //Notify the character.
            if (animParams != null)
                animParams.AnimationEndedHolster();
        }

        /// <summary>
        /// Sets the character's equipped weapon's slide back pose. This function is called from an Animation Event.
        /// </summary>
        private void OnSlideBack(int back)
        {
            //Notify the character.
            if (animParams != null)
                animParams.SetSlideBack(back);
        }

        private void OnPlayReloadOpen()
        {
            if(animParams!=null)
            {
                AudioManagerServices.instance.PlayOneShot(
                animParams.GetInventory().GetEquipped().GetAudioClipReloadOpen(), new AudioSettings(1, 0, true)
                    );
            }
        }

        private void OnPlayReloadClose()
        {
            if (animParams != null)
            {
                AudioManagerServices.instance.PlayOneShot(
                animParams.GetInventory().GetEquipped().GetAudioClipReloadClose(), new AudioSettings(1, 0, true)
                    );
            }
        }
    }
}

