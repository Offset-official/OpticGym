using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;
using Whisper;
using Whisper.Utils;
using Debug = UnityEngine.Debug;


public class VoiceRecog : MonoBehaviour
    {

        public static AudioClip clip;
        public static bool recording;
        public char response;
        public Button _startButton;
        public Button _stopButton;
        private Label _spokenTextBox;
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;
        public bool streamSegments = true;
        public bool printLanguage = true;
        private string _buffer;
        private AudioSource _audioData;
        [SerializeField] private AudioClip NormalInstructions;
        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            _startButton = root.Q<Button>("startButton");
            _spokenTextBox = root.Q<Label>("spokenText");
            whisper.OnNewSegment += OnNewSegment;
            microphoneRecord.OnRecordStop += OnRecordStop;
            whisper.language = "en";
            response = 'A';
            _startButton.RegisterCallback<ClickEvent>(OnButtonPressed);
            // stopButton.onClick.AddListener(StopRecording);
            // stopButton.interactable = false;
            Debug.Log("Number of devices found: " + Microphone.devices.Length);
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        _audioData = GetComponent<AudioSource>();
        }
        
        private void OnButtonPressed(ClickEvent evt)
        {
            if (!microphoneRecord.IsRecording)
            {
                microphoneRecord.StartRecord();
                Debug.Log("Recording Started");
                _startButton.text = "Stop";
            }
            else
            {
                microphoneRecord.StopRecord();
                Debug.Log("Recording Stopped");
                _startButton.text = "Record";
            }
        }
        

        public void StartRecording()
        {
            microphoneRecord.StartRecord();
            Debug.Log("Recording Started");
            _startButton.text = "Stop";
            _spokenTextBox.style.color = Color.white;
            _spokenTextBox.text = "Recording...";
            clip = Microphone.Start(Microphone.devices[0], false, 10, 44100);
            
            recording = true;
        }

        public void StopRecording()
        {
            microphoneRecord.StopRecord();
        }

        public async void OnRecordStop(AudioChunk recordedAudio)
        {
            _startButton.text = "Record";
            _buffer = "";
            Debug.Log(recordedAudio);
            var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
            if (res == null || _spokenTextBox.text =="") 
                return;
            Debug.Log(res.Result);
            response = res.Result.Trim().ToUpper()[0];
            _spokenTextBox.text = "Detected: " + response;
            Debug.Log("This is working!");
        }
        

        private void OnNewSegment(WhisperSegment segment)
        {
            if (!streamSegments || _spokenTextBox.text=="")
                return;

            _buffer += segment.Text;
            _spokenTextBox.text = _buffer + "...";
        }
    }