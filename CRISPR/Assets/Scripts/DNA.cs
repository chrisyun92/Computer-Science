using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DNASlot))]
public class DNA : MonoBehaviour {
    Rigidbody2D rb;
    DNASlot slot;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        slot = GetComponent<DNASlot>();
        StartCoroutine(Move(3));
        StartCoroutine(Duplicate(4));
    }

    void OnDestroy() {
        // create some particle effects or something
        StopAllCoroutines();
    }

    IEnumerator Move(int num) {
        while (true) {
            yield return new WaitForSeconds(num);
            Vector2 direction = new Vector2(Random.value, Random.value);
            rb.AddForce(direction * 0.5f);
        }
    }

    IEnumerator Duplicate(int num) {
        while (true) {
            yield return new WaitForSeconds(num);
            GameObject dup = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
            dup.GetComponent<DNASlot>().DNAType(slot.DNAType());
        }
    }
}
