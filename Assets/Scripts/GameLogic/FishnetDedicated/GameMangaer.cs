using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Component.Spawning;

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
    private CubeBehaviour _currentPlayerCube;

    [Server]
    private void SpawnMiniCube(Vector3 position)
    {
        var cube = Instantiate(_miniCubePrefab, _objectsParent.transform);
        cube.transform.localPosition = position;
        cube.GetComponent<Attracted>().SetAttractedTo(_currentPlayerCube.gameObject);
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
        _objectsParent = Instantiate(_objectsParent, _box.transform);
    }

   public void SetCurrentPlayerCube(CubeBehaviour cube)
    {
        _currentPlayerCube = cube;
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
        if (_playerSpawner == null)
        {
            Debug.LogError("Player spawner refernence is missing");
            return;
        }
        _playerSpawner.OnSpawned += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(NetworkObject nob)
    {
        CubeBehaviour cube = nob.GetComponent<CubeBehaviour>();
        if (cube != null)
        {
            cube.TargetRpcSetAsCurrentPlayer(nob.LocalConnection);
        }
        else
        {
            Debug.LogWarning("Spawned player prefab missing CubeBehaviour!");
        }
    }

    [Server]
    public void RestartTheGame(uint cubeCount = 1000)
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
        FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(planeSize.x / 2), (int)(planeSize.z / 2), 2f, cubeCount);

        _gameStarted = true;
    }
}
