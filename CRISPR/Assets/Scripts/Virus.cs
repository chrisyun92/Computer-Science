using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DNASlot))]
[RequireComponent(typeof(Rigidbody2D))]
public class Virus : MonoBehaviour {

    public GameObject dnaPrefab;
    public Transform dnaSpawnPoint;
    public Transform leftAttachPoint;
    public Transform rightAttachPoint;
    public float attachTimeoutTime = 2;
    public float reattachTimeoutTime = 4;
    public float infectTime = 5;
    public float fadeTime = 3;
    public Color fadedColor;
    [UnityToolbag.SortingLayer]
    public int deadVirusLayer;

    private int DNAType;
    private string DNAString;
    private Vector2 force;
    private bool attached = false;
    private bool KillMe = false;
    private Rigidbody2D rb;
    private Collider2D mainCollider;
    private Collider2D leftCollider;
    private Collider2D rightCollider;
    private HingeJoint2D leftJoint;
    private HingeJoint2D rightJoint;
    private bool leftAttached = false;
    private bool rightAttached = false;
    private DNASlot slot;
    private SpriteRenderer sr;
    private Material material;
    GameObject virusHolder;
    private bool grabbed = false;
    private bool isTutorial = false;

    void Update()
    {
        if (KillMe)
        {
            StartCoroutine(Die());
        }
    }

    void Spawned(SpawnArgs args) {
        transform.parent = args.virusParent.transform;
    }

    // Use this for initialization
    void Start () {
        slot = GetComponent<DNASlot>();
        mainCollider = GetComponent<Collider2D>();
        leftCollider = leftAttachPoint.GetComponent<Collider2D>();
        rightCollider = rightAttachPoint.GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        material = sr.material;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("cell")) {
            if (!attached)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                attached = true;
                //Debug.Log("hit cell");
                StartCoroutine(Infect());
            }
            /* foreach (ContactPoint2D cp in collision.contacts) {
                if (cp.otherCollider == leftCollider) {
                    StartCoroutine(AttachLeft());
                    return;
                } else if (cp.otherCollider == rightCollider) {
                    StartCoroutine(AttachRight());
                    return;
                }
            } */
        }
    }

    public void SetForce(int xF, int yF)
    {
        Vector2 someF = new Vector2(xF, yF);
        force = someF;
    }

    public void SetTransformParent(Transform trans)
    {
        transform.parent = trans;
    }

    IEnumerator AttachLeft() {
        leftCollider.enabled = false;
        leftJoint = gameObject.AddComponent<HingeJoint2D>();
        JointMotor2D ljm = new JointMotor2D();
        ljm.motorSpeed = -200f;
        ljm.maxMotorTorque = float.NegativeInfinity;
        leftJoint.anchor = leftAttachPoint.localPosition;
        leftJoint.enableCollision = true;
        //leftJoint.useMotor = true;
        //leftJoint.motor = ljm;
        leftAttached = true;
        CheckFullyAttached();
        yield return new WaitForSeconds(attachTimeoutTime);
        StartCoroutine(DetachLeft());
    }

    IEnumerator AttachRight() {
        rightCollider.enabled = false;
        rightJoint = gameObject.AddComponent<HingeJoint2D>();
        JointMotor2D rjm = new JointMotor2D();
        rjm.motorSpeed = 200f;
        rjm.maxMotorTorque = float.PositiveInfinity;
        rightJoint.anchor = rightAttachPoint.localPosition;
        rightJoint.enableCollision = true;
        //rightJoint.useMotor = true;
        //rightJoint.motor = rjm;
        rightAttached = true;
        CheckFullyAttached();
        yield return new WaitForSeconds(attachTimeoutTime);
        StartCoroutine(DetachRight());
    }

    IEnumerator DetachLeft() {
        Destroy(leftJoint);
        leftAttached = false;
        yield return null;
        //leftCollider.enabled = true;
        yield return null;
    }

    IEnumerator DetachRight() {
        Destroy(rightJoint);
        rightAttached = false;
        //rightCollider.enabled = true;
        yield return null;
    }

    IEnumerator Infect() {
        yield return new WaitForSeconds(infectTime);
        //Change below
        GameObject.Find("SFXController").GetComponent<AudioButtonController>().Play("Pop");
        GameObject dnaStrand = transform.Find(DNAString).gameObject;
        dnaStrand.SetActive(true);
        if (isTutorial)
        {
            dnaStrand.GetComponent<DNAMovement>().SetIsTutorial();
        }
        dnaStrand.GetComponent<DNAMovement>().SetDNA(DNAType);
        yield return new WaitForSeconds(1);
        if (dnaStrand != null)
        {
            dnaStrand.GetComponent<DNAMovement>().SetForce(force.x, -1 * force.y);
            //Change below
            dnaStrand.GetComponent<DNAMovement>().StartBlinking();
            transform.Find(DNAString).transform.parent = GameObject.Find("CellScene").transform;
        }
        yield return new WaitForSeconds(1);
        Destroy(GetComponent<Flow>());
        //rb.constraints = RigidbodyConstraints2D.None;
        StartCoroutine(DetachLeft());
        StartCoroutine(DetachRight());
        //rb.AddForce(force);
        yield return new WaitForSeconds(2);
        StartCoroutine(Die());
        //Instantiate(dnaPrefab, transform);
    }

    IEnumerator Die() {
        sr.sortingLayerID = deadVirusLayer;
        float time = 0.0f;
        while (time < fadeTime) {
            time += Time.deltaTime;
            float percent = Mathf.Clamp01(time / fadeTime);
            if (material.HasProperty("_EffectAmount")) {
                float current = material.GetFloat("_EffectAmount");
                material.SetFloat("_EffectAmount", Mathf.Lerp(0, 1, percent));
                material.color = Color.Lerp(Color.white, fadedColor, percent);
            }
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield return null;
    }

    void CheckFullyAttached() {
        if (leftAttached && rightAttached) {
            StopAllCoroutines();
            StartCoroutine(Infect());
        }
    }

    //May have to change
    public void SetDNAType(int type)
    {
        DNAType = type;
        if (type == 1)
        {
            DNAString = "DNAType1";
        }
        else if (type == 2)
        {
            DNAString = "DNAType2";
        }
        else if (type == 3)
        {
            DNAString = "DNAType3";
        }
        else if (type == 4)
        {
            DNAString = "DNAType4";
        }
        else if (type == 5)
        {
            DNAString = "DNAType5";
        }
        else if (type == 6)
        {
            DNAString = "DNAType6";
        }
        else if (type == 7)
        {
            DNAString = "DNAType7";
        }
    }

    public void SetIsTutorial()
    {
        isTutorial = true;
    }
}
