using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Villager : MonoBehaviour {

    public int valueVillagers = 10;
    public Text textValue;
    public GameObject deathAnimation;
    //public ParticleSystem ps;


    public bool isSacrified = false;

	// Use this for initialization
	void Start () {
        textValue.text = valueVillagers.ToString();
    }

    public void UpdateValueText()
    {
        textValue.text = valueVillagers.ToString();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void Execute()
    {
        deathAnimation.SetActive(true);
    }
}
