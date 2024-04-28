using System.Collections;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GiveDistance : MonoBehaviour
{
    private TextMeshProUGUI text1;
    private bool beingHandled = false;
    private double distance; 
    private AudioSource audioData;
    void Awake()
    {
        audioData = GetComponent<AudioSource>();
        text1 = GameObject.Find("displayText").GetComponent<TextMeshProUGUI>();
        text1.text = "Distance: " + 0 + "cm";
        audioData.Play();
    }
    
    void Update()
    {
        if (!beingHandled)
        {
            StartCoroutine(GetDistance());
        }
    }

    private IEnumerator GetDistance()
    {
        beingHandled = true;
        if (distance >= 180)
        {
            text1.text = "8ft Reached";
            Debug.Log("going to next scence");
            yield return new WaitForSeconds( 5.0f );
            SceneManager.LoadScene("Voicing");
        }
        else
        {
            // distance = Math.Truncate(((Math.Abs((797.1569 - transform.position.z)) - 0.115f) * 125));
            distance = (Math.Abs(transform.position.z - 0.08))*90;
            text1.text = "Distance: " + distance + "cm";
            yield return new WaitForSeconds(0.25f);
            beingHandled = false;
        }
    }
}
