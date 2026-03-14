using UnityEngine;
using UnityEngine.Rendering;

public class DrinkEffectTransition : MonoBehaviour
{
    [SerializeField] private Volume drinkGlobalVolume;
    [SerializeField] private float transitionTime = 3f;

    private float timer = 0f;
    private bool isTransitionDone = false;

    private void Start()
    {
        drinkGlobalVolume.weight = 0f;
        timer = 0f;
        isTransitionDone = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!isTransitionDone)
        {
            drinkGlobalVolume.weight = Mathf.Lerp(0f, 1f, timer / transitionTime);
        }
        if(timer >= transitionTime)
        {
            isTransitionDone = true;
            drinkGlobalVolume.weight = 1f;
            timer = 0f;
        }
    }
}
