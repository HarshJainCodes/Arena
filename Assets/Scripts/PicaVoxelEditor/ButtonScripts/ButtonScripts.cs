using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class handles the various buttons that form the images that display current volume and edited volumes
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonScripts : MonoBehaviour
{
    /// <summary>
    ///Handle for the master script attached to the parent of the container holding the buttons
    /// </summary>
    [SerializeField] ControlScript _script;
    /// <summary>
    /// Used to set the snap of the current volume whe moving to edit the next volume
    /// </summary>
    [SerializeField] private int _SelfVolume;
    /// <summary>
    /// Hande to add the ONclick listner onto the button
    /// </summary>
    private Button _button;

    [SerializeField] private SnapshotsOfVolumes _snapshotsOfVolumes;
    // Start is called before the first frame update
    void Start()
    {
        _button= GetComponent<Button>();
        _button.onClick.AddListener(ChangeVolumeTo);
    }
    private void ChangeVolumeTo()
    {
        AudioManager.instance?.PlayOneShot(FMODEvents.instance.Select, this.transform.position);
        if (_script.ActiveVolume!=_SelfVolume)
        {
            _snapshotsOfVolumes.grabsnap(_SelfVolume);
            _script.ActiveVolume = _SelfVolume;
        }
    }
    // Update is called once per frame
}
