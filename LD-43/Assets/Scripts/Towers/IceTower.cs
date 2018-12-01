using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : MonoBehaviour {

    private Tower tower;

	// Use this for initialization
	void Start () {
        tower = GetComponent<Tower>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!tower.isPreviewMode)
        {
            
        }
	}
}
