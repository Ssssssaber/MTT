using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 _gravity = new Vector3(0.0f, -9.81f, 0.0f);
    [SerializeField]
    private CameraFollow _cameraFollow;

    [SerializeField]
    private GameObject _miniCubePrefab;
    [SerializeField]
    private GameObject _playerCubePrefab;

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
        cube.GetComponent<Attracted>().SetAttractedTo(_currentPlayerCube.gameObject);
    }

    private void FillPlaneWithCubes(Vector3 offset, int width, int height, float spacing)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = new Vector3(offset.x + x * spacing, offset.y + 0.5f, offset.z + z * spacing);
                SpawnMiniCube(position);
            }
        }
    }

    private void Awake()
    {
        Physics.gravity = _gravity;
    }

    // Start is called before the first frame update
    void Start()
    {
        RestartTheGame(); 
    }

    public void RestartTheGame(uint cubeCount = 100)
    {
        if (_gameStarted)
        {
            Destroy(_objectsParent);
        }

        MainLogger.Info("Game Started");

        _objectsParent = Instantiate(new GameObject("ObjectsParent"), _box.transform);

        _currentPlayerCube = Instantiate(_playerCubePrefab, _objectsParent.transform).GetComponent<CubeBehaviour>();
        _cameraFollow.SetTarget(_currentPlayerCube.transform);

        Vector3 _planeSize = _plane.GetComponent<Renderer>().bounds.size / 2;
        Vector3 offset = new Vector3(-_planeSize.x / 2, 0, -_planeSize.z / 2);
        FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(_planeSize.x / 2), (int)(_planeSize.z / 2), 2f);

        _gameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
