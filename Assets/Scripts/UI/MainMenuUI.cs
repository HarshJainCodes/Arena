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

    // Start is called before the first frame update
    void Start()
    {
        _PlayButton.onClick.AddListener(() =>
        {
            _BackgoundImage.sprite = _LoadingSprite;
            _PlayButton.gameObject.SetActive(false);
            _CreateButton.gameObject.SetActive(false);
            SceneManager.LoadScene("Play");

            //StartCoroutine(LoadScene());
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

        while (!asyncOperation.isDone)
        {
            Debug.Log("LOADING THE NEXT SCENE");
        }
    }
}
