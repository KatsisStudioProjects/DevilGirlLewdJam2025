using LewdJam2025.Controllers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] PlayerController _playerController;

    ConsoleController _consoleInRange;


    private void Awake()
    {
        Instance = this;
        
        if(_playerController == null)
        {
            try { _playerController = FindFirstObjectByType<PlayerController>(); } catch{ Debug.LogError("No Player found in scene."); }
        }
    }

    public void AssignInRangeConsole(ConsoleController consoleInRange, bool enterRange)
    {
        if (enterRange)
            _consoleInRange = consoleInRange;
        else //exit range
            _consoleInRange = null;
    }

    public void BeginConsoleMinigame()
    {
        //The player has pressed E while in range of a console
        //Enable Timer for Console
        //Enable minigame circle
        //Confirm win or loss
    }

}
