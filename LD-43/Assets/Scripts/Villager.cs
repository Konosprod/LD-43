using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Villager : MonoBehaviour {

    public int valueVillagers = 10;
    public Text textValue;

	// Use this for initialization
	void Start () {
        textValue.text = valueVillagers.ToString();
    }
	
	// Update is called once per frame
	void Update () {

    }
}
