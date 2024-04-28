using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using UnityEngine.Android;
using System.Linq;
using System;
using Whisper.Utils;

public class UIController : MonoBehaviour
{

    private VisualElement _bottomContainer;
    private Button _proceedButton;
    private Button _closeButton;
    private VisualElement _overlapSheet;
    // private VisualElement _scrim;
    private VisualElement _imageContainer;
    private Label _snellenLabel;
    private AudioSource _audioData;
    //private bool _beingHandled = false;
    private VoiceRecog VoiceRecogScript;
    private Label _spokenTextBox;
    private Label _actuityScore;
    private Label _scoreLabel;
    public Button _startButton;
    private int error;
    [SerializeField] private AudioClip StartInstructions;
    
    

    private char older;
    //benchmarking text size for scaling
    private const int size = 750;
    //pointer to keep track of the current snellen chart letter
    private int pointer = 1;

    private int errorFlag = 0;

    private string rowNumber = "A0";
    private bool GameState = true;
    //snellen chart mapping
    private Dictionary<string, List<double>> snellenMapping = new Dictionary<string, List<double>>()
    {   
        { "A0", new List<double>{ 1.0, 200.0 } },
        { "E1", new List<double> { 1.0, 200.0 } },
        { "F2", new List<double> { 0.48, 100.0 } },
        { "P2", new List<double> { 0.48, 100.0 } },
        { "T3", new List<double> { 0.42, 70.0 } },
        { "O3", new List<double> { 0.42, 70.0 } },
        { "Z3", new List<double> { 0.42, 70.0 } },
        { "L4", new List<double> { 0.24, 50.0 } },
        { "P4", new List<double> { 0.24, 50.0 } },
        { "E4", new List<double> { 0.24, 50.0 } },
        { "D4", new List<double> { 0.24, 50.0 } },
        { "P5", new List<double> { 0.18, 40.0 } },
        { "E5", new List<double> { 0.18, 40.0 } },
        { "C5", new List<double> { 0.18, 30.0 } },
        { "F5", new List<double> { 0.18, 40.0 } },
        { "D5", new List<double> { 0.18, 40.0 } },
        { "E6", new List<double> { 0.15, 30.0 } },
        { "D6", new List<double> { 0.15, 30.0 } },
        { "F6", new List<double> { 0.15, 30.0 } },
        { "C6", new List<double> { 0.15, 30.0 } },
        { "Z6", new List<double> { 0.15, 30.0 } },
        { "P6", new List<double> { 0.15, 30.0 } },
        { "F7", new List<double> { 0.12, 25.0 } },
        { "E7", new List<double> { 0.12, 25.0 } },
        { "L7", new List<double> { 0.12, 25.0 } },
        { "O7", new List<double> { 0.12, 25.0 } },
        { "P7", new List<double> { 0.12, 25.0 } },
        { "Z7", new List<double> { 0.12, 25.0 } },
        { "D7", new List<double> { 0.12, 25.0 } },
        { "D8", new List<double> { 0.09, 20.0 } },
        { "E8", new List<double> { 0.09, 20.0 } },
        { "F8", new List<double> { 0.09, 20.0 } },
        { "P8", new List<double> { 0.09, 20.0 } },
        { "O8", new List<double> { 0.09, 20.0 } },
        { "T8", new List<double> { 0.09, 20.0 } },
        { "E82", new List<double> { 0.09, 20.0 } },
        { "C8", new List<double> { 0.09, 20.0 } }
    };

    // Start is called before the first frame update
    void Start()
    {
        older = ' ';
        Debug.Log("Number of devices found: " + Microphone.devices.Length);
        VoiceRecogScript = GetComponent<VoiceRecog>();
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        _audioData = GetComponent<AudioSource>();
        var root = GetComponent<UIDocument>().rootVisualElement;

        _bottomContainer = root.Q<VisualElement>("bottomContainer");
        _proceedButton = root.Q<Button>("proceedButton");
        _closeButton = root.Q<Button>("closeButton");
        _overlapSheet = root.Q<VisualElement>("overlapSheet");
        // _scrim = root.Q<VisualElement>("scrim");
        _imageContainer = root.Q<VisualElement>("imageContainer");
        _snellenLabel = root.Q<Label>("snellenLabel");
        _spokenTextBox = root.Q<Label>("spokenText");
        _scoreLabel = root.Q<Label>("scoreLabel");
        _startButton = root.Q<Button>("startButton");
        _actuityScore = root.Q<Label>("ActuityScore");
        
        // _bottomContainer.style.display = DisplayStyle.None;
        _proceedButton.RegisterCallback<ClickEvent>(OnProceedButtonClicked);
        _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
        // _overlapSheet.RegisterCallback<TransitionEndEvent>(OnOverLapSheetDown);
        // _nextButton.RegisterCallback<ClickEvent>(OnNextButtonClicked);
        
        // to start from the test directly
        _bottomContainer.style.display = DisplayStyle.Flex;
        _overlapSheet.AddToClassList("bottomSheet--up");
        // _scrim.AddToClassList("scrim--fadein");
        
        // _snellenLabel.text = snellenMapping.ElementAt(0).Key.Substring(0, 1);
        // _scoreLabel.text = string.Format("Your Current Score is 20/{0}",snellenMapping.ElementAt(0).Value[1].ToString());

        rowNumber = "A0";
        _audioData.clip = StartInstructions;
        _audioData.Play();

    }

    //event handlers

    //proceed button(main screen eye test) clicked

    private void OnProceedButtonClicked(ClickEvent evt)
    {
        _bottomContainer.style.display = DisplayStyle.Flex;
        _overlapSheet.AddToClassList("bottomSheet--up");
        // _scrim.AddToClassList("scrim--fadein");
        _snellenLabel.text = snellenMapping.ElementAt(0).Key.Substring(0, 1);
        _scoreLabel.text = string.Format("Your Current Score is 20/{0}",snellenMapping.ElementAt(0).Value[1].ToString());
        _actuityScore.text = string.Format("Actuity Score: 20/{0}",snellenMapping.ElementAt(0).Value[1].ToString());
        rowNumber = "A0";
        Debug.Log("Row number on proceed button click: " + rowNumber);

    }

    //close button(snellen chart screen closing) clicked
    private void OnCloseButtonClicked(ClickEvent evt)
    {
        _bottomContainer.style.display = DisplayStyle.None;
        _overlapSheet.RemoveFromClassList("bottomSheet--up");
        // _scrim.RemoveFromClassList("scrim--fadein");
        pointer = 1;
        _snellenLabel.text = "";
        _snellenLabel.style.fontSize = size;
    }

    //snellen chart screen closing animation
    private void OnOverLapSheetDown(TransitionEndEvent evt)
    {
        if (!_overlapSheet.ClassListContains("bottomSheet--up"))
        {
            _bottomContainer.style.display = DisplayStyle.None;
        }
    }
    
    //next button clicked when viewing letters
    private void OnNextButtonClicked(ClickEvent evt)
    {

        GoNext();
    }

    private void GoNext()
    {
        if (pointer < snellenMapping.Count)
        {
            pointer++;
            _snellenLabel.text = snellenMapping.ElementAt(pointer - 1).Key.Substring(0, 1);
            _snellenLabel.style.fontSize = (int)(snellenMapping.ElementAt(pointer - 1).Value[0] * size);

            var currentRow = snellenMapping.ElementAt(pointer - 1).Key.Substring(1, 1);
            if (currentRow != rowNumber.Substring(1, 1))
            {
                _scoreLabel.text = string.Format("Your Current Score is 20/{0}", snellenMapping[rowNumber][1].ToString());
                _actuityScore.text = string.Format("Actuity Score: 20/{0}",snellenMapping.ElementAt(0).Value[1].ToString());
                rowNumber = snellenMapping.ElementAt(pointer - 1).Key;
                errorFlag = 1;
            }
            else if (snellenMapping.ElementAt(pointer - 1).Key == "C8")
            {
                _scoreLabel.text = string.Format("Your Current Score is 20/20. Congratulations!!!");
                _actuityScore.text = "Congratulations! You have actuity Score of  20/20";

            }
            else
            {
                errorFlag = 0;
            }


        }
    }

    private void EndGame()
    {
        Debug.Log("Game is Over");
        _bottomContainer.style.display = DisplayStyle.None;
        _overlapSheet.RemoveFromClassList("bottomSheet--up");
        // _scrim.RemoveFromClassList("scrim--fadein");
        pointer = 1;
        _snellenLabel.text = "";
        _snellenLabel.style.fontSize = size;
        GameState = false;

    }

    private void Update()
    {
        if (GameState)
        {
            if (error > 2)
            {
                EndGame();
            }

            if (VoiceRecogScript.response != older)
            {
                if (snellenMapping.ElementAt(pointer - 1).Key.Substring(0, 1)[0] == VoiceRecogScript.response)
                {
                    if (errorFlag == 1)
                    {
                        error = 0;
                    }

                    GoNext();
                }
                else
                {
                    error += 1;
                    Debug.Log("Wrong");
                }


                older = VoiceRecogScript.response;
            }
        }
        // if (!_beingHandled)
        // {
        //     StartCoroutine(Listen());
        // }
        //
        // if (VoiceRecogScript.response != ' ')
        // {
        //     char epic = VoiceRecogScript.response;
        //     Debug.Log("Comparing Against: " + snellenMapping.ElementAt(pointer - 1).Key.Substring(0, 1)[0] + "With " + epic);

        //     else
        //     {
        //         Debug.Log("WRONG");
        //     }
        //     VoiceRecogScript.response = ' ';
        // }
    }
    
    // private IEnumerator Listen()
    // {
    //     _beingHandled = true;
    //     // yield return new WaitForSeconds(2f);
    //     Debug.Log("This Script is running");
    //     // _startButton.SendEvent(ClickEvent);
    //     VoiceRecogScript.StartRecording();
    //     yield return new WaitForSeconds(5f);
    //     VoiceRecogScript.StopRecording();
    //         // _startButton.SendEvent(e);
    //     yield return new WaitForSeconds(4f);
    //     // Debug.Log("Done Waiting");
    //     _beingHandled = false;
    // }
}