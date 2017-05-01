using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows : MonoBehaviour {

    public Vector3 startLookAt = new Vector3(0, 500, 0);
    public Vector3 startLocation = new Vector3(1000, 1000, 1000);
    public float startZoomSpeed = 0.3f;
    public float startZoom = -0.5f;
    public Vector3 baseLocation = new Vector3(0, 30, -200);
    public float moveIn = 0.5f;
    public float detailZoomSpeed = 0.5f;
    public Vector3 detailBaseLocation = new Vector3(0, 0, 0);
    public float detailMoveIn = 0.9f;
    private GameObject detailTarget;
    private float detailZoomAmt = 0;

    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        startZoom += Time.deltaTime * startZoomSpeed;
        if (startZoom < 1)
        {
            transform.position =
                Vector3.Lerp(
                    startLocation,
                    Vector3.Lerp(baseLocation, target.transform.position, moveIn),
                    startZoom);
            transform.LookAt(Vector3.Lerp(
                startLookAt,
                target.transform.position,
                startZoom
                ));
            return;
        }
        if (target.GetComponent<Tribe>().events.currentEvent != null)
        {
            detailTarget = target.GetComponent<Tribe>().events.currentEventTileO;
            detailZoomAmt += Time.deltaTime * detailZoomSpeed;
        }
        else
        {
            detailZoomAmt -= Time.deltaTime * detailZoomSpeed;
        }
        if (detailZoomAmt < 0)
        {
            detailTarget = null;
            detailZoomAmt = 0;
        }
        if (detailZoomAmt > 1)
        {
            detailZoomAmt = 1;
        }
        if (detailTarget == null)
        {
            transform.position = Vector3.Lerp(baseLocation, target.transform.position, moveIn);
            transform.LookAt(target.transform);
        }
        else
        {
            transform.position =
                Vector3.Lerp(
                    Vector3.Lerp(baseLocation, target.transform.position, moveIn),
                    Vector3.Lerp(detailBaseLocation, detailTarget.transform.position, detailMoveIn),
                    detailZoomAmt);
            transform.LookAt(Vector3.Lerp(target.transform.position, detailTarget.transform.position, detailZoomAmt));
        }
	}
}
