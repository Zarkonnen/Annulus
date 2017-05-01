using UnityEngine;
using System.Collections.Generic;

public class Evt
{
    public string name;
    public string message;
    public string requiredSpecial;
    public List<List<Operation>> ifs = new List<List<Operation>>();
    public List<Option> options = new List<Option>();

    public string Desc()
    {
        string desc = "type " + name + "\nmsg " + message;
        if (requiredSpecial != null)
        {
            desc += "\nspecial " + requiredSpecial;
        }
        foreach (List<Operation> f in ifs)
        {
            desc += "\nif";
            foreach (Operation op in f)
            {
                desc += " " + op.key + " " + op.type + " " + op.value;
            }
        }
        foreach (Option o in options)
        {
            desc += "\noption " + o.text + "\noutcome";
            foreach (Operation op in o.outcome)
            {
                desc += " " + op.key + " " + op.type + " " + op.value;
            }
        }
        return desc;
    }

    public string Text()
    {
        string text = message + "\n";
        for (int i = 0; i < options.Count; i++)
        {
            text += "\n" + (i + 1) + " " + options[i].text;
        }
        return text;
    }

    public bool Valid(Tribe tribe, GameObject tribeO, Tile tile, GameObject tileO)
    {
        if (requiredSpecial != null && (tile.special == null || !tile.special.name.Equals(requiredSpecial)))
        {
            //Debug.Log("reqspec fail");
            return false;
        }
        foreach (List<Operation> f in ifs)
        {
            bool valid = true;
            foreach (Operation op in f)
            {
                //Debug.Log(op.key + " " + op.type + " " + op.value + " -> " + op.Eval(tribe, tribeO, tile, tileO));
                valid &= op.Eval(tribe, tribeO, tile, tileO);
            }
            if (valid)
            {
                return true;
            }
        }
        return false;
    }
}
