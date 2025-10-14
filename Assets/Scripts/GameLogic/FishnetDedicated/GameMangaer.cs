using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Component.Spawning;
using Unity.VisualScripting.Antlr3.Runtime;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    public Vector3 _gravity = new Vector3(0.0f, -9.81f, 0.0f);
    public CameraFollow _cameraFollow;

    [SerializeField]
    private GameObject _miniCubePrefab;
    [SerializeField]
    private GameObject _box;
    [SerializeField]
    private GameObject _plane;
    [SerializeField]
    private PlayerSpawner _playerSpawner;

    private bool _gameStarted = false;
    private GameObject _objectsParent;
    public CubeBehaviour CurrentPlayerCube { get; private set; }
    public uint CubeCount = 1000;

    [Server]
    private void SpawnMiniCube(Vector3 position)
    {
        var cube = Instantiate(_miniCubePrefab, _objectsParent.transform);
        cube.transform.localPosition = position;
        Spawn(cube);
        RpcSetToParent(cube);
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

    [ObserversRpc]
    private void RpcSetToParent(GameObject go)
    {
        go.transform.SetParent(_objectsParent.transform);
    }

    [ObserversRpc]
    private void RpcPrepeareobjectsParent()
    {
        _objectsParent = Instantiate(new GameObject(), _box.transform);
    }

   public void SetCurrentPlayerCube(CubeBehaviour cube)
    {
        CurrentPlayerCube = cube;
        if (_cameraFollow != null)
        {
            _cameraFollow.SetTarget(cube.transform);
        }
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        Physics.gravity = _gravity;
    }

    private void Start()
    {
        //if (_playerSpawner == null)
        //{
        //    Debug.LogError("Player spawner refernence is missing");
        //    return;
        //}
        //_playerSpawner.OnSpawned += OnPlayerSpawned;
    }

    //[Server]
    //private void OnPlayerSpawned(NetworkObject nob)
    //{
    //    if (!IsServerInitialized) return;

    //    Debug.Log($"OnPlayerSpawned: OwnerId = {nob.OwnerId}, LocalConnection Id = {nob.LocalConnection?.ClientId ?? -999}, IsOwnerSet = {nob.LocalConnection != null && nob.LocalConnection.IsValid}");
    //    if (nob.TryGetComponent<CubeBehaviour>(out var cube))
    //    {
    //        // Start a coroutine to delay the TargetRpc until observers are set
    //        StartCoroutine(DelayedTargetRpc(cube, nob.LocalConnection));
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Spawned player prefab missing CubeBehaviour!");
    //    }
    //}

    //[Server]
    //private System.Collections.IEnumerator DelayedTargetRpc(CubeBehaviour cube, NetworkConnection targetConn)
    //{
    //    Debug.Log($"DelayedTargetRpc Start: targetConn = {targetConn}, IsValid = {targetConn?.IsValid ?? false}");

    //    // Wait one frame for replication and observer setup
    //    yield return null;

    //    Debug.Log($"DelayedTargetRpc After Yield: targetConn = {targetConn}, IsValid = {targetConn?.IsValid ?? false}");

    //    // Validate the connection before calling TargetRpc
    //    if (targetConn != null && targetConn.IsValid)
    //    {
    //        cube.TargetRpcSetAsCurrentPlayer(targetConn);
    //        Debug.Log("TargetRpc called successfully.");
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"Invalid connection for TargetRpc on player cube. targetConn: {targetConn}");
    //    }
    //}

    [Server]
    public void RestartTheGame()
    {
        RpcPrepeareobjectsParent();

        if (_gameStarted)
        {
            Destroy(_objectsParent);
        }

        MainLogger.instance.Info("Game Started");

        _objectsParent = Instantiate(new GameObject(), _box.transform);

        Vector3 planeSize = _plane.GetComponent<Renderer>().bounds.size / 2;
        Vector3 offset = new Vector3(-planeSize.x / 2, 0, -planeSize.z / 2);
        FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(planeSize.x / 2), (int)(planeSize.z / 2), 2f, CubeCount);

        _gameStarted = true;
    }
}
