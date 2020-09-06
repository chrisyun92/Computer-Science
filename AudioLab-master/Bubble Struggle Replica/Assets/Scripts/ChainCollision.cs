using UnityEngine;

public class ChainCollision : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col)
	{
		Chain.IsFired = false;

        Chain.laserNoise = true;
		if (col.tag == "Ball")
		{
			col.GetComponent<Ball>().Split();
		}
	}

}
