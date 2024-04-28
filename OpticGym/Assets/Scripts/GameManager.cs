using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;

using EyePositionStates = CustomEyeData.EyePositionStates;

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

    List<EyePositionStates> eyeDestinations;

    bool detecting = false;

    FixationPoint2DCoords fixationScript;

    // Start is called before the first frame update
    void Start()
    {
        arFaceManager = FindObjectOfType<ARFaceManager>();
        arFaceManager.facesChanged += OnFacesChanged;
        scoreBoard = GameObject.Find("ScoreBoard");

        eyeDestinations = new List<EyePositionStates>() {EyePositionStates.MiddleTop,
            EyePositionStates.MiddleRight, EyePositionStates.BottomLeft};
    }

    // Update is called once per frame
    void Update()
    {
        if(detecting && fixationScript)
        {
            var isCorrectDirection = fixationScript.IsFixationAt(eyeDestinations[currBubblePos]);
            if (isCorrectDirection)
            {
                Debug.Log("correct eyes state. need to go next");
                detecting = false;
                SpawnBubble(arFace);
            }
        }
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added.Count == 1)
        {
            arFace = args.added[0];
            //canvas.SetActive(false);
            scoreBoard.SetActive(true);
            SpawnBubble(arFace);

            fixationScript = arFace.GetComponent<FixationPoint2DCoords>();

        }

        else if (args.removed.Count == 1)
        {
            arFace = null;
            fixationScript = null;
            scoreBoard.SetActive(false);
        }


    }

    void SpawnBubble(ARFace face)
    {

        if (face == null)
            return; // no AR face

        var camera = Camera.main;

        var zCoord = camera.nearClipPlane + 0.1f;

        var spawnPosition = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, zCoord));

        Debug.Log(currBubblePos);
        GameObject bubble = Instantiate(bubblePrefab,spawnPosition,Quaternion.Euler(new Vector3(0, 0, 0)));

        bubblesAdded++;

        scoreBoard.GetComponent<TextMeshProUGUI>().text = "Score: " + bubblesPopped + "/" + bubblesAdded;
        bubble.GetComponent<BubbleManager>().gameManager = gameObject;
        var destination = eyeDestinations[currBubblePos++];
        var direction = CustomEyeData.EyeStateToPositions[destination];
        bubble.GetComponent<BubbleManager>().direction = new List<int>() { direction[0], direction[1] };
        currBubblePos %= eyeDestinations.Count;

    }

    public void bubbleLeft()
    {
        detecting = true;
    }

}
