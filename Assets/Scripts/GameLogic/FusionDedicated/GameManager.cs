using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance { get; private set; }
    public Vector3 _gravity = new Vector3(0.0f, -9.81f, 0.0f);
    public CameraFollow _cameraFollow;

    [SerializeField]
    private GameObject _miniCubePrefab;

    [SerializeField]
    private GameObject _box;

    [SerializeField]
    private GameObject _plane;

    private bool _gameStarted = false;
    private GameObject _objectsParent;
    private CubeBehaviour _currentPlayerCube;
    private void SpawnMiniCube(Vector3 position)
    {
        var cube = Instantiate(_miniCubePrefab, _objectsParent.transform);
        cube.transform.localPosition = position;
    }

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

    private void Awake()
    {
        Physics.gravity = _gravity;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        RestartTheGame(); 
    }

    public void SetCurrentPlayer(CubeBehaviour cube)
    {
        _currentPlayerCube = cube;
        _cameraFollow.SetTarget(_currentPlayerCube.transform);
    }

    public void RestartTheGame(uint cubeCount = 1000)
    {
        if (_gameStarted)
        {
            Destroy(_objectsParent);
        }

        MainLogger.instance.Info("Game Started");

        _objectsParent = Instantiate(new GameObject("ObjectsParent"), _box.transform);

        Vector3 _planeSize = _plane.GetComponent<Renderer>().bounds.size / 2;
        Vector3 offset = new Vector3(-_planeSize.x / 2, 0, -_planeSize.z / 2);
        FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(_planeSize.x / 2), (int)(_planeSize.z / 2), 2f, cubeCount);

        _gameStarted = true;
    }
}
