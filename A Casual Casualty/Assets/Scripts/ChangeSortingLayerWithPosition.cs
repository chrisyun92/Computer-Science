using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSortingLayerWithPosition : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;

    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
    }
	
	void FixedUpdate () {
        // Sprites with higher position.y values render on a lower order.
        spriteRenderer.sortingOrder = 15 * 5 - ((int)(5 * rigidbody.position[1]));
    }
}
