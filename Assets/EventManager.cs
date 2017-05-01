using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviour {

    public GameObject tribe;

    public TextAsset eventsData;
    public List<Evt> events = new List<Evt>();
    public GameObject text;
    public Evt currentEvent;
    public GameObject currentEventTileO;
    float msgTimeout = 0;
    public float msgTimeoutAmt = 3.0f;

    // Use this for initialization
    void Start()
    {
        Evt currentEvt = null;
        Option currentOption = null;
        foreach (string l in eventsData.text.Split('\n'))
        {
            string line = l.Trim();
            if (!line.Contains(" ")) { continue; }
            string op = line.Split(new char[] { ' ' }, 2)[0];
            string val = line.Split(new char[] { ' ' }, 2)[1];
            string[] bits = val.Split(new char[] { ' ' });
            switch (op)
            {
                case "type":
                    currentEvt = new Evt();
                    currentEvt.name = val;
                    events.Add(currentEvt);
                    break;
                case "msg":
                    currentEvt.message = val;
                    break;
                case "special":
                    currentEvt.requiredSpecial = val;
                    break;
                case "if":
                    currentEvt.ifs.Add(Ops(bits));
                    break;
                case "option":
                    currentOption = new Option();
                    currentOption.text = val;
                    currentEvt.options.Add(currentOption);
                    break;
                case "outcome":
                    currentOption.outcome = Ops(bits);
                    break;
                case "outcomeMsg":
                    currentOption.message = val;
                    break;
            }
        }
        foreach (Evt e in events)
        {
            Debug.Log(e.Desc());
        }
    }

    private List<Operation> Ops(string[] bits)
    {
        List<Operation> ops = new List<Operation>();
        for (int i = 0; i < bits.Length; i += 3)
        {
            ops.Add(new Operation(bits[i + 1], bits[i], Int32.Parse(bits[i + 2])));
        }
        return ops;
    }

    public Evt FindEvent(GameObject tribeO, GameObject tileO)
    {
        Tribe tribe = tribeO.GetComponent<Tribe>();
        Tile tile = tileO.GetComponent<Tile>();
        foreach (Evt e in events)
        {
            if (e.Valid(tribe, tribeO, tile, tileO))
            {
                return e;
            }
        }
        return null;
    }

    public void Run(Evt e, GameObject tribeO, GameObject tileO)
    {
        currentEvent = e;
        text.GetComponent<Text>().text = currentEvent.Text();
        currentEventTileO = tileO;
    }

    void Choose(Option o)
    {
        Tribe tribeC = tribe.GetComponent<Tribe>();
        Tile tile = currentEventTileO.GetComponent<Tile>();
        text.GetComponent<Text>().text = o.message;
        foreach (Operation op in o.outcome)
        {
            op.Eval(tribeC, tribe, tile, currentEventTileO);
        }
        currentEvent = null;
        msgTimeout = msgTimeoutAmt;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentEvent != null)
        {
            if (Input.GetAxis("Option1") != 0 && currentEvent.options.Count > 0)
            {
                Choose(currentEvent.options[0]);
                currentEvent = null;
            }
            if (Input.GetAxis("Option2") != 0 && currentEvent.options.Count > 1)
            {
                Choose(currentEvent.options[1]);
                currentEvent = null;
            }
            if (Input.GetAxis("Option3") != 0 && currentEvent.options.Count > 2)
            {
                Choose(currentEvent.options[2]);
                currentEvent = null;
            }
        }
        else
        {
            if (msgTimeout > 0)
            {
                msgTimeout -= Time.deltaTime;
            }
            else
            {
                text.GetComponent<Text>().text = "";
            }
        }
	}
}
