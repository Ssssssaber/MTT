using UnityEngine;
using Fusion;

public class ColorState : NetworkBehaviour
{
    private Material _material;

    private Color _originalColor;
    public Color _interactedColor = Color.red;
    private Attracted _attracted;

    [Networked] private Color syncedColor { get; set; }

    [SerializeField]
    private float _timeToColorFade = 5f;
    private bool _colorChanged = false;

    private float _timePassed = 0f;

    void Awake()
    {
        _attracted = GetComponent<Attracted>();
        _material = GetComponent<MeshRenderer>().material;
        _originalColor = _material.color;
    }

    public override void Spawned()
    {
        base.Spawned();
        syncedColor = _originalColor;
    }

    public void StartAttraction()
    {
        if (!_material) return;

        syncedColor = _interactedColor;
        _colorChanged = true;
    }

    public override void FixedUpdateNetwork()
    {
        SimulationUpdate(Runner.DeltaTime);
    }

    public override void Render()
    {
        _material.color = syncedColor;
    }

    private void SimulationUpdate(float deltaTime)
    {
        if (!_colorChanged) return;

        _timePassed += deltaTime;
        syncedColor = Color.Lerp(_material.color, _originalColor, deltaTime / _timeToColorFade);
        _attracted.PerformAtrraction(deltaTime);
        if (_timePassed >= _timeToColorFade)
        {
            _colorChanged = false;
            _timePassed = 0f;
            syncedColor = _originalColor;
        }
    }
}
