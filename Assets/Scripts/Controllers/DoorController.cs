using LewdJam2025.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] private List<ConsoleController> _connectedConsoles;

    private void Start()
    {
        foreach (var controller in _connectedConsoles)
        {
            controller.AddDoorToList(this);
        }
    }

    public void Open()
    {
        bool canOpen = _connectedConsoles.TrueForAll(c => c.ConsoleOpen);

        GetComponent<Animator>().SetBool("IsOpen", canOpen);
    }

    public void Close()
    {
        GetComponent<Animator>().SetBool("IsOpen", false);
    }
}
