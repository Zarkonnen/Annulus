using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Operation
{
    public string type, key;
    public int value;

    public Operation(string type, string key, int value)
    {
        this.type = type;
        this.key = key;
        this.value = value;
    }

    public bool Eval(Tribe tribe, GameObject tribeO, Tile tile, GameObject tileO)
    {
        int cmp = key.StartsWith("$")
            ? (tile.properties.ContainsKey(key) ? tile.properties[key] : 0)
            : (tribe.properties.ContainsKey(key) ? tribe.properties[key] : 0);
        switch (type)
        {
            case "==":
                return cmp == value;
            case ">":
                return cmp > value;
            case "<":
                return cmp < value;
            case "=":
                if (key.StartsWith("$"))
                {
                    tile.properties[key] = value;
                }
                else
                {
                    tribe.properties[key] = value;
                }
                return true;
            case "+":
                if (key.StartsWith("$"))
                {
                    tile.properties[key] = cmp + value;
                }
                else
                {
                    tribe.properties[key] = cmp + value;
                }
                return true;
            case "-":
                if (key.StartsWith("$"))
                {
                    tile.properties[key] = cmp - value < 0 ? 0 : cmp - value;
                }
                else
                {
                    tribe.properties[key] = cmp - value < 0 ? 0 : cmp - value;
                }
                return true;
            case "setActive":
                if (!tile.specialActive && value != 0)
                {
                    tile.specialActive = true;
                    if (tileO.transform.FindChild("specialModelInactive") != null)
                    {
                        tileO.transform.FindChild("specialModelInactive").FindChild("default").GetComponent<Renderer>().enabled = false;
                    }
                    tileO.transform.FindChild("specialModelActive").FindChild("default").GetComponent<Renderer>().enabled = true;
                }
                if (tile.specialActive && value == 0)
                {
                    tile.specialActive = false;
                    if (tileO.transform.FindChild("specialModelInactive") != null)
                    {
                        tileO.transform.FindChild("specialModelInactive").FindChild("default").GetComponent<Renderer>().enabled = true;
                    }
                    tileO.transform.FindChild("specialModelActive").FindChild("default").GetComponent<Renderer>().enabled = false;
                }
                return true;
            case "isActive":
                return tile.specialActive == (value != 0);
            case "restart":
                tribe.doRestart = true;
                break;
        }
        return false;
    }
}
