using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;

using EyePositionStates = CustomEyeData.EyePositionStates;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    GameObject bubblePrefab;

    [SerializeField]
    TextMeshProUGUI scoreBoardText;

    ARFaceManager arFaceManager;
    ARFace arFace;

    int currBubblePos = 0;
    int bubblesPopped = 0;
    int bubblesAdded = 0;

    List<EyePositionStates> eyeDestinations;

    bool detecting = false;

    FixationPoint2DCoords fixationScript;

    AudioSource m_AudioSource;
    RawImage currStateSprite;

    // Start is called before the first frame update
    void Start()
    {
        arFaceManager = FindObjectOfType<ARFaceManager>();
        arFaceManager.facesChanged += OnFacesChanged;

        eyeDestinations = new List<EyePositionStates>() {EyePositionStates.MiddleTop,
            EyePositionStates.MiddleRight, EyePositionStates.BottomLeft};

        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("looking for collision");
        Debug.Log("detection: " + detecting);
        if(detecting)
        {
            Debug.Log(eyeDestinations[currBubblePos]);
            var isCorrectDirection = fixationScript.IsFixationAt(eyeDestinations[currBubblePos]);
            if (isCorrectDirection)
            {
                Debug.Log("correct eyes state. need to go next");
                currBubblePos = (currBubblePos + 1) % eyeDestinations.Count;
                bubblesPopped += 1;
                currStateSprite.color = new Color(255, 255, 255, 80);
                detecting = false;
                isCorrectDirection = false; // just in case
                PlaySoundAndSpawn();
            }
        }
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added.Count == 1)
        {
            arFace = args.added[0];
            SpawnBubble(arFace);
            fixationScript = arFace.GetComponent<FixationPoint2DCoords>();

        }

        else if (args.removed.Count == 1)
        {
            arFace = null;
            fixationScript = null;
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

        scoreBoardText.text = "Score: " + bubblesPopped + "/" + bubblesAdded;
        bubble.GetComponent<BubbleManager>().gameManager = gameObject;
        var destination = eyeDestinations[currBubblePos];
        var direction = CustomEyeData.EyeStateToPositions[destination];
        bubble.GetComponent<BubbleManager>().direction = new List<int>() { direction[0], direction[1] };
    }

    public void bubbleLeft()
    {
        Debug.Log("Bubble has left");
        detecting = true;
        Debug.Log(eyeDestinations[currBubblePos].ToString());
        currStateSprite = GameObject.Find(eyeDestinations[currBubblePos].ToString()).GetComponent<RawImage>();
        currStateSprite.color = new Color(255, 0, 0, 255);
    }

    void PlaySoundAndSpawn()
    {
        m_AudioSource.Play();
        SpawnBubble(arFace);
    }
}
