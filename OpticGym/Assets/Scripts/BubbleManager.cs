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
    BoxCollider collider;
    [SerializeField]
    AudioSource audioSource;

    void Start()
    {
        Invoke("DetectStart", 1);
        Invoke("TimeOut", 20.999f);
        audioSource = GetComponent<AudioSource>();
  
    }

    // Update is called once per frame
    void Update()
    {
        /*gameManager.SendMessage("bubbleTimeOut", gameObject);*/
        /* transform.position = Vector3.MoveTowards(transform.position, target, step);*/
        float xStep = Mathf.Sin(randAngle) * Time.deltaTime * speed;
        float yStep = Mathf.Cos(randAngle) * Time.deltaTime * speed;
        if (!detecting)
        {
            //transform.position = new Vector3(transform.position.x + xStep, transform.position.y + yStep, transform.position.z);

            transform.position = new Vector3(transform.position.x,
                transform.position.y + 0.01f,
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
        detecting = true;
        collider.size = new Vector3(3, 3, 3);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (detecting) {
            Debug.Log("Collissio Happened. We love you!");

            Pop();
        }
    }
    IEnumerator PopRoutine()
    {
        audioSource.Play();
        yield return new WaitUntil(() => audioSource.time >= audioSource.clip.length);
        gameManager.SendMessage("bubblePopped", gameObject);
        Destroy(gameObject);
    }

}