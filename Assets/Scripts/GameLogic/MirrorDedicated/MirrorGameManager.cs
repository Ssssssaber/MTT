using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using System;

public class MirrorGameManager : NetworkBehaviour
{
    public static MirrorGameManager instance { get; private set; }
    public Vector3 _gravity = new Vector3(0.0f, -9.81f, 0.0f);
    public CameraFollow _cameraFollow;

    [SerializeField]
    private GameObject _miniCubePrefab;  // Must be a networked prefab with NetworkIdentity
    [SerializeField]
    private GameObject _playerCubePrefab;  // Assumed to be spawned by NetworkManager as player object
    public static Action RecordingStopped;

    [SerializeField]
    private GameObject _box;

    [SerializeField]
    private GameObject _plane;

    private bool _gameStarted = false;
    [SerializeField] private GameObject _objectsParentPrefab;
    private GameObject _objectsParent;
    private MirrorCubeBehaviour _currentPlayerCube;
    private CallbackTimer _restartSimulationTimer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        Physics.gravity = _gravity;
    }

    public override void OnStartServer()
    {
        var initArgs = GameManager.Instance.GetCommandLineArguments();
        CubeCount = initArgs.cubeCount;
        _restartSimulationTimer = new CallbackTimer(RestartTheGame, initArgs.recordingTime, true);
        RestartTheGame();
    }

    [Command]
    public void CmdSetCubeCount(uint count)
    {
        CubeCount = count;
    }

    [Server]
    private void SpawnMiniCube(Vector3 position)
    {
        var cube = Instantiate(_miniCubePrefab, _objectsParent.transform);
        cube.transform.localPosition = position;
        NetworkServer.Spawn(cube);  // Spawn on network

        RpcSetToObjectsParent(cube);
    }

    [ClientRpc]
    public void RpcSetToObjectsParent(GameObject gameObject)
    {
        gameObject.transform.SetParent(_objectsParent.transform);
    }

    [Server]
    private void FillPlaneWithCubes(Vector3 offset, int width, int height, float spacing, uint maxCubes = 100)
    {
        uint cubeCounter = 0;
        for (int x = 0; x < width && cubeCounter < maxCubes; x++)
        {
            for (int z = 0; z < height && cubeCounter < maxCubes; z++)
            {
                cubeCounter++;
                Vector3 position = new Vector3(offset.x + x * spacing, offset.y + 0.5f, offset.z + z * spacing);
                SpawnMiniCube(position);
            }
        }
    }

    public uint CubeCount = 1000;
 
    [Server]
    public void RestartTheGame()
    {
        RecordingStopped.Invoke();
        RestartTheGame(CubeCount);
    }

    [ClientRpc]
    public void RpcPrepeareObjectsParent()
    {
        _objectsParent = Instantiate(_objectsParentPrefab, _box.transform);
    }

    [Server]
    public void RestartTheGame(uint cubeCount = 1000)
    {
        RpcPrepeareObjectsParent();
        if (_gameStarted)
        {
            // Destroy networked objects properly
            foreach (Transform child in _objectsParent.transform)
            {
                NetworkServer.Destroy(child.gameObject);
            }
            Destroy(_objectsParent);
        }

        MainLogger.instance.Info("Game Started");

        _objectsParent = Instantiate(_objectsParentPrefab, _box.transform);

        Vector3 _planeSize = _plane.GetComponent<Renderer>().bounds.size / 2;
        Vector3 offset = new Vector3(-_planeSize.x / 2, 0, -_planeSize.z / 2);
        FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(_planeSize.x / 2), (int)(_planeSize.z / 2), 2f, cubeCount);

        _gameStarted = true;
    }

    [TargetRpc]
    public void TargetRpcSetCurrentPlayerCube(NetworkConnection target, MirrorCubeBehaviour cube)
    {
        _currentPlayerCube = cube;
        SetCameraTarget(cube.transform);
    }

    public MirrorCubeBehaviour GetPlayer() { return _currentPlayerCube; }

    [Client]
    private void SetCameraTarget(Transform target)
    {
        Debug.Log("Setting camera target");
        if (_cameraFollow != null)
        {
            Debug.Log("Camera follow found, setting target");
            _cameraFollow.SetTarget(target);
        }
    }

    // Update is called once per frame (no changes needed)
    void Update()
    {
        if (_restartSimulationTimer == null) return;

        _restartSimulationTimer.Update(Time.deltaTime);
    }
}
