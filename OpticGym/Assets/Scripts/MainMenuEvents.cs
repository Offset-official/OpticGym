using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _button_eyeexercise;
    private Button _button_eyetest;

    private Button _button_blinktest;

    private Button _button_Doctors;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _button_eyeexercise = _document.rootVisualElement.Q("eyeExerciseButton") as Button;
        _button_eyeexercise.RegisterCallback<ClickEvent>(OnEasyEyeGameClick);

        _button_eyetest = _document.rootVisualElement.Q("eyeTestButton") as Button;
        _button_eyetest.RegisterCallback<ClickEvent>(OnEyeTestClick);

        _button_blinktest = _document.rootVisualElement.Q("blinkTestButton") as Button;
        _button_blinktest.RegisterCallback<ClickEvent>(OnBlinkClick);

        _button_Doctors = _document.rootVisualElement.Q("doctorButton") as Button;

        _button_Doctors.RegisterCallback<ClickEvent>(OnDoctorsClick);
    }
    private void OnDisable()
    {
        _button_eyeexercise.UnregisterCallback<ClickEvent>(OnEasyEyeGameClick);
    }

    private void OnEasyEyeGameClick(ClickEvent evt)
    {
        Debug.Log("Easy Eye Exercise Clicked");
        SceneManager.LoadScene("EyeGame");

    }

    private void OnDoctorsClick(ClickEvent evt)
    {
        Debug.Log("Doctors Clicked");
        SceneManager.LoadScene("Doctors");

    }

    private void OnEyeTestClick(ClickEvent evt)
    {
        Debug.Log("Eye Test Clicked");
        SceneManager.LoadScene("DistanceCheck");

    }
    private void OnBlinkClick(ClickEvent evt)
    {
        Debug.Log("Blink Test Clicked");
        SceneManager.LoadScene("EyeBlink");

    }
}
