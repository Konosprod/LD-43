using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SoundManager._instance.PlaySFX(SFXType.Thunder);
        ParticleSystem ps = GetComponent<ParticleSystem>();

        if (ps != null)
        {
            var main = ps.main;
            main.playOnAwake = true;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnParticleSystemStopped()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
