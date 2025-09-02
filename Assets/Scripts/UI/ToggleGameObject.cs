using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (!_objectToToggle) throw new System.Exception("No object assigned to toggle");
    }

    public void Toggle()
    {
        _objectToToggle.SetActive(!_objectToToggle.activeSelf);
    }
}
