using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.CarController.Enable();
    }

    public bool isAccelerateButtonPressed()
    {
        return playerInputActions.CarController.Accelerate.IsPressed();
    }

    public bool isDecelerateButtonPressed()
    {
        return playerInputActions.CarController.Decelerate.IsPressed();
    }

    public bool isBrakeButtonPressed()
    {
        return playerInputActions.CarController.Brake.IsPressed();
    }

    public bool isSteerLeftButtonPressed()
    {
        return playerInputActions.CarController.SteerLeft.IsPressed();
    }

    public bool isSteerRightButtonPressed()
    {
        return playerInputActions.CarController.SteerRight.IsPressed();
    }

    public bool isDrinkButtonPressed()
    {
        return playerInputActions.CarController.Drink.IsPressed();
    }
}
