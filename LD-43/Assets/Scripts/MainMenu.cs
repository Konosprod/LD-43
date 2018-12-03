using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [Header("Main Menu")]
    public Button startButton;
    public Button optionsButton;
    public Button quitButton;

    [Header("Difficulty")]
    public GameObject panelDiff;
    public Button buttonEasy;
    public Button buttonNormal;
    public Button buttonHard;

    [Header("Options")]
    public GameObject panelOptions;

    // Use this for initialization
	void Start () {
        SoundManager._instance.PlayMusic(SoundType.BGM);
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(Options);
        quitButton.onClick.AddListener(QuitGame);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    private void Options()
    {
        panelOptions.SetActive(true);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
