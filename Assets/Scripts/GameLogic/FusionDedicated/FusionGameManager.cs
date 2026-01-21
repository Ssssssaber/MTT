using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FusionGameManager : NetworkBehaviour
{
    public static FusionGameManager Instance { get; private set; }
    public Vector3 _gravity = new Vector3(0.0f, -9.81f, 0.0f);
    public CameraFollow _cameraFollow;
    public CubeBehaviour CurrentPlayerCube { get; private set; }

    [SerializeField]
    private NetworkObject _miniCubePrefab;

    [SerializeField]
    private GameObject _box;

    [SerializeField]
    private GameObject _plane;

    [SerializeField]
    private NetworkObject _objectsParentPrefab;

    private NetworkObject _objectsParent;
    private CallbackTimer _restartSimulationTimer;
    public static Action RecordingStopped;
    private uint CubeCount = 0;
    private bool _gameStarted = false;

    public void SetPlayerCube(CubeBehaviour cube) { CurrentPlayerCube = cube; }

    private void Awake()
    {
        Physics.gravity = _gravity;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override void Spawned()
    {
        if (!Runner.IsServer) return;

        var initArgs = GameManager.Instance.GetCommandLineArguments();
        CubeCount = initArgs.cubeCount;
        _restartSimulationTimer = new CallbackTimer(RestartTheGame, initArgs.recordingTime, true);
        RestartTheGame();
    }

    private void SpawnMiniCube(Vector3 position)
    {
        if (!Runner.IsServer) return;

        var cube = Runner.Spawn(
            _miniCubePrefab,
            position,
            Quaternion.identity,
            inputAuthority: null,
            (runner, networkObject) =>
            {
                // This callback runs on all clients after the object is spawned
                networkObject.transform.SetParent(_objectsParent.transform);
            }
        );
    }

    private void FillPlaneWithCubes(Vector3 offset, int width, int height, float spacing, uint maxCubes = 100)
    {
        if (!Runner.IsServer) return;

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

    public void SetCurrentPlayerCube(CubeBehaviour cube)
    {
        CurrentPlayerCube = cube;
        if (_cameraFollow != null && cube.HasInputAuthority)
        {
            _cameraFollow.SetTarget(CurrentPlayerCube.transform);
        }
    }

    public void RestartTheGame()
    {
        RecordingStopped?.Invoke();
        RestartTheGame(CubeCount);
    }

    public void RestartTheGame(uint cubeCount = 1000)
    {
        if (_gameStarted && _objectsParent != null)
        {
            Runner.Despawn(_objectsParent);
        }

        // Spawn a new parent object
        _objectsParent = Runner.Spawn(
            _objectsParentPrefab,
            _box.transform.position,
            Quaternion.identity,
            onBeforeSpawned: (runner, networkObject) =>
            {
                // This will run on the server before the object is spawned
                networkObject.name = "ObjectsParent";
            }
        );

        Vector3 planeSize = _plane.GetComponent<Renderer>().bounds.size / 2;
        Vector3 offset = new Vector3(-planeSize.x / 2, 0, -planeSize.z / 2);
        FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(planeSize.x / 2), (int)(planeSize.z / 2), 2f, cubeCount);

        _gameStarted = true;
    }

    void Update()
    {
        if (_restartSimulationTimer == null) return;
        _restartSimulationTimer.Update(Time.deltaTime);
    }
}
