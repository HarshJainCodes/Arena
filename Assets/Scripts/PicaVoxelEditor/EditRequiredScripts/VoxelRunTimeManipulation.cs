using PicaVoxel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

/// <summary>s
/// Major Class that handles all the Voxel Manipulation
/// </summary>
public class VoxelRunTimeManipulation : MonoBehaviour
{
    /// <summary>
    /// The enum deternmines the type of operation the RunTimeVoxelManipulation Script is set to.
    /// </summary>
    public enum OperationType
    {
        none,
        add,
        remove,
        edit,
        bucket
    }
    /// <summary>
    /// The bool checks whether the script to add is enables or disabled
    /// </summary>
    /*[SerializeField]
    private bool _Add = false;*/
    /// <summary>
    /// The main camera transform is necessary to fire a ray from the centre
    /// </summary>
    [SerializeField]
    private Transform _CamTransform;
    /// <summary>
    /// The layer mask is necessary to target only the voxel
    /// </summary>
    [SerializeField]
    private LayerMask _VoxelLayer;
    /// <summary>
    /// This array stores the volumes of the voxels that are to be modified
    /// </summary>
    [SerializeField]
    private Volume[] _VoxelVolumes;
    /// <summary>
    /// This variable points to the currently selected volume
    /// </summary>
    [SerializeField]
    private int _SelectedVolume = 0;
    /// <summary>
    /// This colour will be used to select a colour from the editor and use the same
    /// </summary>
    [SerializeField]
    private Color _selectedColour;

    /// <summary>
    /// This enumerator determines which operation is to be executed and has getter method 
    /// <see cref="GetTypeOfOperation"/>
    /// and setter method
    /// <see cref="SetTypeOfOperation(OperationType)"/>
    /// </summary>
    [SerializeField]
    private OperationType _typeOfOperation = OperationType.none;

    [SerializeField] private bool _MirrorChange = false;

    [SerializeField] private ImprovedCamSwivel _CamSwivel;

    [SerializeField] private GameObject _ResetPrefab;
    [SerializeField] private Transform _ResetParent;


    [SerializeField] private GameObject _ResetTilePrefab;


    Cqueue<previousOp>[] Cq;

    private void Start()
    {
        Cq = new Cqueue<previousOp>[_VoxelVolumes.Length];
        for(int i=0;i<_VoxelVolumes.Length;i++)
        {
            Cq[i] = new Cqueue<previousOp>(20);
        }
    }

    void Update()
    {
        _VoxelManipulation();
    }

    /// <summary>
    /// This function holds code for manipulating individual voxels 
    /// </summary>
    private void _VoxelManipulation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // bool found is used to stop the loop when we actually find a voxel;
            bool found = false;

            Ray screenRay=Camera.main.ScreenPointToRay(Input.mousePosition);

            // Casting a ray to detect whether we hit a voxel or not with the cam centre
            Ray r = new Ray(_CamTransform.position, _CamTransform.forward);

            // Debugging and checking the ray if it is working properly
            Debug.DrawRay(r.origin, r.direction * 100f, Color.red, 10f);

            // The for loop runs to find the exact spot at which the voxel is found
            for (float d = 0; d < 50f; d += 0.05f)
            {
                // This is a temporary variable that stores the volume that is currently being edited
                Volume currentVoxelVolume = _VoxelVolumes[_SelectedVolume];

                // The Voxel v stores the position of the current voxel that is selected by the ray
                Voxel? v = currentVoxelVolume.GetVoxelAtWorldPosition(screenRay.GetPoint(d));
                //stores the array position of the point in space
                PicaVoxelPoint temp=currentVoxelVolume.GetVoxelArrayPosition(screenRay.GetPoint(d));
                //checks if that voxel is active
                int halfCubeSize = currentVoxelVolume.XSize/2;
                //stores half of the cube size so that mirror functions
                if (v.HasValue && v.Value.Active)
                { 
                    switch (_typeOfOperation)
                    {
                        case OperationType.none:

                            // This state exists in case that the player wants to move the blocks and so on.
                            break;

                        case OperationType.add:
                            Volume temp2 = new Volume();
                            temp2=currentVoxelVolume;
                            addtoreversestack(temp2,OperationType.add);
                            //gets the position of the voxel to be built
                            Vector3 buildPos = screenRay.GetPoint(d - 0.05f);

                            // The Voxel v2 stores the location of the voxel to be created and is made to check whether the voxel here is active or not 
                            Voxel? v2 = currentVoxelVolume.GetVoxelAtWorldPosition(buildPos);
                            if (v2.HasValue && !v2.Value.Active)
                            {

                                // Here we actually set the values of the voxel at that position
                                currentVoxelVolume.SetVoxelAtWorldPosition(buildPos, new Voxel()
                                {
                                    State = VoxelState.Active,
                                    Color = _selectedColour,
                                    Value = 128
                                });
                                if(_MirrorChange)
                                {
                                    PicaVoxelPoint p = currentVoxelVolume.GetVoxelArrayPosition(buildPos);
                                    p.X = p.X - halfCubeSize ;
                                    p.X = halfCubeSize - p.X - 1;
                                    Voxel? v3=currentVoxelVolume.GetVoxelAtArrayPosition(p.X,p.Y,p.Z);
                                    if(v3.HasValue && !v3.Value.Active)
                                    {
                                        currentVoxelVolume.SetVoxelAtArrayPosition(p, new Voxel()
                                        {
                                            State = VoxelState.Active,
                                            Color = _selectedColour,
                                            Value = 128
                                        }); 
                                    }
                                }
                            }
                            break;

                        case OperationType.remove:
                            Volume temp3 = new Volume(); 
                            temp3=currentVoxelVolume;
                            addtoreversestack(temp3, OperationType.remove);
                            // We simply set the satus of that particular voxel as inactive thus it does not render
                            currentVoxelVolume.SetVoxelAtWorldPosition(screenRay.GetPoint(d), new Voxel()
                            {
                                State = VoxelState.Inactive,
                                Color = _selectedColour,
                                Value = 128
                            });
                            if(_MirrorChange)
                            {
                                PicaVoxelPoint p = currentVoxelVolume.GetVoxelArrayPosition(screenRay.GetPoint(d));
                                p.X = p.X - halfCubeSize;
                                p.X = halfCubeSize - p.X -1;
                                Voxel? v3 = currentVoxelVolume.GetVoxelAtArrayPosition(p.X,p.Y,p.Z);
                                if (v3.HasValue && v3.Value.Active)
                                {
                                    currentVoxelVolume.SetVoxelAtArrayPosition(p, new Voxel()
                                    {
                                        State = VoxelState.Inactive,
                                        Color = _selectedColour,
                                        Value = 128
                                    });
                                }
                            }
                            
                            break;

                        case OperationType.edit:
                            Volume temp4 = new Volume(); 
                            temp4=currentVoxelVolume;
                            addtoreversestack(temp4, OperationType.edit);
                            // We simply change the colour of the voxel here.
                            currentVoxelVolume.SetVoxelAtWorldPosition(screenRay.GetPoint(d), new Voxel()
                            {
                                State = VoxelState.Active,
                                Color = _selectedColour,
                                Value = 128
                            });
                            if (_MirrorChange)
                            {
                                PicaVoxelPoint p = currentVoxelVolume.GetVoxelArrayPosition(screenRay.GetPoint(d));
                                p.X = p.X - halfCubeSize;
                                p.X = halfCubeSize - p.X -1;
                                Voxel? v3 = currentVoxelVolume.GetVoxelAtArrayPosition(p.X, p.Y, p.Z);
                                if (v3.HasValue && v3.Value.Active)
                                {
                                    currentVoxelVolume.SetVoxelAtArrayPosition(p, new Voxel()
                                    {
                                        State = VoxelState.Active,
                                        Color = _selectedColour,
                                        Value = 128
                                    });
                                }
                            }
                            break;

                        case OperationType.bucket:
                            Volume temp5 = new Volume(); 
                            temp5=currentVoxelVolume;
                            addtoreversestack(temp5, OperationType.bucket);
                            for (int i = 0; i < currentVoxelVolume.XSize; i++)
                                for (int j = 0; j < currentVoxelVolume.YSize; j++)
                                    for (int k = 0; k < currentVoxelVolume.ZSize; k++)
                                    {
                                        Voxel? a = currentVoxelVolume.GetVoxelAtArrayPosition(i, j, k);
                                        if (a.Value.Active)
                                        {
                                            currentVoxelVolume.SetVoxelAtArrayPosition(new PicaVoxelPoint(new Vector3(i,j,k)), new Voxel()
                                            {
                                                State = VoxelState.Active,
                                                Color = _selectedColour,
                                                Value = 128
                                            });
                                        }
                                        
                                    }
                            break;
                    }
                    found = true;
                }
                if (found)
                {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// This function returns the current operation that is being performed.
    /// </summary>
    /// <returns>OperationType enum</returns>
    public OperationType GetTypeOfOperation() { return _typeOfOperation; }
    /// <summary>
    /// This function sets the opertaion that is to be performed on a voxel.
    /// </summary>
    /// <param name="op"></param>
    public void SetTypeOfOperation(OperationType op) { _typeOfOperation = op; }

    /// <summary>
    /// This function returns the currently chosen color 
    /// </summary>
    /// <returns>return type is Color</returns>
    public Color GetColor() { return _selectedColour; }

    /// <summary>
    /// This function sets the user requested color to be put on the blocks
    /// </summary>
    /// <param name="choice"></param>
    public void SetColor(Color choice) { _selectedColour = choice; }

    /// <summary>
    /// this getter and setter method sets the private variable
    /// <see cref="_MirrorChange"/> and allows you to mirror developments in runtime
    /// </summary>
    public bool MirrorChange{ get { return _MirrorChange; } set { _MirrorChange = value; } }

    /// <summary>
    /// This getter method gets the currently editable volume 
    /// </summary>
    public Transform CurrentVolume { get {  return _VoxelVolumes[_SelectedVolume].transform; } }


    /// <summary>
    /// Function that returns the currently selected volume
    /// </summary>
    /// <param name="index"></param>
    /// <returns>It returns the volume at a particular index</returns>
    public Volume getVolume(int index)
    { 
         return _VoxelVolumes[index]; 
    }

    /// <summary>
    /// Getter and setter method for selected volume and turns of the deselected volume
    /// </summary>
    public int SelectedVolume { set { _VoxelVolumes[_SelectedVolume].transform.gameObject.SetActive(false); _SelectedVolume = value; _VoxelVolumes[_SelectedVolume].transform.gameObject.SetActive(true); } } //_CamSwivel.ChangeTransform = CurrentVolume; } }

    /// <summary>
    /// Scripts for the operation reversal stack. They do no work as of yet.
    /// </summary>
    public void ResetPrefab()
    {
        Destroy(_VoxelVolumes[_SelectedVolume].transform.gameObject);
        if (_SelectedVolume != 6)
        {
            GameObject temp = Instantiate(_ResetPrefab, _ResetParent);
            temp.name = $"PicaVoxel Volume ({_SelectedVolume})";
            _VoxelVolumes[_SelectedVolume] = temp.GetComponent<Volume>();
        }
        else
        {
            GameObject temp = Instantiate(_ResetTilePrefab, _ResetParent);
            temp.name = $"PicaVoxel Volume ({_SelectedVolume})";
            _VoxelVolumes[_SelectedVolume] = temp.GetComponent<Volume>();
        }
    }

    public void RevertChange()
    {
        if (!Cq[_SelectedVolume].isEmpty())
        {
            previousOp temp = Cq[_SelectedVolume].popFront();
            _VoxelVolumes[_SelectedVolume] = temp.self;
        }
    }

    public void addtoreversestack(Volume t,OperationType op)
    {
        if (!Cq[_SelectedVolume].isFull())
        {
            Debug.Log("Here");
            Cq[_SelectedVolume].push(new previousOp(t,op));
        }
        else
        {
            Cq[_SelectedVolume].popRear();
            Cq[_SelectedVolume].push(new previousOp(t, op));
        }
    }


}
/// <summary>
/// This class is to record the previous operation that was performed on the volume so as to be able to reverse it. It tries to store the entire volume but i feel that is not able to save
/// the voxel data as it is probably only creating a shallow copy and hence the changes in volume are reflected along all
/// </summary>
public class previousOp
{
    public Volume self;
    /*public Voxel? _self;
    public VoxelRunTimeManipulation.OperationType op;
    public bool twinned=false; */
    public VoxelRunTimeManipulation.OperationType op;


    public previousOp(Volume v, VoxelRunTimeManipulation.OperationType op)
    {
        this.op = op;
        self = v;
    }

    //public Voxel? self { get { return _self; } set { _self = value; } }
}

