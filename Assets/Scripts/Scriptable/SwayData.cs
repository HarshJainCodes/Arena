﻿//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;


    /// <summary>
    /// ScriptableObject containing a location and rotation curve, along with settings to interpolate
    /// them using the Spring class.
    /// Very helpful for lots of procedural motions that use curves.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_SD_Default", menuName = "Infima Games/Low Poly Shooter Pack/Sway Data")]
    public class SwayData : ScriptableObject
    {
        #region PROPERTIES

        /// <summary>
        /// Look.
        /// </summary>
        public SwayType Look => look;

        /// <summary>
        /// Movement.
        /// </summary>
        public SwayType Movement => movement;

        /// <summary>
        /// SpringSettings.
        /// </summary>
        public SpringSettings SpringSettings => springSettings;
        
        #endregion
        
        #region FIELDS SERIALIZED

        
        [Tooltip("Look Sway.")]
        [SerializeField]
        private SwayType look;

        
        [Tooltip("Movement Sway.")]
        [SerializeField]
        private SwayType movement;
        
        
        [Tooltip("Spring Settings For Sway.")]
        [SerializeField]
        private SpringSettings springSettings = SpringSettings.Default();
        
        #endregion
    }
