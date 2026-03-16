using UnityEditor;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            Debug.Log(other.name);
            if(other.name == "RedVelvet")
            {
                GameManager.Instance.WonGame();
            }
            else
            {
                GameManager.Instance.IncrementPlayerRank();
                other.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
