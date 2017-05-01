using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public Material highlightMaterial;
    private Material originalMaterial;

    public int s, w;
    public GameObject sPlus, sMinus, wPlus, wMinus;
    public TileType type;
    public SpecialType special;
    public bool specialActive = true;
    public int specialModelIndex;
    public GameObject tribe;
    public Dictionary<string, int> properties = new Dictionary<string, int>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (tribe.GetComponent<Tribe>().CanMoveTo(this))
        {
            tribe.GetComponent<Tribe>().MoveTo(this);
        }
    }

    private void OnMouseEnter()
    {
        if (!tribe.GetComponent<Tribe>().CanMoveTo(this))
        {
            return;
        }
        Renderer r = transform.FindChild("model").FindChild("default").GetComponent<Renderer>();
        if (r.material != highlightMaterial)
        {
            originalMaterial = r.material;
            r.material = highlightMaterial;
        }
    }

    private void OnMouseExit()
    {
        Renderer r = transform.FindChild("model").FindChild("default").GetComponent<Renderer>();
        if (originalMaterial != null)
        {
            r.material = originalMaterial;
        }
    }
}
