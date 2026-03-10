using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Stats")]
    [SerializeField] private int maxNumberOfDrinks = 5;

    [Space]
    [Header("References")]
    [SerializeField] private CarDriver carDriver;


    private void Awake()
    {
        Instance = this;
    }


}
