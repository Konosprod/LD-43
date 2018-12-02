using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBeam : MonoBehaviour {

    public bool activated = true;
    public GameObject target;
    
    private ParticleSystem ps;

    private bool flagStopBeam = false;

	// Use this for initialization
	void Start () {
        ps = GetComponentInChildren<ParticleSystem>();
        ps.gameObject.SetActive(false);
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
            if (!flagStopBeam && ps.gameObject.activeSelf)
            {
                StartCoroutine(DisableParticle());
                flagStopBeam = true;
            }
        }
	}

    IEnumerator DisableParticle()
    {
        yield return new WaitForSeconds(0.25f);
        ps.gameObject.SetActive(false);
        flagStopBeam = false;
    }
}
