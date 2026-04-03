using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyCarAI : MonoBehaviour
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
    [SerializeField, Range(1f, 3f)] private float drinkMultiplierPerDrink = 1.1f;
    [SerializeField] private float motorTorque = 50f;
    [SerializeField] private float brakeTorque = 200;

    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float maxSpeed = 30000f;

    [SerializeField] private float minSpeedToShowTireMarks = 6f;

    [Space]
    [SerializeField] private Vector3 centerOfMass;

    [Space]
    [Header("Wheels")]
    [SerializeField] private List<Wheel> wheels;

    [Space]
    [Header("Path")]
    [SerializeField] private Transform path;

    [Space]
    [Header("Sensors")]
    [SerializeField] private float sensorLength = 10f;
    [SerializeField] private float sideSensorLength = 1f;
    [SerializeField] private float brakeSensorLength = 1f;
    [SerializeField] private float sensorHeightOffset = 0.6f;
    [SerializeField] private float frontSensorFrontOffset = 1.3f;
    [SerializeField] private float frontSensorSideOffset = 0.55f;
    [SerializeField] private float frontSensorAngle = 30f;
    [SerializeField] private float brakeSensorSideOffset = 0.1f;
    [SerializeField] private float sideSensor_1_Offset = 1f;
    [SerializeField] private float sideSensor_2_Offset = 0.7f;

    //[SerializeField] private bool isBraking;

    private Rigidbody carRb;

    private List<Transform> nodes;
    private int currentNode = 0;
    private float currentSpeed;
    private bool isAvoiding = false;
    private float avoidMultiplier = 0;
    private float targetSteerAngle = 0;

    private bool isBraking = false;

    private float drinkMultiplier = 1f;

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        carRb.centerOfMass = centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        foreach (Transform pathTransform in pathTransforms)
        {
            if (pathTransform != path.transform)
            {
                nodes.Add(pathTransform);
            }
        }

        drinkMultiplier = 1f;
    }

    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckWayPointDistance();
        Braking();
        LerpWheelsToSteerAngle();
        AnimateWheels();
        WheelEffects();
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos;
        avoidMultiplier = 0;
        isAvoiding = false;

        // Front Right Sensor
        sensorStartPos = transform.position + transform.forward * frontSensorFrontOffset + transform.up * sensorHeightOffset + transform.right * frontSensorSideOffset;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier -= 1f;
            }
        }

        // Front Right Angled Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        // Extra Sensors to avoid obstacles
        else
        {
            // Side Sensor 1
            sensorStartPos = transform.position + transform.forward * sideSensor_1_Offset + transform.up * sensorHeightOffset + transform.right * frontSensorSideOffset;
            if (Physics.Raycast(sensorStartPos, transform.right, out hit, sideSensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    isAvoiding = true;
                    avoidMultiplier -= 0.2f;
                }
            }

            else
            {
                // Side Sensor 2
                sensorStartPos = transform.position + transform.forward * sideSensor_2_Offset + transform.up * sensorHeightOffset + transform.right * frontSensorSideOffset;
                if (Physics.Raycast(sensorStartPos, transform.right, out hit, sideSensorLength))
                {
                    if (!hit.collider.CompareTag("Terrain"))
                    {
                        Debug.DrawLine(sensorStartPos, hit.point);
                        isAvoiding = true;
                        avoidMultiplier -= 0.1f;
                    }
                }
            }
        }

        // Front Left Sensor
        sensorStartPos = transform.position + transform.forward * frontSensorFrontOffset + transform.up * sensorHeightOffset - transform.right * frontSensorSideOffset;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier += 1f;
            }
        }

        // Front Left Angled Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                avoidMultiplier += 0.5f;
            }
        }

        // Extra Sensors to avoid obstacles
        else
        {
            // Side Sensor 1
            sensorStartPos = transform.position + transform.forward * sideSensor_1_Offset + transform.up * sensorHeightOffset - transform.right * frontSensorSideOffset;
            if (Physics.Raycast(sensorStartPos, -transform.right, out hit, sideSensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    isAvoiding = true;
                    avoidMultiplier += 0.2f;
                }
            }

            else
            {
                // Side Sensor 2
                sensorStartPos = transform.position + transform.forward * sideSensor_2_Offset + transform.up * sensorHeightOffset - transform.right * frontSensorSideOffset;
                if (Physics.Raycast(sensorStartPos, -transform.right, out hit, sideSensorLength))
                {
                    if (!hit.collider.CompareTag("Terrain"))
                    {
                        Debug.DrawLine(sensorStartPos, hit.point);
                        isAvoiding = true;
                        avoidMultiplier += 0.1f;
                    }
                }
            }
        }

        // Front Middle Sensor
        if (avoidMultiplier == 0)
        {
            sensorStartPos = transform.position + transform.forward * frontSensorFrontOffset + transform.up * sensorHeightOffset;
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    isAvoiding = true;

                    avoidMultiplier = (hit.normal.x < 0) ? -1f : 1f;
                }
            }
        }

        // Front Right Break Sensor
        sensorStartPos = transform.position + transform.forward * frontSensorFrontOffset + transform.up * sensorHeightOffset + transform.right * brakeSensorSideOffset;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, brakeSensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isBraking = true;
            }
        }

        else
        {
            // Front Left Break Sensor
            sensorStartPos = transform.position + transform.forward * frontSensorFrontOffset + transform.up * sensorHeightOffset - transform.right * brakeSensorSideOffset;
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, brakeSensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    isBraking = true;
                }
            }

            else
            {
                isBraking = false;
            }
        }
    }

    private void ApplySteer()
    {
        if (isAvoiding)
        {
            targetSteerAngle = avoidMultiplier * maxSteerAngle;
        }
        else
        {
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position).normalized;
            float steerAngle = relativeVector.x * maxSteerAngle;

            targetSteerAngle = steerAngle;
        }
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheels[0].wheelCollider.radius * wheels[0].wheelCollider.rpm * 60;

        if (currentSpeed < maxSpeed * drinkMultiplier  && !isBraking)
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = motorTorque * drinkMultiplier;
            }
        }
        else
        {
            foreach (Wheel wheel in wheels)
            {
                if (wheel.axel == Axel.Front) wheel.wheelCollider.motorTorque = 0;
            }
        }
    }

    private void CheckWayPointDistance()
    {
        Vector2 transPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 nodePos = new Vector2(nodes[currentNode].position.x, nodes[currentNode].position.z);
        if(Vector3.Distance(transPos, nodePos) <= 5f)
        {
            currentNode = (currentNode + 1) % nodes.Count;
        }
        else if (Vector3.Distance(transPos, nodePos) <= 15f)
        {
            if (transform.InverseTransformPoint(nodes[currentNode].position).z <= 0)
            {
                currentNode = (currentNode + 1) % nodes.Count;
            }
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            foreach (Wheel wheel in wheels)
            {
                if (wheel.axel == Axel.Back) wheel.wheelCollider.brakeTorque = brakeTorque;
            }
        }
        else
        {
            foreach (Wheel wheel in wheels)
            {
                if (wheel.axel == Axel.Back) wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private void AnimateWheels()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
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
                if (isBraking && wheel.axel == Axel.Back && wheel.wheelCollider.isGrounded && carRb.linearVelocity.magnitude >= minSpeedToShowTireMarks)
                {
                    wheel.wheelEffectGameObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                    if (wheel.driftSmokeParticleSystem != null) wheel.driftSmokeParticleSystem.Emit(1);
                }
                else
                {
                    wheel.wheelEffectGameObj.GetComponentInChildren<TrailRenderer>().emitting = false;
                }
            }

        }
    }

    private void LerpWheelsToSteerAngle()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, targetSteerAngle, turnSpeed * Time.deltaTime);
            }
        }
    }

    public void SetDrinkMultiplier(int numberOfDrinks) {
        drinkMultiplier = Mathf.Pow(drinkMultiplierPerDrink, numberOfDrinks);
    }
}
