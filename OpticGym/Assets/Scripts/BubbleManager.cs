using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{

    public GameObject gameManager;
    [SerializeField]
    float speed = 0.07f;

    public List<int> direction = new List<int> { 0, 0 };

    private Camera mainCamera;

    bool borderCheck = false;

    void Start()
    {
        mainCamera = Camera.main;
        Invoke("StartBorderCheck", 2);

    }

    // Update is called once per frame
    void Update()
    {

        var myViewportCoords = mainCamera.WorldToViewportPoint(transform.position);
        // The camera texture is mirrored so x and y must be changed to match where the fixation point is in relation to the face.
        var mirrorFixationInView = new Vector3(1 - myViewportCoords.x, 1 - myViewportCoords.y, myViewportCoords.z);

        if (borderCheck && (mirrorFixationInView.x <= 0 || mirrorFixationInView.x >= 1 || mirrorFixationInView.y <= 0 || mirrorFixationInView.y >= 1))
        {
            StartCoroutine(DestroyCoroutine());
        }


        transform.position = new Vector3(transform.position.x + (direction[0] * speed),
            transform.position.y + (direction[1] * speed),
            transform.position.z);
    }
    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        gameManager.GetComponent<GameManager>().bubbleLeft();

        Destroy(gameObject);
    }
    void StartBorderCheck()
    {
        borderCheck = true;
    }

}