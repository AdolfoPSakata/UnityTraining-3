using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button optionButton;


    private void Start()
    {

        playButton.onClick.AddListener(delegate { SceneManager.LoadScene(1); });
        exitButton.onClick.AddListener(delegate { Application.Quit(); });
        //optionButton.onClick.AddListener(delegate { SceneManager.LoadScene(1); });
    }
}
