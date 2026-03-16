using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Stats")]
    [SerializeField] private int maxNumberOfDrinks = 5;
    [SerializeField] private float drinkCooldown = 3f;

    [Space]
    [Header("References")]
    [SerializeField] private CarDriver carDriver;
    [SerializeField] private Transform cansParent;
    [SerializeField] private Animator drinkAnimator;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private RadialBlurController radialBlurController;
    [SerializeField] private AudioSource musicSource;

    [Header("Drink Effects")]
    [SerializeField] private List<GameObject> drinkEffectsInOrder;

    private float drinkTimer = 0;
    private int drinksLeft = 0;
    private bool isInDrinkCooldown = false;

    private bool isPaused;

    private bool isGameAlreadyEnded = false;

    private int playerRank = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        radialBlurController.SetMinMax(0, 0);

        ResumeGame();
        victoryCanvas.SetActive(false);

        playerRank = 1;

        isGameAlreadyEnded = false;

        drinksLeft = maxNumberOfDrinks;

        if (drinkEffectsInOrder.Count > 0)
        {
            foreach (GameObject drinkEffect in drinkEffectsInOrder)
            {
                drinkEffect.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (GameInput.Instance.WasPauseButtonPressed())
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (!isInDrinkCooldown)
        {
            drinkTimer = drinkCooldown;
        }
        else
        {
            drinkTimer -= Time.deltaTime;

            if(drinkTimer <= 0 && !GameInput.Instance.isDrinkButtonPressed())
            {
                drinkTimer = drinkCooldown;
                isInDrinkCooldown = false;
            }
        }

        if(!isInDrinkCooldown && GameInput.Instance.isDrinkButtonPressed())
        {
            if (drinksLeft > 0)
            {
                Drink();
            }
        }
    }

    private void Drink()
    {
        isInDrinkCooldown = true;

        drinksLeft--;
        if(cansParent.childCount > 0) Destroy(cansParent.GetChild(0).gameObject);

        drinkAnimator.SetTrigger("Drink");

        carDriver.SetDrinkMultiplier(maxNumberOfDrinks - drinksLeft);

        for(int i=0; i<maxNumberOfDrinks; i++)
        {
            if(i < maxNumberOfDrinks - drinksLeft)
            {
                drinkEffectsInOrder[i].SetActive(true);
            }
            else
            {
                drinkEffectsInOrder[i].SetActive(false);
            }
        }

        switch (drinksLeft)
        {
            default:
                break;
            case 2:          // Drank 3
                radialBlurController.SetMinMax(-0.1f, 0.1f);
                break;
            case 1:          // Drank 4
                radialBlurController.SetMinMax(-0.1f, 0.4f);
                break;
            case 0:          // Drank 5
                radialBlurController.SetMinMax(0.3f, 0.5f);
                break;
        }
    }

    public int GetMaxNumberOfDrinks()
    {
        return maxNumberOfDrinks;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseCanvas.SetActive(true);
        isPaused = true;
        musicSource.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
        isPaused = false;
        musicSource.Play();
    }

    public void WonGame()
    {
        if (isGameAlreadyEnded) return;

        victoryCanvas.SetActive(true);

        finalText.text = $"Congrats Drunkard!\nYou are on position {playerRank} among drunkards";
    }

    public void IncrementPlayerRank()
    {
        playerRank++;
    }
}
