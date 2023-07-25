using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetImporterTest : MonoBehaviour
{
    [SerializeField]
    private string _AssetFolder;
    private string _FullPath;

    void Awake()
    {
        _FullPath = $"{Application.dataPath}"+$"{_AssetFolder}";
        Debug.Log(_FullPath);
    }

    public GameObject[] ImportAssetAt()
    {
        GameObject[] ReturnObj;
        //string pathcompletion = $"{_FullPath}/{side}Side{number}";
        //ReturnObj = AssetDatabase.LoadAllAssetsAtPath(_FullPath);
        ReturnObj = (GameObject[])Resources.LoadAll<GameObject>("PrefabImportData");
        return ReturnObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
