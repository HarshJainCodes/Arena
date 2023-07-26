using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimerUI : MonoBehaviour
{

    [SerializeField] private Image _WaveTimerBarImage;
    [SerializeField] private TextMeshProUGUI _WaveTimerText;
    [SerializeField] private SpawnManager _SpawnManager;

    Material WaveTimeBarMat;

    // Start is called before the first frame update
    void Start()
    {
        _WaveTimerBarImage.color = Color.green;
		WaveTimeBarMat =_WaveTimerBarImage.material;
        WaveTimeBarMat.SetFloat("_WaveTime", 0.6f);
    }

    // Update is called once per frame
    void Update()
	{
		WaveTimeBarMat.SetFloat("_WaveTime", (_SpawnManager.CurrentTime) / (_SpawnManager.TimeBetweenWaves));
        _WaveTimerText.text = "Wave " + (_SpawnManager.CurrentWave + 1) + " in " + (int)_SpawnManager.CurrentTime + "s";
        _WaveTimerBarImage.gameObject.SetActive(WaveTimeBarMat.GetFloat("_WaveTime") != 1);
        _WaveTimerText.gameObject.SetActive(WaveTimeBarMat.GetFloat("_WaveTime") != 1);
	}
}
