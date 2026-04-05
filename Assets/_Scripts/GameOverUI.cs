using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private float selectedButtonScale = 1.2f;
    [SerializeField] private float buttonSelectSpeed = 5f;

    [Header("Buttons")]
    [SerializeField] private Button mainMenuButton;

    private GameObject selectedButton;
    void Start() {
        mainMenuButton.Select();
        selectedButton = mainMenuButton.gameObject;
    }

    private void Update() {
        selectedButton = EventSystem.current.currentSelectedGameObject;

        SetButtonSizes();
    }

    public void GoToMainMenu() {
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene("mainmenu");
    }

    private void SetButtonSizes() {
        mainMenuButton.transform.localScale = Vector3.Lerp(mainMenuButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.unscaledDeltaTime);

        if (selectedButton != null) selectedButton.transform.localScale = Vector3.Lerp(selectedButton.transform.localScale, new Vector3(selectedButtonScale, selectedButtonScale, selectedButtonScale), buttonSelectSpeed * 2 * Time.unscaledDeltaTime);
    }
}


