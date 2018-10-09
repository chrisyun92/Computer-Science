using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour {

    public bool mouseControl = false;
    Dragable mouseObject = null;
    Dictionary<int, Dragable> origins = new Dictionary<int, Dragable>();

    private bool spacerDetected = false;
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Input.touchCount; i += 1) {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began) {
                Dragable follower = GetTouchedObject(touch.position);
                if (follower != null) {
                    if (!follower.amISpacer())
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);
                        follower.StartDrag(pos);
                        origins.Add(touch.fingerId, follower); // Get object at mouse location
                    } else
                    {
                        spacerDetected = true;
                    }
                }
            }

            if (origins.ContainsKey(touch.fingerId)) {
                if (touch.phase == TouchPhase.Ended) {
                    origins[touch.fingerId].FinishDrag(); // Tell the object that a drag has finished
                    if (spacerDetected)
                    {
                        origins[touch.fingerId].gameObject.GetComponent<Spacer>().SpawnRNA();
                    }
                    spacerDetected = false;
                    origins.Remove(touch.fingerId);
                }

                if (touch.phase == TouchPhase.Moved) {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);
                    origins[touch.fingerId].Drag(pos);
                } 
            }
        }
        if (mouseControl) {
            if (Input.GetMouseButtonDown(0)) {
                mouseObject = GetTouchedObject(Input.mousePosition);
                if (mouseObject != null) {
                    if (!mouseObject.amISpacer())
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mouseObject.StartDrag(pos);
                    } else
                    {
                        spacerDetected = true;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (mouseObject != null) {
                    mouseObject.FinishDrag();
                    if (spacerDetected)
                    {
                        mouseObject.gameObject.GetComponent<Spacer>().SpawnRNA();
                    }
                    spacerDetected = false;
                    mouseObject = null;
                }
            }

            if (mouseObject != null) {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseObject.Drag(pos);
            }
        }
    }

    private Dragable GetTouchedObject(Vector2 position) {
        RaycastHit2D[] hits;
        Ray ray = Camera.main.ScreenPointToRay(position);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue);

        hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
        foreach (RaycastHit2D hit in hits) {
            Dragable follower = hit.collider.gameObject.GetComponent<Dragable>();
            if (follower != null) {
                return follower;
            }
        }
        return null;
    }
}
