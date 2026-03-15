using System.Collections.Generic;
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

    [Header("Drink Effects")]
    [SerializeField] private List<GameObject> drinkEffectsInOrder;

    private float drinkTimer = 0;
    private int drinksLeft = 0;
    private bool isInDrinkCooldown = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
    }

    public int GetMaxNumberOfDrinks()
    {
        return maxNumberOfDrinks;
    }

    public void WonGame()
    {
        Debug.Log("Won");
    }
}
