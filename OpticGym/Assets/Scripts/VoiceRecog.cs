using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class VoiceRecog : MonoBehaviour{
    
    public static AudioClip clip;
    private static byte[] bytes;
    public static bool recording;
    public static string response;
    private Button _startButton;
    private Button _stopButton;
    private Label _spokenTextBox;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _startButton = root.Q<Button>("startButton");
        _stopButton = root.Q<Button>("stopButton");
        _spokenTextBox = root.Q<Label>("spokenText");
            
        // startButton.onClick.AddListener(StartRecording);
        // stopButton.onClick.AddListener(StopRecording);
        // stopButton.interactable = false;
        // Debug.Log("Number of devices found: " + Microphone.devices.Length);
        // if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        // {
        //     Permission.RequestUserPermission(Permission.Microphone);
        // }
        // foreach (var device in Microphone.devices)
        // {
        //     Debug.Log("Name: " + device);
        // }
    }
    //
    private void Update() {
        if (recording && Microphone.GetPosition(Microphone.devices[0]) >= clip.samples) {
            StopRecording();
        }
    }

    public void StartRecording()
    {
        _spokenTextBox.style.color = Color.white;
        _spokenTextBox.text = "Recording...";
        _stopButton.SetEnabled(true);
        _startButton.SetEnabled(false);
        clip = Microphone.Start(Microphone.devices[0], false, 10, 44100);
        recording = true;
    }

    public void StopRecording() {
        var position = Microphone.GetPosition(Microphone.devices[0]);
        Microphone.End(Microphone.devices[0]);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        // SendRecording();
    }

    // private  void SendRecording() {
    //     _spokenTextBox.style.color = Color.yellow;
    //     _spokenTextBox.text = "Sending...";
    //     _stopButton.SetEnabled(false);
    //     HuggingFaceAPI.AutomaticSpeechRecognition(bytes, res => {
    //         _spokenTextBox.style.color = Color.white;
    //         _spokenTextBox.text = "Detected: " +res;
    //         response = res;
    //         _startButton.SetEnabled(true);
    //     }, error => {
    //         _spokenTextBox.style.color = Color.red;
    //         _spokenTextBox.text = error;
    //         _startButton.SetEnabled(true);
    //     });
    // }

    public byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}
