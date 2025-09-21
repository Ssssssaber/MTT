using UnityEngine;
using SendState.DI;
using VContainer;
using VContainer.Unity;

using SendState.PlayerCube;
using SendState.MiniCubes;
using Mirror;

namespace SendState
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager instance { get; private set; }
        UpdateableFactory _factory;
        [Inject] LifetimeScope _lifetime;

        [SerializeField]
        private Vector3 _gravity = new Vector3(0.0f, -9.81f, 0.0f);
        [SerializeField]
        private CameraFollow _activeCamera;
        [SerializeField]
        private GameObject _cameraPrefab;

        [SerializeField]
        private GameObject _miniCubePrefab;
        [SerializeField]
        private GameObject _playerCubePrefab;
        [SerializeField]
        private GameObject _canvas;
        

        [SerializeField]
        private GameObject _box;

        [SerializeField]
        private GameObject _plane;

        private bool _gameStarted = false;
        private GameObject _objectsParent;
        private CubeBehaviour _currentPlayerCube;
        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(this);

            Physics.gravity = _gravity;
            _activeCamera = FindObjectOfType<CameraFollow>();
            if (_objectsParent == null) _objectsParent = Instantiate(new GameObject("ObjectsParent"), _box.transform);
            _factory = new UpdateableFactory(_lifetime);
            // _canvas.SetActive(false);
        }
        public UpdateableBehaviour CreateMiniCube(Vector3 position, Quaternion rotation = default, ulong withUID = 0)
        {
            var cube = _factory.Create<MiniCube>(_miniCubePrefab, _objectsParent.transform, withUID);
            cube.transform.localPosition = position;
            cube.transform.localRotation = rotation == default ? Quaternion.identity : rotation;
            cube.GetComponent<Attracted>().SetAttractedTo(_currentPlayerCube.gameObject);

            return cube.GetComponent<MiniCube>();
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
                    CreateMiniCube(position);
                }
            }
        }
        public UpdateableBehaviour CreatePlayerCube(ulong withUID = 0)
        {
            if (_currentPlayerCube != null) return null;

            CubeBehaviour cube = _factory.Create<CubeBehaviour>(_playerCubePrefab, _objectsParent.transform, withUID);

            cube.transform.parent = _objectsParent.transform;
            _currentPlayerCube = cube;
            return cube;
        }
        public UpdateableBehaviour CreateMainCamera(ulong withUID = 0)
        {
            _factory.RegisterAlreadyCreated(_activeCamera, withUID);

            _activeCamera.SetTarget(_currentPlayerCube.transform);
            return _activeCamera;
        }
        public void RestartTheGame(uint cubeCount = 1000)
        {
            if (_gameStarted)
            {
                Destroy(_objectsParent);
            }

            MainLogger.instance.Info("Game Started");

            Vector3 _planeSize = _plane.GetComponent<Renderer>().bounds.size / 2;
            Vector3 offset = new Vector3(-_planeSize.x / 2, 0, -_planeSize.z / 2);
            FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(_planeSize.x / 2), (int)(_planeSize.z / 2), 2f, cubeCount);

            _gameStarted = true;
        }
    }
}