using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour {

    public GameObject tribe;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        string info = "";
        Dictionary<string, int> ps = tribe.transform.GetComponent<Tribe>().properties;
        foreach (string k in ps.Keys)
        {
            //info += ps[k] + " " + k + "\n";
            if (k.StartsWith("."))
            {
                continue;
            }
            if (ps[k] == 0)
            {
                continue;
            }
            if (k.StartsWith("#"))
            {
                info += ps[k] + " " + k.Substring(1) + "\n";
            }
            else
            {
                info += k + "\n";
            }
        }
        transform.GetComponent<Text>().text = info;
    }
}
