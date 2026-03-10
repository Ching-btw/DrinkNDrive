using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuUI : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;
    void Start()
    { 
        instructionsPanel.SetActive(false);
        
    }

    void Update()
    {
        
    }

    public void Play()
    {
        Debug.Log("play");
        SceneManager.LoadScene("TestingArea");
    }

    public void ShowInstruction()
    {
        instructionsPanel.SetActive(true);
    }

    public void HideInstruction()
    {
        instructionsPanel.SetActive(false);

    }

    public void Credits()
    {
        Debug.Log("credits");

    }

    public void Exit()
    {
        Application.Quit();
    }
}





