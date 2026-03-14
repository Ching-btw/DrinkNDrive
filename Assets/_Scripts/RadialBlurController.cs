using UnityEngine;

public class RadialBlurController : MonoBehaviour
{
    [SerializeField] private Material radialScreenBlur;
    [SerializeField] private float blurLoopTime = 3f;

    private int loopDir = +1;
    private float timer = 0f;

    private void Awake()
    {
        if (radialScreenBlur.HasFloat("_Strength")) radialScreenBlur.SetFloat("_Strength", 0f);
    }

    private void Start()
    {
        loopDir = +1;
        timer = 0f;
    }

    private void Update()
    {
        timer = timer + Time.deltaTime * loopDir;

        if(radialScreenBlur.HasFloat("_Strength")) radialScreenBlur.SetFloat("_Strength", timer / (blurLoopTime * 2f));

        if(timer >= blurLoopTime || timer <= -blurLoopTime)
        {
            loopDir *= -1;
        }
    }
}
