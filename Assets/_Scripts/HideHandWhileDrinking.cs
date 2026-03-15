using UnityEngine;

public class HideHandWhileDrinking : MonoBehaviour
{
    [SerializeField] private GameObject handToHide;

    public void HideHand()
    {
        handToHide.SetActive(false);
    }

    public void ShowHand()
    {
        handToHide.SetActive(true);
    }
}
