using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour {
    [SerializeField] private GameObject canvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.SetActive(!canvas.gameObject.activeSelf);
        }
    }

    public void Play1v1()
    {
        PlayerPrefs.SetInt("GameMode", 2);
        SceneManager.LoadScene(1);
    }

    public void Play3v3()
    {
        PlayerPrefs.SetInt("GameMode", 3);
        SceneManager.LoadScene(1);
    }

    public void Play5v5()
    {
        PlayerPrefs.SetInt("GameMode", 4);
        SceneManager.LoadScene(1);
    }

    public void OpenProfile()
    {
        SceneManager.LoadScene(5);
    }

    public void OpenCharacters()
    {
        SceneManager.LoadScene(6);
    }

    public void OpenShop()
    {
        SceneManager.LoadScene(7);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        // If running in a build, quit the application.
        Application.Quit();
#endif
    }
}