using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOfImport : MonoBehaviour
{
    [SerializeField]
    Object[] assets;
    [SerializeField]
    private AssetImporterTest _script;
    private void Start()
    {
        assets = _script.ImportAssetAt();
    }
}
