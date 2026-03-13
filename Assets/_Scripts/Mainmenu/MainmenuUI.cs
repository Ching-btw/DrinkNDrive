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

    
    public void OnHover(GameObject button)
    {
        button.transform.localScale = Vector3.Lerp(button.transform.localScale, new Vector3(1.25f,1.25f,1.25f),Time.deltaTime*100f);
    }
    
    public void OnHover1(GameObject Button)
    {
        Button.transform.localScale = Vector3.Lerp(Button.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 100f);
        
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





