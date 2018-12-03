using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour {

    private Text text;
    public float blinkTime = 1f;
    public float pauseTime = 2f;

    private bool fadeout = true;
    private float elapsedTime = 0f;
    private float elapsedPauseTime = 0f;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        Color c = text.color;

        elapsedTime += Time.deltaTime;
        elapsedPauseTime += Time.deltaTime;

        float progress = elapsedTime / blinkTime;

        if (fadeout)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1, 0, progress));
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0, 1, progress));
        }

        if(elapsedTime >= blinkTime)
        {
            if(elapsedPauseTime >= pauseTime)
            {
                fadeout = !fadeout;
                elapsedTime = 0f;
                elapsedPauseTime = 0f;
            }
        }
		
	}
}
