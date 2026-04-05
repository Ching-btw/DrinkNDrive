using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private float selectedButtonScale = 1.2f;
    [SerializeField] private float buttonSelectSpeed = 5f;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button showOptionsButton;
    [SerializeField] private Button hideOptionsButton;
    [SerializeField] private Button mainMenuButton;

    private GameObject selectedButton;
    void Start()
    {
        optionsPanel.SetActive(false);

        resumeButton.Select();
        selectedButton = resumeButton.gameObject;
    }

    private void Update() {
        selectedButton = EventSystem.current.currentSelectedGameObject;

        SetButtonSizes();
    }


    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);

        hideOptionsButton.Select();
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);

        showOptionsButton.Select();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene("mainmenu");
    }

    private void SetButtonSizes() {
        resumeButton.transform.localScale = Vector3.Lerp(resumeButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.unscaledDeltaTime);
        showOptionsButton.transform.localScale = Vector3.Lerp(showOptionsButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.unscaledDeltaTime);
        hideOptionsButton.transform.localScale = Vector3.Lerp(hideOptionsButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.unscaledDeltaTime);
        mainMenuButton.transform.localScale = Vector3.Lerp(mainMenuButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.unscaledDeltaTime);

        if (selectedButton != null) selectedButton.transform.localScale = Vector3.Lerp(selectedButton.transform.localScale, new Vector3(selectedButtonScale, selectedButtonScale, selectedButtonScale), buttonSelectSpeed * 2 * Time.unscaledDeltaTime);
    }
}


