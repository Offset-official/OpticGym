using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using UnityEngine.Android;
using System.Linq;
public class UIController : MonoBehaviour
{

    private VisualElement _bottomContainer;
    private Button _proceedButton;
    private Button _closeButton;
    private VisualElement _overlapSheet;
    private VisualElement _scrim;
    private VisualElement _imageContainer;
    private Label _snellenLabel;
    private Button _nextButton;
    private AudioSource _audioData;
    private bool _beingHandled = false;
    private Button _startButton;
    private Button _stopButton;
    private VoiceRecog VoiceRecogScript;
    private Label _spokenTextBox;
    private Label _scoreLabel;
    
    //benchmarking text size for scaling
    private const int size = 750;
    //pointer to keep track of the current snellen chart letter
    private int pointer = 1;

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
        _scrim = root.Q<VisualElement>("scrim");
        _imageContainer = root.Q<VisualElement>("imageContainer");
        _snellenLabel = root.Q<Label>("snellenLabel");
        _nextButton = root.Q<Button>("nextButton");
        _startButton = root.Q<Button>("startButton");
        _stopButton = root.Q<Button>("stopButton");
        _spokenTextBox = root.Q<Label>("spokenText");
        _scoreLabel = root.Q<Label>("scoreLabel");
        
        // _bottomContainer.style.display = DisplayStyle.None;
        _proceedButton.RegisterCallback<ClickEvent>(OnProceedButtonClicked);
        _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
        _overlapSheet.RegisterCallback<TransitionEndEvent>(OnOverLapSheetDown);
        _nextButton.RegisterCallback<ClickEvent>(OnNextButtonClicked);
        _startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        _stopButton.RegisterCallback<ClickEvent>(OnStopButtonClicked);
        
        _stopButton.SetEnabled(false);
        
        // to start from the test directly
        _bottomContainer.style.display = DisplayStyle.Flex;
        _overlapSheet.AddToClassList("bottomSheet--up");
        _scrim.AddToClassList("scrim--fadein");
        _snellenLabel.text = snellenMapping.ElementAt(0).Key.Substring(0, 1);
        _scoreLabel.text = string.Format("Your Current Score is 20/{0}",snellenMapping.ElementAt(0).Value[1].ToString());

    }

    //event handlers

    //proceed button(main screen eye test) clicked
    
    private void OnProceedButtonClicked(ClickEvent evt)
    {
        _bottomContainer.style.display = DisplayStyle.Flex;
        _overlapSheet.AddToClassList("bottomSheet--up");
        _scrim.AddToClassList("scrim--fadein");
        _snellenLabel.text = snellenMapping.ElementAt(0).Key.Substring(0, 1);
        _scoreLabel.text = string.Format("Your Current Score is 20/{0}",snellenMapping.ElementAt(0).Value[1].ToString());
        
    }

    //close button(snellen chart screen closing) clicked
    private void OnCloseButtonClicked(ClickEvent evt)
    {
        _overlapSheet.RemoveFromClassList("bottomSheet--up");
        _scrim.RemoveFromClassList("scrim--fadein");
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
        if (pointer < snellenMapping.Count)
        {
            pointer++;
            _snellenLabel.text = snellenMapping.ElementAt(pointer - 1).Key.Substring(0, 1);
            _snellenLabel.style.fontSize = (int)(snellenMapping.ElementAt(pointer - 1).Value[0] * size);
            _scoreLabel.text = string.Format("Your Current Score is 20/{0}",snellenMapping.ElementAt(pointer - 2).Value[1].ToString());

        }
    }

    private void OnStartButtonClicked(ClickEvent evt)
    {
        VoiceRecogScript.StartRecording();
    }

    private void OnStopButtonClicked(ClickEvent evt)
    {
        VoiceRecogScript.StopRecording();
    }
    private void Update()
    {
        if (!_beingHandled)
        {
            StartCoroutine(Listen());
        }
    }
    
    private IEnumerator Listen()
    {
        _beingHandled = true;
        // Debug.Log("This Script is running");
        // VoiceRecogScript.StartRecording();
        yield return new WaitForSeconds(0.25f);
        // VoiceRecogScript.StopRecording();
        // yield return new WaitForSeconds(10f);
        _beingHandled = false;
    }
}
