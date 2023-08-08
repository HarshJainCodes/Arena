using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script changes cam selection between perspective and orthographic
/// </summary>
public class CamPerspectiveScript : MonoBehaviour
{
    /// <summary>
    /// Main cam will basically have the camera component to allow it to change projection
    /// </summary>
    [SerializeField]
    private Camera _MainCam;
    /// <summary>
    /// _IsOrthographic checks if the projection is orthographic or not
    /// </summary>
    private bool _IsOrthographic = false;
    /// <summary>
    /// _IsPerspective checks if the projection is perspective or not
    /// </summary>
    private bool _IsPerspective = true;



    /// <summary>
    /// This method swaps projection to Perspective
    /// </summary>
    public void SwapToPerspective()
    {
        if(!_IsPerspective && _IsOrthographic)
        {
            _IsPerspective = true;
            _IsOrthographic = false;
            _MainCam.orthographic = false;
        }
    }


    /// <summary>
    /// This method swaps projection to Orthographic
    /// </summary>
    public void SwapToOrthographic()
    {
        if (_IsPerspective && !_IsOrthographic)
        {
            _IsPerspective = false;
            _IsOrthographic = true;
            _MainCam.orthographic = true;
        }
    }
}
