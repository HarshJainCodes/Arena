using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LaserType { Lasersight, Flashlight }
public class Laser : MonoBehaviour
{
    [Tooltip("Sprite. Displayed on the player's interface.")]
    [SerializeField]
    private Sprite sprite;

    [Tooltip("Type of laser.")]
    [SerializeField]
    private LaserType laserType;

    [Tooltip("True if the lasersight should start active.")]
    [SerializeField]
    private bool active = true;

    [Tooltip("If true, the laser will be turned off automatically while the character is running.")]
    [SerializeField]
    private bool turnOffWhileRunning = true;

    [Tooltip("If true, the laser will be turned off automatically while the character is aiming.")]
    [SerializeField]
    private bool turnOffWhileAiming = true;

    [Tooltip("Transform of the laser.")]
    [SerializeField]
    private Transform laserTransform;

    [Tooltip("Determines how thick the laser beam is.")]
    [SerializeField]
    private float beamThickness = 1.2f;

    
    [Tooltip("Maximum distance for tracing the laser beam.")]
    [SerializeField]
    private float beamMaxDistance = 500.0f;

    private Transform beamParent;

    public  Sprite GetSprite() => sprite;
    /// <summary>
    /// GetTurnOffWhileRunning.
    /// </summary>
    public  bool GetTurnOffWhileRunning() => turnOffWhileRunning;
    /// <summary>
    /// GetTurnOffWhileAiming.
    /// </summary>
    public  bool GetTurnOffWhileAiming() => turnOffWhileAiming;
    // Start is called before the first frame update
    public  void Toggle()
    {
        //Toggle active.
        active = !active;

        //Activate/Deactivate the laser.
        Reapply();

        //Plays a little sound now that we're toggling this laser!
        /*if (toggleClip != null)
            ServiceLocator.Current.Get<IAudioManagerService>().PlayOneShot(toggleClip, toggleAudioSettings);*/
    }
    /// <summary>
    /// Reapply.
    /// </summary>
    public  void Reapply()
    {
        //Activate/Deactivate the laser.
        if (laserTransform != null)
            laserTransform.gameObject.SetActive(active);
    }
    /// <summary>
    /// Hide.
    /// </summary>
    public  void Hide()
    {
        //Deactivate Laser.
        if (laserTransform != null)
            laserTransform.gameObject.SetActive(false);
    }
}
