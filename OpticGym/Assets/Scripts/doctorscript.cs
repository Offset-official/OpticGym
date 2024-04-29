using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class doctorscript : MonoBehaviour
{

    public Button _backButton;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _backButton = root.Q<Button>("BackButton");
        _backButton.RegisterCallback<ClickEvent>(OnBackClicked);
    }

    private void OnBackClicked(ClickEvent c)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
