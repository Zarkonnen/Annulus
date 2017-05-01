using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleFade : MonoBehaviour {
    public float t;
    public float fadeIn = 1.0f;
    public float fadeOut = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
        t += Time.deltaTime;
        float amt = 0;
        if (t < fadeIn)
        {
            amt = t / fadeIn;
        }
        else
        {
            if (fadeOut > 0)
            {
                amt = 1 - (t - fadeIn) / fadeOut;
            }
            else
            {
                amt = 1;
            }
        }
        amt = Mathf.Clamp(amt, 0, 1);
        transform.GetComponent<Text>().color = new Color(1, 1, 1, amt);
	}
}
