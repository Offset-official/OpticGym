using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class PrevButton : MonoBehaviour
{
    public Button _backButton;
    public void Start()
    {
        _backButton.onClick.AddListener(ReturnToMain);
    }
	public void ReturnToMain()
	{
        SceneManager.LoadScene("MainMenu");
    }
}

