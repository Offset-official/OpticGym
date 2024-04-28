using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _button;
    private Button _button_eyetest;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _button = _document.rootVisualElement.Q("easy-button") as Button;
        _button.RegisterCallback<ClickEvent>(OnEasyEyeGameClick);

        _button_eyetest = _document.rootVisualElement.Q("eyetestbutton") as Button;
        _button_eyetest.RegisterCallback<ClickEvent>(OnEyeTestClick);
    }
    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(OnEasyEyeGameClick);
    }
    
    private void OnEasyEyeGameClick(ClickEvent evt)
    {
        Debug.Log("Easy Eye Exercise Clicked");
        SceneManager.LoadScene("EasyEyeGame");
        
    }

    private void OnEyeTestClick(ClickEvent evt)
    {
        Debug.Log("Eye Test Clicked");
        SceneManager.LoadScene("EyeTest");
        
    }
}
