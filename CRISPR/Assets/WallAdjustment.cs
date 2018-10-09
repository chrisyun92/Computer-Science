using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class WallAdjustment : MonoBehaviour {

    public BoxCollider2D colliderOne;
    public BoxCollider2D colliderTwo;

	// Use this for initialization
	void Start () {
        BoxCollider2D[] array = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in array)
        {
            if (collider.offset.x < 0)
            {
                colliderOne = collider;
            } else
            {
                colliderTwo = collider;
            }
        }
#if UNITY_IOS
        if ((UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPad") > -1)
        {
            colliderOne.offset = new Vector2(-4.4f, -2.6f);
            colliderTwo.offset = new Vector2(4.4f, -2.6f);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            colliderOne.offset = new Vector2(-4.8f, -2.6f);
            colliderTwo.offset = new Vector2(4.8f, -2.6f);
        }
#endif
#if UNITY_ANDROID
        colliderOne.offset = new Vector2(-4.8f, -2.6f);
        colliderTwo.offset = new Vector2(4.8f, -2.6f);
#endif
    }
}
