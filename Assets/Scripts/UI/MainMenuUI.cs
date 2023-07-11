using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _PlayButton;
    [SerializeField] private Button _CreateButton;

    // Start is called before the first frame update
    void Start()
    {
        _PlayButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Play");
        });

        _CreateButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Create");
        });
    }
}
