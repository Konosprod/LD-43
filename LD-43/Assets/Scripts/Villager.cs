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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<Villager>() != null)
                {
                    Debug.Log(hit.transform.gameObject.name);
                }
                else
                {
                    Debug.Log("This isn't a Player");
                }
            }
        }
    }
}
