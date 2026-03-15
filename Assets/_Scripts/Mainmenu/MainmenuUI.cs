using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainmenuUI : MonoBehaviour

{
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private float selectedButtonScale = 1.2f;
    [SerializeField] private float buttonSelectSpeed = 5f;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button showInstructionsButton;
    [SerializeField] private Button hideInstructionsButton;
    [SerializeField] private Button showCreditsButton;
    [SerializeField] private Button hideCreditsButton;
    [SerializeField] private Button quitButton;

    private GameObject selectedButton;

    void Start()
    { 
        instructionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);

        playButton.Select();
        selectedButton = playButton.gameObject;
    }

    private void Update()
    {
        selectedButton = EventSystem.current.currentSelectedGameObject;

        SetButtonSizes();
    }

    public void OnHover(GameObject button)
    {
        //button.transform.localScale = Vector3.Lerp(button.transform.localScale, new Vector3(1.25f,1.25f,1.25f),Time.deltaTime*100f);
    }
    
    public void OnHover1(GameObject Button)
    {
        //Button.transform.localScale = Vector3.Lerp(Button.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 100f);
        
    }

    public void Play()
    {
        Debug.Log("play");
        SceneManager.LoadScene("GameScene");
    }

    public void ShowInstruction()
    {
        instructionsPanel.SetActive(true);

        hideInstructionsButton.Select();
    }

    public void HideInstruction()
    {
        instructionsPanel.SetActive(false);

        showInstructionsButton.Select();
    }

    public void HideCredits()
    {
        CreditsPanel.SetActive(false);

        showCreditsButton.Select();
    }



    public void ShowCredits()
    {
        CreditsPanel.SetActive(true);

        hideCreditsButton.Select();
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void SetButtonSizes()
    {
        playButton.transform.localScale = Vector3.Lerp(playButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.deltaTime);
        showInstructionsButton.transform.localScale = Vector3.Lerp(showInstructionsButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.deltaTime);
        hideInstructionsButton.transform.localScale = Vector3.Lerp(hideInstructionsButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.deltaTime);
        showCreditsButton.transform.localScale = Vector3.Lerp(showCreditsButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.deltaTime);
        hideCreditsButton.transform.localScale = Vector3.Lerp(hideCreditsButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.deltaTime);
        quitButton.transform.localScale = Vector3.Lerp(quitButton.transform.localScale, Vector3.one, buttonSelectSpeed * Time.deltaTime);

        if(selectedButton != null) selectedButton.transform.localScale = Vector3.Lerp(selectedButton.transform.localScale, new Vector3(selectedButtonScale, selectedButtonScale, selectedButtonScale), buttonSelectSpeed * 2 * Time.deltaTime);
    }
}





