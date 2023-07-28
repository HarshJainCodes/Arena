using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{

    private TextMeshProUGUI _LoadingText;
    private float _Timer = 0;
    private float _ProgressInterval = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _LoadingText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _Timer += Time.deltaTime;

        if (_Timer > _ProgressInterval)
        {
            _Timer = 0;
            _LoadingText.text += ". ";

            if (_LoadingText.text.Length > 10)
            {
                _LoadingText.text = "";
            }
        }
    }
}
