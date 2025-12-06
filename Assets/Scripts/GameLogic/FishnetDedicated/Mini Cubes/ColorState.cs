using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class ColorState : NetworkBehaviour
{
    private Material _material;
    private Color _originalColor;
    public Color _interactedColor = Color.red;
    private readonly SyncVar<Color> _currentColor = new SyncVar<Color>();
    private Attracted _attracted;

    [SerializeField]
    private float _timeToColorFade = 5f;
    private float _fadeDeltaTimer = 0.1f;
    private bool _colorChanged = false;

    private float _timePassed = 0f;

    [Server]
    public void StartAttraction()
    {
        if (!_material) return;

        _material.color = _interactedColor;
        _colorChanged = true;
    }

    private void Awake()
    {
        _currentColor.OnChange += OnCurrentColorChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        _attracted = GetComponent<Attracted>();
        _material = GetComponent<MeshRenderer>().material;
        _originalColor = _material.color;
    }

    private void Update()
    {
        if (!IsServerInitialized) return;
        SimulationUpdate(Time.deltaTime);
    }

    [Server]
    private void SimulationUpdate(float deltaTime)
    {
        if (!_colorChanged) return;

        _timePassed += deltaTime;
        _currentColor.Value = Color.Lerp(_material.color, _originalColor, deltaTime / _timeToColorFade);
        _attracted.PerformAtrraction(deltaTime);
        if (_timePassed >= _timeToColorFade)
        {
            _colorChanged = false;
            _timePassed = 0f;
			_currentColor.Value = _originalColor;
        }
    }

    private void OnCurrentColorChange(Color prev, Color next, bool asServer)
    {
        _material.color = next;
    }
}
