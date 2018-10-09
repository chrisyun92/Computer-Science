using UnityEngine;

[RequireComponent(typeof(TargetJoint2D))]
public class Dragable : MonoBehaviour {

    TargetJoint2D tj;

    public bool amSpacer = false;

    private bool draggable = true;

    private void Start() {
        tj = GetComponent<TargetJoint2D>();
        tj.enabled = false;
    }

    public void StartDrag(Vector2 point) {
        if (draggable)
        {
            Vector2 delta = point - new Vector2(transform.position.x, transform.position.y);
            tj.anchor = delta;
        }
    }

    public void Drag(Vector2 point) {
        if (draggable)
        {
            tj.enabled = true;
            tj.target = point;
        }
    }

    public void FinishDrag() {
        tj.anchor = Vector2.zero;
        tj.enabled = false;
    }

    public void MakeUndraggable()
    {
        tj.enabled = false;
        draggable = false;
    }

    public void MakeDraggable()
    {
        draggable = true;
    }

    public bool amISpacer()
    {
        return amSpacer;
    }
}
 