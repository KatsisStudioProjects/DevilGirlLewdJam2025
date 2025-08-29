using LewdJam2025.Controllers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] PlayerController _playerController;
    [SerializeField] CameraController _cameraController;

    ConsoleController _consoleInRange;


    private void Awake()
    {
        Instance = this;

        if (_playerController == null)
        {
            try { _playerController = FindFirstObjectByType<PlayerController>(); } catch { Debug.LogError("No Player found in scene."); }
        }
    }

    public void AssignInRangeConsole(ConsoleController consoleInRange, bool enterRange)
    {
        if (enterRange)
            _consoleInRange = consoleInRange;
        else //exit range
            _consoleInRange = null;
    }

    #region Console Controls

    public void BeginConsoleMinigame()
    {
        if (_consoleInRange == null) return;

        //The player has pressed E while in range of a console

        _consoleInRange.UsePanel(_playerController.transform, _cameraController.transform);
        _playerController.StartConsole();

        //Enable Timer for Console
        //Enable minigame circle
        //Confirm win or loss
    }

    public bool CheckConsole()
    {
        if (_consoleInRange.InZone)
        {
            Debug.Log("In");
            return true;
        }
        else
        {
            Debug.Log("Out");
            return false;
        }
    }

    public void EndConsoleMinigame(bool beatMinigame)
    {
        //check if game over or won
    }
    #endregion
}
