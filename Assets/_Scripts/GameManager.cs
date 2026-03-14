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

            if(drinkTimer <= 0)
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

        carDriver.SetDrinkMultiplier(maxNumberOfDrinks - drinksLeft);
    }

    public int GetMaxNumberOfDrinks()
    {
        return maxNumberOfDrinks;
    }

}
