using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GiveDistance : MonoBehaviour
{
    private TextMeshProUGUI text1;
    private XROrigin XROrigin;
    private bool beingHandled = false;
    private double distance; 
    private AudioSource audioData;
    void Awake()
    {
        audioData = GetComponent<AudioSource>();
        XROrigin=GameObject.Find("XR Origin (XR Rig)").GetComponent<XROrigin>();
        text1 = GameObject.Find("displayText").GetComponent<TextMeshProUGUI>();
        text1.text = "Distance: " + 0 + "cm";
        audioData.Play();
        // Vector3 XRpos = camera1.ViewportToWorldPoint(XROrigin.transform.position);
        // Vector3 facepos = camera1.ViewportToWorldPoint(transform.position);
        // text1.text = "Distance: " + Vector3.Distance(XRpos,facepos);
        // text1.text = "Distance: " + (Vector3.Distance(XROrigin.transform.position,transform.position)-1) + "cm";
    }
    
    void Update()
    {
        // Vector3 XRpos = camera1.ViewportToWorldPoint(XROrigin.transform.position);
        // Vector3 facepos = camera1.ViewportToWorldPoint(transform.position);
        // text1.text = "Distance: " + Vector3.Distance(XRpos,facepos);
        // text1.text = "Distance: " + (Vector3.Distance(XROrigin.transform.position,transform.position) -1) + "cm";
        // text1.text = "Distance: " + Vector3.Distance(XROrigin.transform.position,transform.position) + "cm";
        // distance = Math.Truncate(((Math.Abs((797.1569 - transform.position.z)) - 0.115f) * 125));
        // text1.text = "Distance: " + distance + "cm";
        // if(distance > 275){
        //     text1.text = "8ft Reached";
        //     
        // }
        if (!beingHandled)
        {
            StartCoroutine(GetDistance());
        }
    }

    private IEnumerator GetDistance()
    {
        beingHandled = true;
        if (distance > 230)
        {
            text1.text = "8ft Reached";
            audioData.Play();
            yield return new WaitForSeconds( 5.0f );
            SceneManager.LoadScene("Voicing");
        }
        else
        {
            distance = Math.Truncate(((Math.Abs((797.1569 - transform.position.z)) - 0.115f) * 125));
            text1.text = "Distance: " + distance + "cm";
            yield return new WaitForSeconds(0.25f);
            beingHandled = false;
        }
    }
}
