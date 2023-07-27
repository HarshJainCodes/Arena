using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena{
    public class WeaponAttachmentManager : MonoBehaviour
    {
        [Tooltip("Determines if the ironsights should be shown on the weapon model.")]
        [SerializeField]
        private bool scopeDefaultShow = true;

        [Tooltip("Default Scope!")]
        [SerializeField]
        private Scope scopeDefaultBehaviour;

        [Tooltip("Selected Scope Index. If you set this to a negative number, ironsights will be selected as the enabled scope.")]
        [SerializeField]
        private int scopeIndex = -1;

        [Tooltip("First scope index when using random scopes.")]
        [SerializeField]
        private int scopeIndexFirst = -1;

        [Tooltip("Should we pick a random index when starting the game?")]
        [SerializeField]
        private bool scopeIndexRandom;

        [Tooltip("All possible Scope Attachments that this Weapon can use!")]
        [SerializeField]
        private Scope[] scopeArray;




        [Tooltip("Selected Muzzle Index.")]
        [SerializeField]
        private int muzzleIndex;

        [Tooltip("Should we pick a random index when starting the game?")]
        [SerializeField]
        private bool muzzleIndexRandom = true;

        [Tooltip("All possible Muzzle Attachments that this Weapon can use!")]
        [SerializeField]
        private Muzzle[] muzzleArray;


        [Tooltip("Selected Laser Index.")]
        [SerializeField]
        private int laserIndex = -1;

        [Tooltip("Should we pick a random index when starting the game?")]
        [SerializeField]
        private bool laserIndexRandom = true;

        [Tooltip("All possible Laser Attachments that this Weapon can use!")]
        [SerializeField]
        private Laser[] laserArray;



        [Tooltip("Selected Grip Index.")]
        [SerializeField]
        private int gripIndex = -1;

        [Tooltip("Should we pick a random index when starting the game?")]
        [SerializeField]
        private bool gripIndexRandom = true;

        [Tooltip("All possible Grip Attachments that this Weapon can use!")]
        [SerializeField]
        private Grip[] gripArray;


        [Tooltip("Selected Magazine Index.")]
        [SerializeField]
        private int magazineIndex;

        [Tooltip("Should we pick a random index when starting the game?")]
        [SerializeField]
        private bool magazineIndexRandom = true;

        [Tooltip("All possible Magazine Attachments that this Weapon can use!")]
        [SerializeField]
        private Magzine[] magazineArray;



        private Scope scopeBehaviour;
        /// <summary>
        /// Equipped Muzzle.
        /// </summary>
        private Muzzle muzzleBehaviour;
        /// <summary>
        /// Equipped Laser.
        /// </summary>
        private Laser laserBehaviour;
        /// <summary>
        /// Equipped Grip.
        /// </summary>
        private Grip gripBehaviour;
        /// <summary>
        /// Equipped Magazine.
        /// </summary>
        private Magzine magazineBehaviour;


        protected void Awake()
        {
            //Randomize. This allows us to spice things up a little!
            if (scopeIndexRandom)
                scopeIndex = Random.Range(scopeIndexFirst, scopeArray.Length);
            //Select Scope!
            scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);
            //Check if we have no scope. This could happen if we have an incorrect index.
            if (scopeBehaviour == null)
            {
                //Select Default Scope.
                scopeBehaviour = scopeDefaultBehaviour;
                //Set Active.
                scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
            }

            //Randomize. This allows us to spice things up a little!
            if (muzzleIndexRandom)
                muzzleIndex = Random.Range(0, muzzleArray.Length);
            //Select Muzzle!
            muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);

            //Randomize. This allows us to spice things up a little!
            if (laserIndexRandom)
                laserIndex = Random.Range(0, laserArray.Length);
            //Select Laser!
            laserBehaviour = laserArray.SelectAndSetActive(laserIndex);

            //Randomize. This allows us to spice things up a little!
            if (gripIndexRandom)
                gripIndex = Random.Range(0, gripArray.Length);
            //Select Grip!
            gripBehaviour = gripArray.SelectAndSetActive(gripIndex);

            //Randomize. This allows us to spice things up a little!
            if (magazineIndexRandom)
                magazineIndex = Random.Range(0, magazineArray.Length);
            //Select Magazine!
            magazineBehaviour = magazineArray.SelectAndSetActive(magazineIndex);
        }

        #region GETTER FUNCTIONS
        public Scope GetEquippedScope() => scopeBehaviour;
        public Scope GetEquippedScopeDefault() => scopeDefaultBehaviour;

        public Magzine GetEquippedMagazine() => magazineBehaviour;
        public Muzzle GetEquippedMuzzle() => muzzleBehaviour;

        public Laser GetEquippedLaser() => laserBehaviour;
        public Grip GetEquippedGrip() => gripBehaviour;
        #endregion
    }
}

