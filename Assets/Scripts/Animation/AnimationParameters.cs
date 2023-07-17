using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arena
{
    public class AnimationParameters : MonoBehaviour
    {
        [SerializeField]
        private int weaponIndexEquippedAtStart;

        [Tooltip("Inventory.")]
        [SerializeField]
        private Inventory inventory;

        [Tooltip("If true, the character's grenades will never run out.")]
        [SerializeField]
        private bool grenadesUnlimited;

        [Tooltip("Total amount of grenades at start.")]
        [SerializeField]
        private int grenadeTotal = 10;

        [Tooltip("Grenade spawn offset from the character's camera.")]
        [SerializeField]
        private float grenadeSpawnOffset = 1.0f;

        [Tooltip("Grenade Prefab. Spawned when throwing a grenade.")]
        [SerializeField]
        private GameObject grenadePrefab;




        [Tooltip("Normal Camera.")]
        [SerializeField]
        private Camera cameraWorld;

        [Tooltip("Weapon-Only Camera. Depth.")]
        [SerializeField]
        private Camera cameraDepth;




        [Tooltip("Determines how smooth the turning animation is.")]
        [SerializeField]
        private float dampTimeTurning = 0.4f;

        [Tooltip("Determines how smooth the locomotion blendspace is.")]
        [SerializeField]
        private float dampTimeLocomotion = 0.15f;

        [Tooltip("How smoothly we play aiming transitions. Beware that this affects lots of things!")]
        [SerializeField]
        private float dampTimeAiming = 0.3f;

        [Tooltip("Interpolation speed for the running offsets.")]
        [SerializeField]
        private float runningInterpolationSpeed = 12.0f;

        [Tooltip("Determines how fast the character's weapons are aimed.")]
        [SerializeField]
        private float aimingSpeedMultiplier = 1.0f;

        [Tooltip("Character Animator.")]
        [SerializeField]
        private Animator characterAnimator;


        [Tooltip("Normal world field of view.")]
        [SerializeField]
        private float fieldOfView = 100.0f;

        [Tooltip("Multiplier for the field of view while running.")]
        [SerializeField]
        private float fieldOfViewRunningMultiplier = 1.05f;

        [Tooltip("Weapon-specific field of view.")]
        [SerializeField]
        private float fieldOfViewWeapon = 55.0f;


        [Tooltip("If true, the running input has to be held to be active.")]
        [SerializeField]
        private bool holdToRun = true;

        [Tooltip("If true, the aiming input has to be held to be active.")]
        [SerializeField]
        private bool holdToAim = true;


        private bool aiming;
        private bool wasAiming;
        private bool running;
        private bool holstered = false;
        private float lastShotTime;
        private int layerOverlay;
        private int layerHolster;
        private int layerActions;
        private bool reloading;
        private bool inspecting;
        private bool throwingGrenade;
        private bool meleeing;
        private bool holstering;
        private float aimingAlpha;
        private float crouchingAlpha;
        private float runningAlpha;

        private Vector2 axisLook;
        private Vector2 axisMovement;

        private bool bolting;
        private int grenadeCount;
        private bool holdingButtonAim;
        private bool holdingButtonRun;
        private bool holdingButtonFire;
        private int shotsFired;

        private PlayerMovement pm;
        private Weapon equippedWeapon;
        private WeaponAttachmentManager weaponAttachmentManager;
        private Scope equippedScope;
        private Magzine equippedMagzine;

        //private float landTime;
        private void Awake()
        {
            pm = GetComponent<PlayerMovement>();
            inventory.Init(weaponIndexEquippedAtStart);
            RefreshWeaponSetup();
        }
        // Start is called before the first frame update
        void Start()
        {
            grenadeCount = grenadeTotal;

            layerHolster = characterAnimator.GetLayerIndex("Layer Holster");
            layerActions = characterAnimator.GetLayerIndex("Layer Actions");
            layerOverlay = characterAnimator.GetLayerIndex("Layer Overlay");

        }

        // Update is called once per frame
        void Update()
        {
            aiming = holdingButtonAim && CanAim();
            running = holdingButtonRun && CanRun() && !pm.isWallRunning;
            switch (aiming)
            {
                //Just Started.
                case true when !wasAiming:
                    equippedScope.OnAim();
                    break;
                //Just Stopped.
                case false when wasAiming:
                    equippedScope.OnAimStop();
                    break;
            }
            if (holdingButtonFire)
            {
                if (CanPlayAnimationFire() && equippedWeapon.HasAmmunition() && equippedWeapon.IsAutomatic())
                {
                    if (Time.time - lastShotTime > 60f / equippedWeapon.GetRateOfFire())
                        Fire();
                }
                else
                {
                    shotsFired = 0;
                }
            }
            UpdateAnimator();
            aimingAlpha = characterAnimator.GetFloat(AHashes.AimingAlpha);
            crouchingAlpha = Mathf.Lerp(crouchingAlpha, pm.IsCrouching() ? 1.0f : 0.0f, Time.deltaTime * 12.0f);
            runningAlpha = Mathf.Lerp(runningAlpha, running ? 1.0f : 0.0f, Time.deltaTime * runningInterpolationSpeed);
            float runningFieldOfView = Mathf.Lerp(1.0f, fieldOfViewRunningMultiplier, runningAlpha);
            cameraWorld.fieldOfView = Mathf.Lerp(fieldOfView, fieldOfView * equippedWeapon.GetFieldOfViewMultiplierAim(), aimingAlpha) * runningFieldOfView;
            //Interpolate the depth camera's field of view based on whether we are aiming or not.
            cameraDepth.fieldOfView = Mathf.Lerp(fieldOfViewWeapon, fieldOfViewWeapon * equippedWeapon.GetFieldOfViewMultiplierAimWeapon(), aimingAlpha);

            wasAiming = aiming;

        }
        public int GetShotsFired() => shotsFired;
        public Camera GetCameraWorld() => cameraWorld;
        /// <summary>
        /// GetCameraDepth.
        /// </summary>
        /// <returns></returns>
        public Camera GetCameraDepth() => cameraDepth;
        public Inventory GetInventory() => inventory;
        public int GetGrenadesCurrent() => grenadeCount;
        public int GetGrenadesTotal() => grenadeTotal;
        public bool IsRunning() => running;
        public bool IsHolstered() => holstered;
        public bool IsCrouching() => pm.IsCrouching();
        public bool IsReloading() => reloading;
        public bool IsThrowingGrenade() => throwingGrenade;
        public bool IsMeleeing() => meleeing;
        public bool IsAiming() => aiming;
        public Vector2 GetInputMovement() => axisMovement;
        public Vector2 GetInputLook() => axisLook;
        public bool IsInspecting() => inspecting;
        public bool IsHoldingButtonFire() => holdingButtonFire;


        private void UpdateAnimator()
        {
            #region Reload Stop

            //Check if we're currently reloading cycled.
            const string boolNameReloading = "Reloading";
            if (characterAnimator.GetBool(boolNameReloading))
            {
                //If we only have one more bullet to reload, then we can change the boolean already.
                if (equippedWeapon.GetAmmunitionMax() - equippedWeapon.GetAmmunitionCurrent() < 1)
                {
                    //Update the character animator.
                    characterAnimator.SetBool(boolNameReloading, false);
                    //Update the weapon animator.
                    equippedWeapon.GetAnimator().SetBool(boolNameReloading, false);
                }
            }

            #endregion

            //Leaning. Affects how much the character should apply of the leaning additive animation.
            float leaningValue = Mathf.Clamp01(axisMovement.y);
            characterAnimator.SetFloat(AHashes.LeaningForward, leaningValue, 0.5f, Time.deltaTime);

            //Movement Value. This value affects absolute movement. Aiming movement uses this, as opposed to per-axis movement.
            float movementValue = Mathf.Clamp01(Mathf.Abs(axisMovement.x) + Mathf.Abs(axisMovement.y));
            characterAnimator.SetFloat(AHashes.Movement, movementValue, dampTimeLocomotion, Time.deltaTime);

            //Aiming Speed Multiplier.
            characterAnimator.SetFloat(AHashes.AimingSpeedMultiplier, aimingSpeedMultiplier);

            //Turning Value. This determines how much of the turning animation to play based on our current look rotation.
            characterAnimator.SetFloat(AHashes.Turning, Mathf.Abs(axisLook.x), dampTimeTurning, Time.deltaTime);

            //Horizontal Movement Float.
            characterAnimator.SetFloat(AHashes.Horizontal, axisMovement.x, dampTimeLocomotion, Time.deltaTime);
            //Vertical Movement Float.
            characterAnimator.SetFloat(AHashes.Vertical, axisMovement.y, dampTimeLocomotion, Time.deltaTime);

            //Update the aiming value, but use interpolation. This makes sure that things like firing can transition properly.
            characterAnimator.SetFloat(AHashes.AimingAlpha, Convert.ToSingle(aiming), dampTimeAiming, Time.deltaTime);

            //Set the locomotion play rate. This basically stops movement from happening while in the air.
            const string playRateLocomotionBool = "Play Rate Locomotion";
            characterAnimator.SetFloat(playRateLocomotionBool, pm.IsPlayerGrounded() ? 1.0f : 0.0f, 0.2f, Time.deltaTime);

            #region Movement Play Rates

            //Update Forward Multiplier. This allows us to change the play rate of our animations based on our movement multipliers.
            characterAnimator.SetFloat(AHashes.PlayRateLocomotionForward, pm.GetMultiplierForward(), 0.2f, Time.deltaTime);
            //Update Sideways Multiplier. This allows us to change the play rate of our animations based on our movement multipliers.
            characterAnimator.SetFloat(AHashes.PlayRateLocomotionSideways, pm.GetMultiplierSideways(), 0.2f, Time.deltaTime);
            //Update Backwards Multiplier. This allows us to change the play rate of our animations based on our movement multipliers.
            characterAnimator.SetFloat(AHashes.PlayRateLocomotionBackwards, pm.GetMultiplierBackwards(), 0.2f, Time.deltaTime);

            #endregion

            //Update Animator Aiming.
            characterAnimator.SetBool(AHashes.Aim, aiming);
            //Update Animator Running.
            characterAnimator.SetBool(AHashes.Running, running);
            //Update Animator Crouching.
            characterAnimator.SetBool(AHashes.Crouching, pm.IsCrouching());
        }
        private void Inspect()
        {
            //State.
            inspecting = true;
            //Play.
            characterAnimator.CrossFade("Inspect", 0.0f, layerActions, 0);
        }
        private void Fire()
        {
            //Increase shots fired. We use this value to increase the spread, and also to apply recoil, so
            //it is very important that we keep it up to date.
            shotsFired++;

            //Save the shot time, so we can calculate the fire rate correctly.
            lastShotTime = Time.time;
            //Fire the weapon! Make sure that we also pass the scope's spread multiplier if we're aiming.
            equippedWeapon.Fire(aiming ? equippedScope.GetMultiplierSpread() : 1.0f);

            //Play firing animation.
            const string stateName = "Fire";
            characterAnimator.CrossFade(stateName, 0.05f, layerOverlay, 0);

            //Play bolt actioning animation if needed, and if we have ammunition. We don't play this for the last shot.
            if (equippedWeapon.IsBoltAction() && equippedWeapon.HasAmmunition())
                UpdateBolt(true);

            //Automatically reload the weapon if we need to. This is very helpful for things like grenade launchers or rocket launchers.
            if (!equippedWeapon.HasAmmunition() && equippedWeapon.GetAutomaticallyReloadOnEmpty())
                StartCoroutine(nameof(TryReloadAutomatic));
        }
        private void PlayReloadAnimation()
        {
            #region Animation

            //Get the name of the animation state to play, which depends on weapon settings, and ammunition!
            string stateName = equippedWeapon.HasCycledReload() ? "Reload Open" :
                (equippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty");

            //Play the animation state!
            characterAnimator.Play(stateName, layerActions, 0.0f);

            #endregion

            //Set Reloading Bool. This helps cycled reloads know when they need to stop cycling.
            characterAnimator.SetBool(AHashes.Reloading, reloading = true);

            //Reload.
            equippedWeapon.Reload();
        }
        private IEnumerator TryReloadAutomatic()
        {
            //Yield.
            yield return new WaitForSeconds(equippedWeapon.GetAutomaticallyReloadOnEmptyDelay());

            //Play Reload Animation.
            PlayReloadAnimation();
        }
        private IEnumerator Equip(int index = 0)
        {
            //Only if we're not holstered, holster. If we are already, we don't need to wait.
            if (!holstered)
            {
                //Holster.
                SetHolstered(holstering = true);
                //Wait.
                yield return new WaitUntil(() => holstering == false);
            }
            //Unholster. We do this just in case we were holstered.
            SetHolstered(false);
            //Play Unholster Animation.
            characterAnimator.Play("Unholster", layerHolster, 0);

            //Equip The New Weapon.
            inventory.Equip(index);
            //Refresh.
            RefreshWeaponSetup();
        }
        private void RefreshWeaponSetup()
        {
            //Make sure we have a weapon. We don't want errors!
            if ((equippedWeapon = inventory.GetEquipped()) == null)
                return;

            //Update Animator Controller. We do this to update all animations to a specific weapon's set.
            characterAnimator.runtimeAnimatorController = equippedWeapon.GetAnimatorController();
            weaponAttachmentManager = equippedWeapon.GetAttachmentManager();
            if (weaponAttachmentManager == null)
                return;

            //Get equipped scope. We need this one for its settings!
            equippedScope = weaponAttachmentManager.GetEquippedScope();
            //Get equipped magazine. We need this one for its settings!
            equippedMagzine = weaponAttachmentManager.GetEquippedMagazine();

        }
        private void FireEmpty()
        {
            /*
             * Save Time. Even though we're not actually firing, we still need this for the fire rate between
             * empty shots.
             */
            lastShotTime = Time.time;
            //Play.
            characterAnimator.CrossFade("Fire Empty", 0.05f, layerOverlay, 0);
        }
        private void PlayGrenadeThrow()
        {
            //Start State.
            throwingGrenade = true;

            //Play Normal.
            characterAnimator.CrossFade("Grenade Throw", 0.15f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);

            //Play Additive.
            characterAnimator.CrossFade("Grenade Throw", 0.05f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);
        }
        private void PlayMelee()
        {
            //Start State.
            meleeing = true;

            //Play Normal.
            characterAnimator.CrossFade("Knife Attack", 0.05f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);

            //Play Additive.
            characterAnimator.CrossFade("Knife Attack", 0.05f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);
        }
        private void UpdateBolt(bool value)
        {
            //Update.
            characterAnimator.SetBool(AHashes.Bolt, bolting = value);
        }
        private void SetHolstered(bool value = true)
        {
            //Update value.
            holstered = value;
            Debug.Log("Holst cllled in " + value);

            //Update Animator.
            const string boolName = "Holstered";
            characterAnimator.SetBool(boolName, holstered);
        }

        private bool CanPlayAnimationFire()
        {
            //Block.
            if (holstered || holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }
        private bool CanPlayAnimationReload()
        {
            //No reloading!
            if (reloading)
                return false;

            //No meleeing!
            if (meleeing)
                return false;

            //Not actioning a bolt.
            if (bolting)
                return false;

            //Can't reload while throwing a grenade.
            if (throwingGrenade)
                return false;

            //Block while inspecting.
            if (inspecting)
                return false;

            //Block Full Reloading if needed.
            if (!equippedWeapon.CanReloadWhenFull() && equippedWeapon.IsFull())
                return false;

            //Return.
            return true;
        }
        private bool CanPlayAnimationGrenadeThrow()
        {
            //Block.
            if (holstered || holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //We need to have grenades!
            if (!grenadesUnlimited && grenadeCount == 0)
                return false;

            //Return.
            return true;
        }
        private bool CanPlayAnimationMelee()
        {
            //Block.
            if (holstered || holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }
        private bool CanPlayAnimationHolster()
        {
            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }
        private bool CanChangeWeapon()
        {
            //Block.
            if (holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }
        private bool CanPlayAnimationInspect()
        {
            //Block.
            if (holstered || holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }
        private bool CanRun()
        {
            if (inspecting || bolting)
                return false;

            //No running while crouching.
            if (pm.IsCrouching())
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || aiming)
                return false;

            //While trying to fire, we don't want to run. We do this just in case we do fire.
            if (holdingButtonFire)
                return false;

            //This blocks running backwards, or while fully moving sideways.
            if (axisMovement.y <= 0 || Math.Abs(Mathf.Abs(axisMovement.x) - 1) < 0.01f)
                return false;

            //Return.
            return true;
        }

        private bool CanAim()
        {
            if (holstered || inspecting)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (holstering)
                return false;

            //Return.
            return true;
        }






        public void OnTryFire(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.


            //Switch.
            switch (context)
            {
                //Started.
                case { phase: InputActionPhase.Started }:
                    //Hold.
                    holdingButtonFire = true;

                    //Restart the shots.
                    shotsFired = 0;
                    break;
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Ignore if we're not allowed to actually fire.
                    if (!CanPlayAnimationFire())
                        break;

                    //Check.
                    if (equippedWeapon.HasAmmunition())
                    {
                        //Check.
                        if (equippedWeapon.IsAutomatic())
                        {
                            //Reset fired shots, so recoil/spread does not just stay at max when we've run out
                            //of ammo already!
                            shotsFired = 0;

                            //Break.
                            break;
                        }

                        //Has fire rate passed.
                        if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                            Fire();
                    }
                    //Fire Empty.
                    else
                        FireEmpty();
                    break;
                //Canceled.
                case { phase: InputActionPhase.Canceled }:
                    //Stop Hold.
                    holdingButtonFire = false;

                    //Reset shotsFired.
                    shotsFired = 0;
                    break;
            }


        }
        public void OnTryPlayReload(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.


            //Block.
            if (!CanPlayAnimationReload())
                return;

            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Play Animation.
                    PlayReloadAnimation();
                    break;
            }
        }

        public void OnTryInspect(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.


            //Block.
            if (!CanPlayAnimationInspect())
                return;

            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Play Animation.
                    Inspect();
                    break;
            }
        }

        public void OnTryAiming(InputAction.CallbackContext context)
        {


            //Switch.
            switch (context.phase)
            {
                //Started.
                case InputActionPhase.Started:
                    //Started.
                    if (holdToAim)
                        holdingButtonAim = true;
                    break;
                //Performed.
                case InputActionPhase.Performed:
                    //Performed.
                    if (!holdToAim)
                        holdingButtonAim = !holdingButtonAim;
                    break;
                //Canceled.
                case InputActionPhase.Canceled:
                    //Canceled.
                    if (holdToAim)
                        holdingButtonAim = false;
                    break;
            }
        }

        public void OnTryHolster(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.

            //Go back if we cannot even play the holster animation.
            if (!CanPlayAnimationHolster())
                return;
            Debug.Log("Holst called");

            //Switch.
            switch (context.phase)
            {
                //Started. This is here so we unholster with a tap, instead of a hold.
                case InputActionPhase.Started:
                    //Only if holstered.
                    if (holstered)
                    {
                        //Unholster.
                        Debug.Log("Holst cllled in true origin");

                        SetHolstered(false);
                        //Holstering.
                        holstering = true;
                    }
                    break;
                //Performed.
                case InputActionPhase.Performed:
                    //Set.
                    Debug.Log("Holst cllled in false origin");

                    SetHolstered(!holstered);
                    //Holstering.
                    holstering = true;
                    break;
            }
        }

        public void OnTryThrowGrenade(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.


            //Switch.
            switch (context.phase)
            {
                //Performed.
                case InputActionPhase.Performed:
                    //Try Play.
                    if (CanPlayAnimationGrenadeThrow())
                        PlayGrenadeThrow();
                    break;
            }
        }

        public void OnTryMelee(InputAction.CallbackContext context)
        {

            //Switch.
            switch (context.phase)
            {
                //Performed.
                case InputActionPhase.Performed:
                    //Try Play.
                    if (CanPlayAnimationMelee())
                        PlayMelee();
                    break;
            }
        }


        public void OnTryRun(InputAction.CallbackContext context)
        {


            //Switch.
            switch (context.phase)
            {
                //Performed.
                case InputActionPhase.Performed:
                    //Use this if we're using run toggle.
                    if (!holdToRun)
                        holdingButtonRun = !holdingButtonRun;
                    break;
                //Started.
                case InputActionPhase.Started:
                    //Start.
                    if (holdToRun)
                        holdingButtonRun = true;
                    break;
                //Canceled.
                case InputActionPhase.Canceled:
                    //Stop.
                    if (holdToRun)
                        holdingButtonRun = false;
                    break;
            }
        }

        public void OnTryJump(InputAction.CallbackContext context)
        {

            //Switch.
            switch (context.phase)
            {
                //Performed.
                case InputActionPhase.Performed:
                    //Jump.
                    pm.Jump();
                    break;
            }
        }

        public void OnTryInventoryNext(InputAction.CallbackContext context)
        {


            //Null Check.
            if (inventory == null)
                return;

            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Get the index increment direction for our inventory using the scroll wheel direction. If we're not
                    //actually using one, then just increment by one.
                    float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2)) ? Mathf.Sign(context.ReadValue<Vector2>().y) : 1.0f;

                    //Get the next index to switch to.
                    int indexNext = scrollValue > 0 ? inventory.GetNextIndex() : inventory.GetLastIndex();
                    //Get the current weapon's index.
                    int indexCurrent = inventory.GetEquippedIndex();

                    //Make sure we're allowed to change, and also that we're not using the same index, otherwise weird things happen!
                    if (CanChangeWeapon() && (indexCurrent != indexNext))
                        StartCoroutine(nameof(Equip), indexNext);
                    break;
            }
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            //Read.
            axisMovement = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            //Read.
            axisLook = context.ReadValue<Vector2>();

            //Make sure that we have a weapon.
            if (equippedWeapon == null)
                return;



            //If we're aiming, multiply by the mouse sensitivity multiplier of the equipped weapon's scope!
            axisLook *= 1.0f;
        }





        public void EjectCasing()
        {
            //Notify the weapon.
            if (equippedWeapon != null)
                equippedWeapon.EjectCasing();
        }
        public void FillAmmunition(int amount)
        {
            //Notify the weapon to fill the ammunition by the amount.
            if (equippedWeapon != null)
                equippedWeapon.FillAmmunition(amount);
        }
        public void AnimationEndedBolt()
        {
            //Update.
            UpdateBolt(false);
        }
        public void AnimationEndedReload()
        {
            //Stop reloading!
            reloading = false;
        }
        public void AnimationEndedGrenadeThrow()
        {
            //Stop Grenade Throw.
            throwingGrenade = false;
        }
        /// <summary>
        /// AnimationEndedMelee.
        /// </summary>
        public void AnimationEndedMelee()
        {
            //Stop Melee.
            meleeing = false;
        }

        /// <summary>
        /// AnimationEndedInspect.
        /// </summary>
        public void AnimationEndedInspect()
        {
            //Stop Inspecting.
            inspecting = false;
        }
        /// <summary>
        /// AnimationEndedHolster.
        /// </summary>
        public void AnimationEndedHolster()
        {
            //Stop Holstering.
            holstering = false;
        }

        /// <summary>
        /// SetSlideBack.
        /// </summary>
        public void SetSlideBack(int back)
        {
            //Set slide back.
            if (equippedWeapon != null)
                equippedWeapon.SetSlideBack(back);
        }
    }
}

