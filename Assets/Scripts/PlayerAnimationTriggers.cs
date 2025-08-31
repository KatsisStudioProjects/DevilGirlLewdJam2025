using LewdJam2025.Controllers;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    [SerializeField] PlayerController _pc;

    public void CanMove()
    {
        _pc.OffConsoleToggle();
    }
}
