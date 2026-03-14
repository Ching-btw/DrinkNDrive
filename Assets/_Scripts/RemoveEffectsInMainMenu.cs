using UnityEngine;

public class RemoveEffectsInMainMenu : MonoBehaviour
{
    [SerializeField] private Material radialScreenBlurMaterial;

    private void Start()
    {
        if (radialScreenBlurMaterial.HasFloat("_Strength")) radialScreenBlurMaterial.SetFloat("_Strength", 0f);
    }
}
