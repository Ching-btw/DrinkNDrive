using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuUI : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject CreditsPanel;
    void Start()
    { 
        instructionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        
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

    public void HideCredits()
    {
        CreditsPanel.SetActive(false);
    }



    public void ShowCredits()
    {
        CreditsPanel.SetActive(true);

    }

    public void Exit()
    {
        Application.Quit();
    }
}





