using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCreator : MonoBehaviour {

    public GameObject tile;
    public TextAsset tileTypesData;
    public TextAsset specialTypesData;
    public TextAsset mapData;
    public GameObject tribe;
    private Dictionary<string, TileType> tileTypes = new Dictionary<string, TileType>();
    private Dictionary<string, SpecialType> specialTypes = new Dictionary<string, SpecialType>();
    private GameObject[,] grid;
    private TileType[,] mapGrid;
    private SpecialType[,] specialGrid;

    // Use this for initialization
    void Start()
    {
        TileType currentType = null;
        foreach (string l in tileTypesData.text.Split('\n'))
        {
            string line = l.Trim();
            if (!line.Contains(" ")) { continue; }
            string op = line.Split(new char[] { ' ' }, 2)[0];
            string[] bits = line.Split(new char[] { ' ' }, 2)[1].Split(new char[] { ' ' });
            switch (op)
            {
                case "type":
                    currentType = new TileType();
                    currentType.name = bits[0];
                    currentType.mapChar = bits[1];
                    tileTypes[currentType.mapChar] = currentType;
                    break;
                case "models":
                    foreach (string mod in bits)
                    {
                        //currentType.models.Add((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/" + mod + ".obj", typeof(GameObject)));
                        currentType.models.Add(Resources.Load<GameObject>(mod));
                    }
                    break;
                case "blocker":
                    currentType.blocker = true;
                    break;
            }
        }

        SpecialType currentSpecial = null;
        foreach (string l in specialTypesData.text.Split('\n'))
        {
            string line = l.Trim();
            if (!line.Contains(" ")) { continue; }
            string op = line.Split(new char[] { ' ' }, 2)[0];
            string[] bits = line.Split(new char[] { ' ' }, 2)[1].Split(new char[] { ' ' });
            switch (op)
            {
                case "type":
                    currentSpecial = new SpecialType();
                    currentSpecial.name = bits[0];
                    currentSpecial.mapChar = bits[1];
                    specialTypes[currentSpecial.mapChar] = currentSpecial;
                    break;
                case "models":
                    foreach (string mod in bits)
                    {
                        currentSpecial.models.Add(Resources.Load<GameObject>(mod));
                    }
                    break;
                case "activeModels":
                    foreach (string mod in bits)
                    {
                        currentSpecial.activeModels.Add(Resources.Load<GameObject>(mod));
                    }
                    break;
            }
        }

        int tribeW = 3;
        int tribeS = 71;
        int widthSegments = 10;
        int wPerSegment = 21;
        int ringSegments = 100;
        float radius = 330;
        mapGrid = new TileType[ringSegments, widthSegments];
        specialGrid = new SpecialType[ringSegments, widthSegments];
        string[] rows = mapData.text.Split('\n');
        for (int s = 0; s < ringSegments; s++)
        {
            for (int w = 0; w < widthSegments; w++)
            {
                mapGrid[s, w] = tileTypes[rows[w * 2].Substring(s, 1)];
                if (specialTypes.ContainsKey(rows[w * 2 + 1].Substring(s, 1)))
                {
                    specialGrid[s, w] = specialTypes[rows[w * 2 + 1].Substring(s, 1)];
                }
            }
        }
        grid = new GameObject[ringSegments, widthSegments];
        for (int s = 0; s < ringSegments; s++)
        {
            for (int w = 0; w < widthSegments; w++)
            {
                TileType type = mapGrid[s, w];
                SpecialType special = specialGrid[s, w];
                GameObject tileO = Instantiate(tile, new Vector3(
                        Mathf.Cos(s * Mathf.PI * 2 / ringSegments) * radius,
                        Mathf.Sin(s * Mathf.PI * 2 / ringSegments) * radius,
                        w * wPerSegment
                    ),
                    Quaternion.identity);
                tileO.transform.Rotate(new Vector3(0, 0, s * 360.0f / ringSegments + 90));
                grid[s, w] = tileO;
                GameObject modelO = Instantiate(type.models[Random.Range(0, type.models.Count)]);
                modelO.name = "model";
                modelO.transform.SetParent(tileO.transform, false);
                if (special != null)
                {
                    int specialModelIndex = Random.Range(0, special.activeModels.Count);
                    GameObject specialModelO = Instantiate(special.activeModels[specialModelIndex]);
                    specialModelO.name = "specialModelActive";
                    specialModelO.transform.SetParent(tileO.transform, false);
                    if (special.models.Count > 0)
                    {
                        GameObject specialModelInactiveO = Instantiate(special.models[specialModelIndex]);
                        specialModelInactiveO.name = "specialModelInactive";
                        specialModelInactiveO.transform.SetParent(tileO.transform, false);
                        specialModelInactiveO.transform.FindChild("default").GetComponent<Renderer>().enabled = false;
                    }
                    tileO.GetComponent<Tile>().specialModelIndex = specialModelIndex;
                    tileO.GetComponent<Tile>().special = special;
                }
                tileO.GetComponent<Tile>().s = s;
                tileO.GetComponent<Tile>().w = w;
                tileO.GetComponent<Tile>().type = type;
                tileO.GetComponent<Tile>().tribe = tribe;

                if (w == tribeW && s == tribeS)
                {
                    tribe.GetComponent<Tribe>().tile = tileO;
                    tribe.transform.SetPositionAndRotation(tileO.transform.position, tileO.transform.rotation);
                }
            }
        }

        for (int s = 0; s < ringSegments; s++)
        {
            for (int w = 0; w < widthSegments; w++)
            {
                Tile t = grid[s, w].GetComponent<Tile>();
                if (w > 0)
                {
                    t.wMinus = grid[s, w - 1];
                }
                if (w < widthSegments - 1)
                {
                    t.wPlus = grid[s, w + 1];
                }
                t.sPlus = grid[(s + 1) % ringSegments, w];
                t.sMinus = grid[(s + ringSegments - 1) % ringSegments, w];
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
