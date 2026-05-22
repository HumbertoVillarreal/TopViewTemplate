using UnityEngine;

public class PlayerShieldBlock : MonoBehaviour
{

    [SerializeField] private GameObject shieldCollider;

    private bool isBlocking;
    public bool IsBlocking => isBlocking;

    public void StartBlock()
    {
        if (isBlocking) return;

        isBlocking = true;
        shieldCollider.SetActive(true);
    }

    public void StopBlock()
    {
        isBlocking = false;
        shieldCollider.SetActive(false);
    }
}
