using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    GameObject bubblePrefab;
    ARFaceManager arFaceManager;
    ARFace arFace;
    GameObject canvas;
    GameObject scoreBoard;
    int currBubblePos = 0;
    int bubblesPopped = 0;
    int bubblesAdded = 0;


    // Start is called before the first frame update
    void Start()
    {
        arFaceManager = FindObjectOfType<ARFaceManager>();
        arFaceManager.facesChanged += OnFacesChanged;
        canvas = GameObject.Find("FaceDetection");
        scoreBoard = GameObject.Find("ScoreBoard");
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(Time.time);*/
        if (arFace != null)
        {
            Debug.Log("Got a face!");
        }
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added.Count == 1)
        {
            arFace = args.added[0];
            canvas.SetActive(false);
            scoreBoard.SetActive(true);
            SpawnBubble(arFace);

        }

        else if (args.removed.Count == 1)
        {
            arFace = null;
            canvas.SetActive(true);
            scoreBoard.SetActive(false);
        }


    }

    void SpawnBubble(ARFace face)
    {

        if (face != null)
        {
            Transform faceChild = face.gameObject.transform.GetChild(0);
            currBubblePos = (currBubblePos + 1) % 4;
            Debug.Log(currBubblePos);
            GameObject bubble = Instantiate(bubblePrefab,
                faceChild.transform.position + new Vector3(0, 0, -0.1f),
                Quaternion.Euler(new Vector3(0, 0, 0))
                );
            /*faceChild.transform.rotation = Quaternion.Euler(new Vector3(0, currBubblePos * 90));*/
            bubblesAdded++;
            Debug.Log("Bubble spawned");
            /*            scoreBoard.GetComponent<TextMeshProUGUI>().text = "Time " + Time.time;
            */

            scoreBoard.GetComponent<TextMeshProUGUI>().text = "Score: " + bubblesPopped + "/" + bubblesAdded;
            bubble.GetComponent<BubbleManager>().gameManager = gameObject;
            float randAngle = Random.Range(0, 2 * Mathf.PI);
            bubble.GetComponent<BubbleManager>().randAngle = randAngle;


        }
    }

    void bubblePopped(GameObject bubble)
    {
        bubblesPopped++;
        SpawnBubble(arFace);


    }
    void bubbleTimeOut(GameObject bubble)
    {
        Debug.Log("GAME MANAGER RECEIVED TIMEOUT");
        SpawnBubble(arFace);
    }



}
