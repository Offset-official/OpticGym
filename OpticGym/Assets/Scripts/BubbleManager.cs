using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class BubbleManager : MonoBehaviour
{
    // Start is called before the first frame update

    float time;
    float lapsed;
    [SerializeField]
    public GameObject gameManager;
    public float randAngle;
    float speed = 0.1f;
    bool detecting = false;
    // BoxCollider collider;
    [SerializeField]
    AudioSource audioSource;
    int randomHor;
    int randomVer;
    bool popping = false;
    List<List<int>> orientations = new List<List<int>> {
        new List<int> { 0, 1 },
        new List<int> { 1, 0 },
        new List<int> { -1, 0 },
        new List<int> { 0, -1 } };

    void Start()
    {
        Invoke("DetectStart", 4f);
        Invoke("TimeOut", 20.999f);
        audioSource = GetComponent<AudioSource>();
        // collider = GetComponent<BoxCollider>();
        List<int> orientation = orientations[Random.Range(0, orientations.Count)];
        randomHor = orientation[0];
        randomVer = orientation[1];

    }

    // Update is called once per frame
    void Update()
    {
        /*gameManager.SendMessage("bubbleTimeOut", gameObject);*/
        /* transform.position = Vector3.MoveTowards(transform.position, target, step);*/
        float xStep = Mathf.Sin(randAngle) * Time.deltaTime * speed;
        float yStep = Mathf.Cos(randAngle) * Time.deltaTime * speed;
        if (!detecting && !popping)
        {
            //transform.position = new Vector3(transform.position.x + xStep, transform.position.y + yStep, transform.position.z);

            transform.position = new Vector3(transform.position.x + (0.005f * randomHor),
                transform.position.y + (0.005f * randomVer),
                transform.position.z);
        }


    }


    void Pop()
    {

        StartCoroutine(PopRoutine());

    }
    void TimeOut()
    {
        gameManager.SendMessage("bubbleTimeOut", gameObject);
        Destroy(gameObject);
    }
    void DetectStart()
    {
        Debug.Log("Detecting!");
        detecting = true;
        // collider.size = new Vector3(10, 10, 10);

    }
    void OnTriggerEnter(Collider other)
    {
        if (detecting && !popping)
        {
            Debug.Log("Collided!");
            // Pop();
        }
    }
    IEnumerator PopRoutine()
    {
        detecting = false;
        popping = true;
        audioSource.Play();
        yield return new WaitUntil(() => audioSource.time >= audioSource.clip.length);
        gameManager.SendMessage("bubblePopped", gameObject);
        Destroy(gameObject);
    }

}