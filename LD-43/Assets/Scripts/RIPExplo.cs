using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIPExplo : MonoBehaviour {

	// Use this for initialization
	void Awake()
    {
        StartCoroutine(SelfDestroy());
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
