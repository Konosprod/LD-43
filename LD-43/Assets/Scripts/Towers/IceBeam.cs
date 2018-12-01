using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBeam : MonoBehaviour {

    public bool activated = true;
    public GameObject target;
    
    private ParticleSystem ps;

	// Use this for initialization
	void Start () {
        ps = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(activated && target != null)
        {
            Vector3 positionTarget = target.transform.position;
            positionTarget += new Vector3(0f, 0.1f, 0f);
            ps.transform.LookAt(positionTarget);
            ps.gameObject.SetActive(true);
        }
        else
        {
            ps.gameObject.SetActive(false);
        }
	}
}
