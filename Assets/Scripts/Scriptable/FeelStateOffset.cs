﻿//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;


    /// <summary>
    /// FeelStateOffset. Contains all information needed to offset something properly, and is used in FeelStates for
    /// that exact purpose!
    /// </summary>
    [CreateAssetMenu(fileName = "SO_FSO_Default", menuName = "Infima Games/Low Poly Shooter Pack/Feel State Offset",
        order = 0)]
    public class FeelStateOffset : ScriptableObject
    {
        #region PROPERTIES

        /// <summary>
        /// OffsetLocation.
        /// </summary>
        public Vector3 OffsetLocation => offsetLocation;
        /// <summary>
        /// SpringSettingsLocation.
        /// </summary>
        public SpringSettings SpringSettingsLocation => springSettingsLocation;

        /// <summary>
        /// OffsetRotation.
        /// </summary>
        public Vector3 OffsetRotation => offsetRotation;
        /// <summary>
        /// SpringSettingsRotation.
        /// </summary>
        public SpringSettings SpringSettingsRotation => springSettingsRotation;
        
        #endregion
        
        #region FIELDS SERIALIZED
        
        
        [Tooltip("The location offset.")]
        [SerializeField]
        public Vector3 offsetLocation;
        
        [Tooltip("Spring settings relating to interpolating the location.")]
        [SerializeField]
        public SpringSettings springSettingsLocation = SpringSettings.Default();

        
        [Tooltip("The rotation offset.")]
        [SerializeField]
        public Vector3 offsetRotation;

        [Tooltip("Spring settings relating to interpolating the rotation.")]
        [SerializeField]
        public SpringSettings springSettingsRotation = SpringSettings.Default();
        
        #endregion
    }
