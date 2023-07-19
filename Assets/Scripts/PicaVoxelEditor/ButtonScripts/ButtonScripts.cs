using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonScripts : MonoBehaviour
{
    [SerializeField] ControlScript _script;
    [SerializeField] private int _SelfVolume;
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
        _script.ActiveVolume = _SelfVolume;
        _snapshotsOfVolumes.grabsnap();
        _snapshotsOfVolumes.index = _SelfVolume;
    }
    // Update is called once per frame
}
