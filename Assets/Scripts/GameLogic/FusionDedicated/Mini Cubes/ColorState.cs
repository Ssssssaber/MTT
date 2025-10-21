using System.Collections;
using System.Collections.Generic;
using DI;
using UnityEngine;

public class ColorState : MonoBehaviour
{
    private Material _material;

    private Color _originalColor;
    public Color _interactedColor = Color.red;
    private Attracted _attracted;

    [SerializeField]
    private float _timeToColorFade = 5f;
    private float _fadeDeltaTimer = 0.1f;
    private bool _colorChanged = false;

    private float _timePassed = 0f;

    public void StartAttraction()
    {
        if (!_material) return;

        _material.color = _interactedColor;
        _colorChanged = true;
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
        SimulationUpdate(Time.deltaTime);
    }

    private void SimulationUpdate(float deltaTime)
    {
        if (!_colorChanged) return;

        _timePassed += deltaTime;
        _material.color = Color.Lerp(_material.color, _originalColor, deltaTime / _timeToColorFade);
        _attracted.PerformAtrraction(deltaTime);
        if (_timePassed >= _timeToColorFade)
        {
            _colorChanged = false;
            _timePassed = 0f;
        }
    }
}
