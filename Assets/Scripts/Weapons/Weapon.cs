using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class Weapon : MonoBehaviour
    {
        [Tooltip("Weapon Name. Currently not used for anything, but in the future, we will use this for pickups!")]
        [SerializeField]
        private string weaponName;

        [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
        [SerializeField]
        private bool automatic;

        [Tooltip("Is this weapon bolt-action? If yes, then a bolt-action animation will play after every shot.")]
        [SerializeField]
        private bool boltAction;

        [Tooltip("Amount of shots fired at once. Helpful for things like shotguns, where there are multiple projectiles fired at once.")]
        [SerializeField]
        private int shotCount = 1;

        [Tooltip("How far the weapon can fire from the center of the screen.")]
        [SerializeField]
        private float spread = 0.25f;

        [Tooltip("How fast the projectiles are.")]
        [SerializeField]
        private float projectileImpulse = 400.0f;

        [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
        [SerializeField]
        private int roundsPerMinutes = 200;

        [Tooltip("Determines if this weapon reloads in cycles, meaning that it inserts one bullet at a time, or not.")]
        [SerializeField]
        private bool cycledReload;

        [Tooltip("Determines if the player can reload this weapon when it is full of ammunition.")]
        [SerializeField]
        private bool canReloadWhenFull = true;

        [Tooltip("Should this weapon be reloaded automatically after firing its last shot?")]
        [SerializeField]
        private bool automaticReloadOnEmpty;

        [Tooltip("Time after the last shot at which a reload will automatically start.")]
        [SerializeField]
        private float automaticReloadOnEmptyDelay = 0.25f;

        [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
        [SerializeField]
        private Transform socketEjection;

        [Tooltip("Settings this to false will stop the weapon from being reloaded while the character is aiming it.")]
        [SerializeField]
        private bool canReloadAimed = true;

        [Tooltip("Casing Prefab.")]
        [SerializeField]
        private GameObject prefabCasing;

        [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")]
        [SerializeField]
        private GameObject prefabProjectile;

        [Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")]
        [SerializeField]
        public RuntimeAnimatorController controller;

        [SerializeField]
        private AnimationParameters playerAnim;

        [SerializeField]
        private Transform playerCam;

        [Tooltip("Holster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipHolster;

        [Tooltip("Unholster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipUnholster;


        [Tooltip("Reload Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReload;

        [Tooltip("Reload Empty Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadEmpty;


        [Tooltip("Reload Open Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadOpen;

        [Tooltip("Reload Insert Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadInsert;

        [Tooltip("Reload Close Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadClose;

        


        [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
        [SerializeField]
        private AudioClip audioClipFireEmpty;

        [Tooltip("")]
        [SerializeField]
        private AudioClip audioClipBoltAction;

        private Animator animator;

        private int ammunitionCurrent;

        private WeaponAttachmentManager attachmentManager;
        private Scope scopeBehaviour;

        /// <summary>
        /// Equipped Magazine Reference.
        /// </summary>
        private Magzine magazineBehaviour;
        /// <summary>
        /// Equipped Muzzle Reference.
        /// </summary>
        private Muzzle muzzleBehaviour;

        /// <summary>
        /// Equipped Laser Reference.
        /// </summary>
        private Laser laserBehaviour;
        /// <summary>
        /// Equipped Grip Reference.
        /// </summary>
        private Grip gripBehaviour;

        [SerializeField]
        private int maxAmmo;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            attachmentManager = GetComponent<WeaponAttachmentManager>();
            scopeBehaviour = attachmentManager.GetEquippedScope();
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();

            //Get Laser.
            laserBehaviour = attachmentManager.GetEquippedLaser();
            //Get Grip.
            gripBehaviour = attachmentManager.GetEquippedGrip();

        }
        // Start is called before the first frame update
        void Start()
        {
            ammunitionCurrent = maxAmmo;


        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Reload()
        {
            const string boolName = "Reloading";
            animator.SetBool(boolName, true);
            animator.Play(cycledReload ? "Reload Open" : (HasAmmunition() ? "Reload" : "Reload Empty"), 0, 0.0f);
           // AudioManagerServices.instance.PlayOneShot(HasAmmunition() ? audioClipReload : audioClipReloadEmpty, new AudioSettings(1.0f, 0.0f, false));
            AudioManager.instance?.PlayOneShot(HasAmmunition() ? FMODEvents.instance.audioClipReload : FMODEvents.instance.audioClipReloadEmpty, this.transform.position);
            Debug.Log("Reloading");
        }
        /// <summary>
        /// Performs fire action, reduces bullet count, plays animation, and muzzle flash effect
        /// </summary>
        /// <param name="spreadMultiplier"></param>
        public void Fire(float spreadMultiplier = 1f)
        {
            if (muzzleBehaviour == null)
                return;
            if (playerCam == null)
                return;
            if (!playerCam.gameObject.activeInHierarchy)
                return;
            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);
            //Reduce ammunition! We just shot, so we need to get rid of one!
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, maxAmmo);
            
            //Set the slide back if we just ran out of ammunition.
            if (ammunitionCurrent == 0)
                SetSlideBack(1);

            muzzleBehaviour.Effect();
            for (var i = 0; i < shotCount; i++)
            {
                //Determine a random spread value using all of our multipliers.
                Vector3 spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                //Remove the forward spread component, since locally this would go inside the object we're shooting!
                spreadValue.z = 0;
                //Convert to world space.
                spreadValue = playerCam.TransformDirection(spreadValue);

                //Spawn projectile from the projectile spawn point.
                GameObject projectile = Instantiate(prefabProjectile, playerCam.position, Quaternion.Euler(playerCam.eulerAngles + spreadValue));
                //Add velocity to the projectile.
                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
            }
        }
        /// <summary>
        /// Refills the ammunition count
        /// </summary>
        /// <param name="amount"></param>
        public void FillAmmunition(int amount)
        {
            ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount, 0, GetAmmunitionMax()) : maxAmmo;
        }
        public void SetSlideBack(int back)
        {
            //Set the slide back bool.
            const string boolName = "Slide Back";
            animator.SetBool(boolName, back != 0);
        }
        /// <summary>
        /// Aniamtion event to spawn a bullet casing prefab
        /// </summary>
        public void EjectCasing()
        {
            //Spawn casing prefab at spawn point.
            if (prefabCasing != null && socketEjection != null)
                Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
        }
        /// <summary>
        /// Gets FOV multiplier while AIMING for different scopes
        /// </summary>
        /// <returns></returns>
        public float GetFieldOfViewMultiplierAim()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null)
                return scopeBehaviour.GetFieldOfViewMultiplierAim();

            //Error.
            Debug.LogError("Weapon has no scope equipped!");

            //Return.
            return 1.0f;
        }
        /// <summary>
        /// Gets FOV multiplier while NOT AIMING 
        /// </summary>
        /// <returns></returns>
        public float GetFieldOfViewMultiplierAimWeapon()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null)
                return scopeBehaviour.GetFieldOfViewMultiplierAimWeapon();

            //Error.
            Debug.LogError("Weapon has no scope equipped!");

            //Return.
            return 1.0f;
        }

        #region GETTER FUNCTIONS
        public Animator GetAnimator() => animator;
        public bool CanReloadAimed() => canReloadAimed;
        public int GetAmmunitionCurrent() => ammunitionCurrent;
        public int GetAmmunitionMax() => maxAmmo;
        public bool HasCycledReload() => cycledReload;
        public bool IsAutomatic() => automatic;
        public bool IsBoltAction() => boltAction;
        public bool GetAutomaticallyReloadOnEmpty() => automaticReloadOnEmpty;
        public float GetAutomaticallyReloadOnEmptyDelay() => automaticReloadOnEmptyDelay;
        public bool CanReloadWhenFull() => canReloadWhenFull;
        public float GetRateOfFire() => roundsPerMinutes;
        public bool IsFull() => ammunitionCurrent == maxAmmo;
        public bool HasAmmunition() => ammunitionCurrent > 0;
        public RuntimeAnimatorController GetAnimatorController() => controller;
        public WeaponAttachmentManager GetAttachmentManager() => attachmentManager;
        public  AudioClip GetAudioClipHolster() => audioClipHolster;
        public  AudioClip GetAudioClipUnholster() => audioClipUnholster;
        public  AudioClip GetAudioClipReload() => audioClipReload;
        public  AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;
        public  AudioClip GetAudioClipReloadOpen() => audioClipReloadOpen;
        public  AudioClip GetAudioClipReloadInsert() => audioClipReloadInsert;
        public  AudioClip GetAudioClipReloadClose() => audioClipReloadClose;
        public  AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
        public  AudioClip GetAudioClipBoltAction() => audioClipBoltAction;
        public  AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();
        #endregion
    }
}


