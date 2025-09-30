using UnityEngine;
using Mirror;

public class MirrorColorState : NetworkBehaviour
{
    private Material _material;
    private Color _originalColor;
    public Color _interactedColor = Color.red;
    private MirrorAttracted _attracted;

    [SerializeField] private float _timeToColorFade = 5f;
    private float _fadeDeltaTimer = 0.1f;

    [SyncVar] private Color _currentColor;
    [SyncVar] private bool _colorChanged = false;

    private float _timePassed = 0f;

    public void StartAttraction()
    {
        if (!isServer) return;

        if (!_material) return;

        _currentColor = _interactedColor;
        _colorChanged = true;
        _timePassed = 0f;
    }

    private void Start()
    {
        _attracted = GetComponent<MirrorAttracted>();
        _material = GetComponent<MeshRenderer>().material;
        _originalColor = _material.color;
        _currentColor = _originalColor;
    }

    [ServerCallback]
    private void Update()
    {
        SimulationUpdate(Time.deltaTime);
    }

    private void SimulationUpdate(float deltaTime)
    {
        if (!_colorChanged) return;

        _timePassed += deltaTime;
        _currentColor = Color.Lerp(_currentColor, _originalColor, deltaTime / _timeToColorFade);
        _attracted.PerformAtrraction(deltaTime);  // This runs on server since Update is [ServerCallback]

        if (_timePassed >= _timeToColorFade)
        {
            _colorChanged = false;
            _timePassed = 0f;
            _currentColor = _originalColor;
        }
    }

    [ClientCallback]
    private void LateUpdate()
    {
        // Apply synced color to material on client
        _material.color = _currentColor;
    }
}
