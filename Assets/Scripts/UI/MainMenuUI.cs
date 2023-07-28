using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _PlayButton;
    [SerializeField] private Button _CreateButton;

    [SerializeField] private Image _BackgoundImage;
    [SerializeField] private Sprite _LoadingSprite;

    [SerializeField] private GameObject _LoadingText;

    // Start is called before the first frame update
    void Start()
    {
        _LoadingText.SetActive(false);
        _PlayButton.onClick.AddListener(() =>
        {
            _BackgoundImage.sprite = _LoadingSprite;
            _PlayButton.gameObject.SetActive(false);
            _CreateButton.gameObject.SetActive(false);
            _LoadingText.SetActive(true);
            //SceneManager.LoadScene("Play");

            StartCoroutine(LoadScene());
        });

        _CreateButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Create");
        });
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Play");
    }
}
