using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimationData : MonoBehaviour
{

    [Tooltip("The object that contains all offset data for this item.")]
    [SerializeField]
    private ItemOffsets itemOffsets;


    [Tooltip("This object contains all the data needed for us to set the lowered pose of this weapon.")]
    [SerializeField]
    private LowerData lowerData;


    [Tooltip("LeaningData. Contains all the information on what this weapon should do while the character is leaning.")]
    [SerializeField]
    private LeaningData leaningData;


    [Tooltip("Weapon Recoil Data Asset. Used to get some camera recoil values, usually for weapons.")]
    [SerializeField]
    private RecoilData cameraRecoilData;


    [Tooltip("Weapon Recoil Data Asset. Used to get some recoil values, usually for weapons.")]
    [SerializeField]
    private RecoilData weaponRecoilData;




    /// <summary>
    /// GetCameraRecoilData.
    /// </summary>
    public  RecoilData GetCameraRecoilData() => cameraRecoilData;
    /// <summary>
    /// GetWeaponRecoilData.
    /// </summary>
    public  RecoilData GetWeaponRecoilData() => weaponRecoilData;

    /// <summary>
    /// GetRecoilData.
    /// </summary>
    public  RecoilData GetRecoilData(MotionType motionType) =>
        motionType == MotionType.Item ? GetWeaponRecoilData() : GetCameraRecoilData();

    /// <summary>
    /// GetLowerData.
    /// </summary>
    public  LowerData GetLowerData() => lowerData;
    /// <summary>
    /// GetLeaningData.
    /// </summary>
    public  LeaningData GetLeaningData() => leaningData;

    /// <summary>
    /// GetItemOffsets.
    /// </summary>
    public  ItemOffsets GetItemOffsets() => itemOffsets;

}
