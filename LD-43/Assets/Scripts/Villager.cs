using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Villager : MonoBehaviour
{

    public int valueVillagers = 10;
    public Text textValue;
    public GameObject deathAnimation;
    //public ParticleSystem ps;


    public bool isSacrified = false;

    // Use this for initialization
    void Start()
    {
        UpdateValueText();
    }

    public void UpdateValueText()
    {
        textValue.text = valueVillagers.ToString();
        if (valueVillagers > 50)
            textValue.color = Color.blue;
        else if (valueVillagers > 200)
            textValue.color = Color.magenta;
        else if (valueVillagers > 1000)
            textValue.color = Color.red;
        else if (valueVillagers > 5000)
            textValue.color = Color.yellow;
        else
            textValue.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Execute()
    {
        deathAnimation.SetActive(true);
    }
}
