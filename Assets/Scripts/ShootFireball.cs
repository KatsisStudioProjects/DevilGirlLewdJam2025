using LewdJam2025.Controllers;
using UnityEngine;

public class ShootFireball : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    public void Fire()
    {
        playerController.ShootAFireball();
    }
}
