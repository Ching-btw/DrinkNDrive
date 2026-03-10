using UnityEngine;

public class CarSounds : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    private float currentSpeed;
    private float pitchFromCar;

    private Rigidbody carRb;
    private AudioSource audioSource;

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        EngineSound();
    }

    private void EngineSound()
    {
        currentSpeed = carRb.linearVelocity.magnitude;
        pitchFromCar = carRb.linearVelocity.magnitude / 20f;

        if(currentSpeed < minSpeed)
        {
            audioSource.pitch = minPitch;
        }
        else if(currentSpeed > maxSpeed)
        {
            audioSource.pitch = maxPitch;
        }
        else
        {
            audioSource.pitch = minPitch + pitchFromCar;
        }
    }
}
