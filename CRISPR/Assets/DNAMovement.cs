using System.Collections;
using UnityEngine;

public class DNAMovement : MonoBehaviour
{

    public int type = 1;
    public float speed = 10f;
    public float timerLeftTillExplosion = 20f;
    public float timeBetweenBlinks = 2.5f;
    public float delay = 1.5f;

    private Color whiteColor;
    private Color originalColor;
    private bool halfway = false;
    private float timer;
    private SpriteRenderer sp;
    private Vector2 tempF;
    private Rigidbody2D rb;
    private Cas1Cas2 script;
    private BoxCollider2D dna;
    private GameObject cas1;
    private bool grabbed = false;
    private GameObject ps;
    private BoxCollider2D bc;
    private ScoreController sc;
    private bool isTutorial = false;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        ps = transform.Find("Particles").gameObject;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        originalColor = sp.color;
        whiteColor = Color.white;
        if (GameObject.Find("ScoreController").GetComponent<ScoreController>() != null)
        {
            sc = GameObject.Find("ScoreController").GetComponent<ScoreController>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (grabbed)
        {
            dna.transform.Translate(cas1.transform.position * Time.deltaTime);
            Debug.Log("following");
        }
        if (!halfway)
        {
            if (timer > timerLeftTillExplosion - 10f)
            {
                timeBetweenBlinks = 1.5f;
                delay = 0.5f;
                halfway = true;
            }
        } else if (halfway) {
            if (timer > timerLeftTillExplosion)
            {
                if (GameObject.Find("DestroyAllZone") != null)
                {
                    GameObject.Find("DestroyAllZone").GetComponent<KillEverythingScript>().KillAll();
                }
            } else if (timer > timerLeftTillExplosion - 4f)
            {
                timeBetweenBlinks = 0.3f;
                delay = 0f;
                if (isTutorial)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collidedObject = other.gameObject;
        if (collidedObject.CompareTag("cas1cas2"))
        {
            script = other.gameObject.GetComponentInParent<Cas1Cas2>();
            if (!(script.IsGrabbing()))
            {
                if (!script.HasTransported(type))
                {
                    script.AddTypeTransported(type);
                    script.GrabOn();
                    script.SetDNA(type);
                    StartCoroutine(ParticleEffects());
                }
            }
        } else if (collidedObject.CompareTag("cas9"))
        {
            if (isTutorial)
            {
                FindObjectOfType<GameController>().InitiateNextStep();
            }
            bool killMyself = collidedObject.GetComponent<Cas9>().StartDeath(type);
            if (killMyself)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        StartCoroutine(ParticleEffects());
        sc.updateScore(100);
    }

    IEnumerator ParticleEffects()
    {
        bc.enabled = false;
        sp.enabled = false;
        ps.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        yield return null;
    }

    public int ReturnType()
    {
        return type;
    }

    public void SetForce(float x, float y)
    {
        tempF = new Vector2(x, y);
        StartCoroutine(Push());
    }

    IEnumerator Push()
    {
        yield return new WaitForSeconds(1);
        rb.AddForce(tempF);
    }

    public void SetDNA(int newType)
    {
        type = newType;
    }

    public void StartBlinking()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenBlinks);
            sp.color = whiteColor;
            yield return new WaitForSeconds(0.15f);
            sp.color = originalColor;
        }
    }

    public void SetIsTutorial()
    {
        isTutorial = true;
    }
}
