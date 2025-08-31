using LewdJam2025.Controllers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] PlayerController _playerController;
    [SerializeField] CameraController _cameraController;

    ConsoleController _consoleInRange;

    float _consoleCheckDelayMax = 0.1f;
    float _consoleCheckDelayTimer;

    public int indy = 0;

    private void Awake()
    {
        Instance = this;

        if (_playerController == null)
        {
            try { _playerController = FindFirstObjectByType<PlayerController>(); } catch { Debug.LogError("No Player found in scene."); }
        }

        _consoleCheckDelayTimer = _consoleCheckDelayMax;
    }

    private void Start()
    {
        UpdateCameraPivot(0);
    }

    private void Update()
    {
        _consoleCheckDelayTimer -= Time.deltaTime;
    }

    public void AssignInRangeConsole(ConsoleController consoleInRange, bool enterRange)
    {
        if (enterRange)
            _consoleInRange = consoleInRange;
        else if(!enterRange && consoleInRange == _consoleInRange) //exit range
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
        if (_consoleInRange.InZone && _consoleCheckDelayTimer <= 0f)
        {
            //Debug.Log("In");
            _consoleInRange.AddToTimer();
            _consoleCheckDelayTimer = _consoleCheckDelayMax;
            return true;
        }
        else
        {
            //Debug.Log("Out");
            return false;
        }
    }

    public void EndConsoleMinigame(bool beatMinigame)
    {
        //check if game over or won
        if(beatMinigame)
        {
            StartCoroutine(_consoleInRange.EndConsoleMinigame());
            _consoleInRange = null;
        }
    }

    public void UpdateCameraPivot(int p)
    {
        _cameraController.ChangePivot(p);
    }
    #endregion
}
