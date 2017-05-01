using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tribe : MonoBehaviour {

    public EventManager events;
    public GameObject prevTile;
    public GameObject tile;
    private float cooldown = 0;
    public float cooldownAmt = 0.35f;
    public float moveTime = 0.2f;
    public Dictionary<string, int> properties = new Dictionary<string, int>();
    public bool doRestart;
    public float restartDelay = 5;
    private float restartDelayAmt = 0;

	// Use this for initialization
	void Start () {
        properties["#Might"] = 3;
        properties["#Hope"] = 3;
        properties["#Food"] = 10;
        properties["#People"] = 17;
        properties[".t"] = 0;
        properties[".t2"] = 0;
    }

    private void PostMove (float rot)
    {
        transform.SetPositionAndRotation(prevTile.transform.position, tile.transform.rotation);
        transform.FindChild("tribe").transform.localRotation = Quaternion.Euler(0, rot, 0);
        cooldown = cooldownAmt;
        properties[".t"]++;
    }

    public bool CanMoveTo(Tile t2)
    {
        Tile t = tile.GetComponent<Tile>();
        if (t.type.blocker) { return false; }
        if (t.sPlus.GetComponent<Tile>() == t2) { return true;  }
        if (t.sMinus.GetComponent<Tile>() == t2) { return true; }
        if (t.wPlus != null && t.wPlus.GetComponent<Tile>() == t2) { return true; }
        if (t.wMinus != null && t.wMinus.GetComponent<Tile>() == t2) { return true; }
        return false;
    }

    public void MoveTo(Tile t2)
    {
        if (cooldown > 0) { return; }
        if (t2.type.blocker) { return; }
        Tile t = tile.GetComponent<Tile>();
        if (t.sPlus.GetComponent<Tile>() == t2)
        {
            Evt e = events.FindEvent(gameObject, t2.gameObject);
            if (e != null)
            {
                events.Run(e, gameObject, t2.gameObject);
            }
            else
            {
                prevTile = tile;
                tile = t.sPlus;
                PostMove(270);
            }
            return;
        }
        if (t.sMinus.GetComponent<Tile>() == t2)
        {
            Evt e = events.FindEvent(gameObject, t2.gameObject);
            if (e != null)
            {
                events.Run(e, gameObject, t2.gameObject);
            }
            else
            {
                prevTile = tile;
                tile = t.sMinus;
                PostMove(90);
            }
            return;
        }
        if (t.wPlus != null && t.wPlus.GetComponent<Tile>() == t2)
        {
            Evt e = events.FindEvent(gameObject, t2.gameObject);
            if (e != null)
            {
                events.Run(e, gameObject, t2.gameObject);
            }
            else
            {
                prevTile = tile;
                tile = t.wPlus;
                PostMove(180);
            }
            return;
        }
        if (t.wMinus != null && t.wMinus.GetComponent<Tile>() == t2)
        {
            Evt e = events.FindEvent(gameObject, t2.gameObject);
            if (e != null)
            {
                events.Run(e, gameObject, t2.gameObject);
            }
            else
            {
                prevTile = tile;
                tile = t.wMinus;
                PostMove(0);
            }
            return;
        }
    }

    void FixedUpdate()
    {
        cooldown -= Time.deltaTime;
        if (prevTile == null)
        {
            prevTile = tile;
        }
        transform.position = Vector3.Slerp(prevTile.transform.position, tile.transform.position, Mathf.Clamp(1 - (cooldown + moveTime - cooldownAmt) / moveTime, 0, 1));
    }

    // Update is called once per frame
    void Update () {
        if (doRestart)
        {
            restartDelayAmt += Time.deltaTime;
            if (restartDelayAmt >= restartDelay)
            {
                SceneManager.LoadScene("Scene2");
            }
            return;
        }
        if (cooldown > 0) { return; }
        if (events.currentEvent != null) { return; }
        Tile t = tile.GetComponent<Tile>();
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) { return; }
        Vector2 inputVec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        inputVec = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * inputVec;
		if (Mathf.Abs(Vector2.Angle(new Vector2(1, 0), inputVec)) < 46 && t.sPlus)
        {
            MoveTo(t.sPlus.GetComponent<Tile>());
            return;
        }
        if (Mathf.Abs(Vector2.Angle(new Vector2(-1, 0), inputVec)) < 46 && t.sMinus)
        {
            MoveTo(t.sMinus.GetComponent<Tile>());
            return;
        }
        if (Mathf.Abs(Vector2.Angle(new Vector2(0, 1), inputVec)) < 46 && t.wPlus)
        {
            MoveTo(t.wPlus.GetComponent<Tile>());
            return;
        }
        if (Mathf.Abs(Vector2.Angle(new Vector2(0, -1), inputVec)) < 46 && t.wMinus)
        {
            MoveTo(t.wMinus.GetComponent<Tile>());
            return;
        }
    }
}
