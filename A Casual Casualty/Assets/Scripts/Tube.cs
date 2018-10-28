using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour {

	// Killzone
    public List<GameObject> KillZone(bool kill) {
        Debug.Log("KillZone!");
        bool hit = false;
        List<GameObject> deadPlayers = new List<GameObject>();
        Vector2 topLeft = new Vector2(-19, 4);
        Vector2 botRight = new Vector2(-11, 3);
        int layermask = LayerMask.GetMask("player");
        Collider2D[] killArray = Physics2D.OverlapAreaAll(topLeft, botRight, layermask);

        // For each player in killArray, respawn.
        if (killArray != null && killArray.Length > 0) {
            for (int i = 0; i < killArray.Length; i++) {
                // Someone is in the tube.
                hit = true;
                deadPlayers.Add(killArray[i].gameObject);

                // Should we kill them?
                if (kill) {
                    GameObject player = killArray[i].gameObject;
                    player.GetComponent<PlayerScript>().RespawnPlayer();
                    Debug.Log("Kill player: " + player.name);
                }   
            }
        }

        return deadPlayers;
    }
}
