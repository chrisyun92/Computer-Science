using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class StrandRenderer : MonoBehaviour {

    LineRenderer lr;
    public float textureTilingScale = 1;
    public float slideSpeed = 1;
    public float texturePerSegment = 0.3f;
	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();
    }

    private void LateUpdate() {
        int segments = transform.childCount;
        lr.positionCount = segments;
        lr.material.mainTextureScale = new Vector2(segments * textureTilingScale, 1);
        lr.material.mainTextureOffset = lr.material.mainTextureOffset + Vector2.right * slideSpeed * Time.fixedDeltaTime;
        for (int i = 0; i < segments; i += 1) {
            Transform child = transform.GetChild(i);
            lr.SetPosition(i, child.position);
        }
        
    }
} 
