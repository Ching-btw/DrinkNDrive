using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject optionsPanel;
    void Start()
    {
        optionsPanel.SetActive(false);   
    }

    public void ResumeGame()
    {
        Debug.Log("Resume button pressed");
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }
}


