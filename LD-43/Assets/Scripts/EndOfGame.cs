using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfGame : MonoBehaviour {

    public Text textScore;

	// Use this for initialization
	void Start () {

        int score = SettingsManager._instance.gameSettings.Score;
        int oldScore = SettingsManager._instance.gameSettings.oldScore;

        if(score > oldScore)
        {
            textScore.text = "New Highscore ! " + score.ToString();
            SettingsManager._instance.gameSettings.oldScore = score;
        }
        else
        {
            textScore.text = "Score : " + score.ToString();
        }

        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
