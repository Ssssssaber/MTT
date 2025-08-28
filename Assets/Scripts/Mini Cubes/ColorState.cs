using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorState : MonoBehaviour
{
    private Material _material;

    private Color _originalColor;
    [SerializeField]
    private Color _interactedColor = Color.red;
    private Attracted _attracted;

    [SerializeField]
    private float _timeToColorFade = 5f;
    private bool _colorChanged = false;

    private float _timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _attracted = GetComponent<Attracted>();
        _originalColor = GetComponent<MeshRenderer>().material.color;
        _material = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (_colorChanged)
        {
            _timePassed += Time.deltaTime;
            _material.color = Color.Lerp(_material.color, _originalColor, Time.deltaTime / _timeToColorFade);
            _attracted.PerformAtrraction();
            if (_timePassed >= _timeToColorFade)
            {
                _colorChanged = false;
                _timePassed = 0f;
            }
            return;
        }
    }        

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CubeBehaviour>())
        {
            _material.color = _interactedColor;
            _colorChanged = true;
        }
    }
}
