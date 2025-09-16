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
        public UpdateableBehaviour CreateMiniCube(Vector3 position, Quaternion rotation = default)
        {
            var cube = Instantiate(_miniCubePrefab, _objectsParent.transform);
            cube.transform.localPosition = position;
            cube.transform.localRotation = rotation == default ? Quaternion.identity : rotation;

            cube.GetComponent<Attracted>().SetAttractedTo(_currentPlayerCube.gameObject);
            _lifetime.Container.InjectGameObject(cube);

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

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(this);

            Physics.gravity = _gravity;
            _activeCamera = FindObjectOfType<CameraFollow>();
            // _canvas.SetActive(false);
        }

        public UpdateableBehaviour CreatePlayerCube()
        {
            if (_currentPlayerCube != null) return null;

            var cube = Instantiate(_playerCubePrefab).GetComponent<CubeBehaviour>();
            RegisterPlayerCube(cube);
            return cube;
        }

        public UpdateableBehaviour CreateMainCamera()
        {
            // if (_activeCamera == null) _activeCamera = Instantiate(_cameraPrefab).GetComponent<CameraFollow>();

            _lifetime.Container.InjectGameObject(_activeCamera.gameObject);

            _activeCamera.SetTarget(_currentPlayerCube.transform);
            return _activeCamera;
        }

        public void RegisterPlayerCube(CubeBehaviour cube)
        {
            if (cube == null)
            {
                MainLogger.instance.Error("Cube is null");
                return;
            }

            if (_objectsParent == null) _objectsParent = Instantiate(new GameObject("ObjectsParent"), _box.transform);

            cube.transform.parent = _objectsParent.transform;


            _lifetime.Container.InjectGameObject(cube.gameObject);
            _currentPlayerCube = cube;

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