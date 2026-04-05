using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }

    [SerializeField, Range(0.1f, 0.9f)] private float joystickDeadzone = 0.1f;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.CarController.Enable();
    }

    private void OnDestroy()
    {
        playerInputActions.Dispose();
    }

    public bool WasPauseButtonPressed()
    {
        return playerInputActions.CarController.Pause.WasPressedThisFrame();
    }

    public float GetAccelerateInputMagnitude()
    {
        return GetNormalizedInputWithDeadzone(Mathf.Abs(playerInputActions.CarController.Accelerate.ReadValue<float>()));
    }

    public float GetDecelerateInputMagnitude()
    {
        return GetNormalizedInputWithDeadzone(Mathf.Abs(playerInputActions.CarController.Decelerate.ReadValue<float>()));
    }

    public float GetSteerLeftInputMagnitude()
    {
        return GetNormalizedInputWithDeadzone(Mathf.Abs(playerInputActions.CarController.SteerLeft.ReadValue<float>()));
    }

    public float GetSteerRightInputMagnitude()
    {
        return GetNormalizedInputWithDeadzone(Mathf.Abs(playerInputActions.CarController.SteerRight.ReadValue<float>()));
    }

    public bool isAccelerateButtonPressed()
    {
        return playerInputActions.CarController.Accelerate.IsPressed();
    }

    public bool isDecelerateButtonPressed()
    {
        return playerInputActions.CarController.Decelerate.IsPressed();
    }

    public bool isSteerRightButtonPressed()
    {
        return playerInputActions.CarController.SteerRight.IsPressed();
    }

    public bool isDrinkButtonPressed()
    {
        return playerInputActions.CarController.Drink.IsPressed();
    }

    private float GetNormalizedInputWithDeadzone(float input)
    {
        if (input <= joystickDeadzone)
        {
            return 0f;
        }
        else if (input >= 1f - joystickDeadzone)
        {
            return 1f;
        }
        else
        {
            return input;
        }
    }
}
