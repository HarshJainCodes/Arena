using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component is present on all the individual color buttons on the various palettes
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[ExecuteInEditMode]
public class IndividualColour : MonoBehaviour
{
    [SerializeField] private Color _Value;
    private ColourPaleteMaster _master;
    public Color value { get { return _Value; } set { _Value = value; } }

    public void Awake()
    {
        gameObject.GetComponent<Image>().color = value;
        _master=gameObject.GetComponentInParent<ColourPaleteMaster>();
        gameObject.GetComponent<Button>().onClick.AddListener(PassToParentObject);
    }

    public void PassToParentObject()
    {
        _master.passColour(_Value,this);
    }

    public void setParentColor()
    {
        gameObject.GetComponent<Image>().color = value;
    }


}
