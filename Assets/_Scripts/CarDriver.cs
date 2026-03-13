using System;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Back,
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelMesh;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectGameObj;
        public ParticleSystem driftSmokeParticleSystem;
        public Axel axel;
    }

    [Header("Stats")]
    [SerializeField, Range(1f, 3f)] private float drinkMultiplierPerDrink = 1.2f;
    [SerializeField] private float motorTorque = 69f;
    [SerializeField] private float brakeTorque = 300f;

    [SerializeField] private float turnSensitivity = 5f;
    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private float maxSpeed = 50000f;

    [SerializeField] private float minSpeedToShowTireMarks = 6f;

    [Space]
    [SerializeField] private Vector3 centerOfMass;
    
    [Space]
    [Header("Wheels")]
    [SerializeField] private List<Wheel> wheels;

    [Space]
    [SerializeField] private GameObject steeringWheel;

    private float moveInput;
    private float steerInput;
    private float brakeInput;
    private Rigidbody carRb;
    private float currentSpeed;

    private float drinkMultiplier = 1f;

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        carRb.centerOfMass = centerOfMass;

        drinkMultiplier = 1f;
    }

    private void Update()
    {
        GetInput();
    }

    private void LateUpdate()
    {
        Move();
        Steer();
        AnimateWheels();
        Brake();
        WheelEffects();
    }

    private void GetInput()
    {
        moveInput = 0f;
        steerInput = 0f;
        brakeInput = 0f;

        moveInput += GameInput.Instance.GetAccelerateInputMagnitude();
        moveInput -= GameInput.Instance.GetDecelerateInputMagnitude();

        steerInput += GameInput.Instance.GetSteerRightInputMagnitude();
        steerInput -= GameInput.Instance.GetSteerLeftInputMagnitude();

        brakeInput += GameInput.Instance.GetBrakeInputMagnitude();

    }

    private void Move()
    {
        currentSpeed = 2 * Mathf.PI * wheels[0].wheelCollider.radius * wheels[0].wheelCollider.rpm * 60;

        if (currentSpeed < maxSpeed * drinkMultiplier)
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * motorTorque * drinkMultiplier;
            }
        }
        else
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }

    private void Steer()
    {
        float steerAngle = steerInput * maxSteerAngle;
        foreach(Wheel wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);

                if(steeringWheel != null)
                {
                    Vector3 eulerAngles = steeringWheel.transform.eulerAngles;
                    steeringWheel.transform.rotation = Quaternion.Slerp(steeringWheel.transform.rotation, Quaternion.Euler(eulerAngles.x, eulerAngles.y, -steerAngle * 2), turnSensitivity * Time.deltaTime);
                }
            }
        }
    }

    private void Brake()
    {
        if (brakeInput > 0.01f)
        {
            foreach(Wheel wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = brakeTorque;
            }
        }
        else
        {
            foreach(Wheel wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private void AnimateWheels()
    {
        foreach(Wheel wheel in wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 pos, out  Quaternion rot);
            wheel.wheelMesh.transform.position = pos;
            wheel.wheelMesh.transform.rotation = rot;
        }
    }

    private void WheelEffects()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.wheelEffectGameObj != null && wheel.wheelEffectGameObj.GetComponentInChildren<TrailRenderer>() != null)
            {
                if (brakeInput > 0.01f && wheel.axel == Axel.Back && wheel.wheelCollider.isGrounded && carRb.linearVelocity.magnitude >= minSpeedToShowTireMarks)
                {
                    wheel.wheelEffectGameObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                }
                else
                {
                    wheel.wheelEffectGameObj.GetComponentInChildren<TrailRenderer>().emitting = false;
                }
            }

            if(wheel.driftSmokeParticleSystem != null) wheel.driftSmokeParticleSystem.Emit(1);
        }
    }

    public void SetDrinkMultiplier(int numberOfDrinks)
    {
        drinkMultiplier = Mathf.Pow(drinkMultiplierPerDrink, numberOfDrinks);
    }

}
 