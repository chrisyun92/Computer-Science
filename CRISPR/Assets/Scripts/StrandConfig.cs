using UnityEngine;
using System.Collections;

public class StrandConfig : MonoBehaviour {

    public int segments = 5;
    public float length = 1.0f;
    public bool loop = false;
    public bool fixStart = false;
    public bool fixEnd = false;
    public GameObject segmentPrefab;
    public float segmentSpawnTime = 1f;


	// Use this for initialization
	void Start () {
        StartCoroutine(BuildStrand());
	}

    IEnumerator BuildStrand() {

        FixedJoint2D[] start = null;
        Rigidbody2D previous = null;
        GameObject g = gameObject;

        for (int i = 0; i < segments + 1; i += 1) {
            yield return new WaitForSeconds(segmentSpawnTime);
            Vector3 delta = g.transform.right.normalized * (length / segments);
            g = Instantiate(segmentPrefab, g.transform.position + delta, g.transform.rotation, transform);
            g.layer = gameObject.layer;

            FixedJoint2D[] joints = g.GetComponents<FixedJoint2D>();
            if (i == 0) {
                start = GetComponents<FixedJoint2D>();
                foreach (FixedJoint2D joint in joints) joint.enabled = false;
                if (fixStart) g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            }

            if (previous) {
                foreach (FixedJoint2D joint in joints) {
                    joint.connectedBody = previous;
                }
            }
            previous = g.GetComponent<Rigidbody2D>();

            if (i == segments) {
                if (fixEnd) {
                    previous.constraints = RigidbodyConstraints2D.FreezePosition;
                }
                
            }
        }
        SendMessage("OnFinishedCreation", SendMessageOptions.DontRequireReceiver);
    }

    private void OnDrawGizmos() {
#if UNITY_EDITOR
        Util.DrawLines(transform.position, transform.position + (transform.right * length), 0.2f);
#endif
    } 
}
