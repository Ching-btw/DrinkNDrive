using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject optionsPanel;
    void Start()
    {
        optionsPanel.SetActive(false);   
    }


    public void OnHover(GameObject button)
    {
        button.transform.localScale = Vector3.Lerp(button.transform.localScale, new Vector3(1.25f, 1.25f, 1.25f), Time.deltaTime * 100f);
    }

    public void OnHover1(GameObject Button)
    {
        Button.transform.localScale = Vector3.Lerp(Button.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 100f);

    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
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


