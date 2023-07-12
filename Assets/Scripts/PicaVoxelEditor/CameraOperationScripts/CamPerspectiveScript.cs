using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPerspectiveScript : MonoBehaviour
{
    [SerializeField]
    private Camera _MainCam;
    private bool _IsOrthographic = false;
    private bool _IsPerspective = true;

    public void SwapToPerspective()
    {
        if(!_IsPerspective && _IsOrthographic)
        {
            _IsPerspective = true;
            _IsOrthographic = false;
            _MainCam.orthographic = false;
        }
    }

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
