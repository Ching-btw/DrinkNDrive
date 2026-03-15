using UnityEngine;

public class RadialBlurController : MonoBehaviour
{
    [SerializeField] private Material radialScreenBlur;
    [SerializeField] private float blurLoopTime = 3f;

    private int loopDir = +1;
    private float timer = 0f;

    private float minStrength = 0f;
    private float maxStrength = 0f;

    private void Awake()
    {
        if (radialScreenBlur.HasFloat("_Strength")) radialScreenBlur.SetFloat("_Strength", 0f);
    }

    private void Start()
    {
        loopDir = +1;
        timer = 0f;

        minStrength = 0f;
        maxStrength = 0f;
    }

    private void Update()
    {
        timer = timer + Time.deltaTime * loopDir;

        if(radialScreenBlur.HasFloat("_Strength")) radialScreenBlur.SetFloat("_Strength", ((timer / blurLoopTime) * (maxStrength - minStrength)) + minStrength);

        if (timer >= blurLoopTime) loopDir = -1;
        else if (timer <= 0f) loopDir = +1;
    }

    public void SetLoopTime(float time)
    {
        blurLoopTime = time;
    }

    public void SetMinMax(float min, float max)
    {
        minStrength = min;
        maxStrength = max;
    }
}
