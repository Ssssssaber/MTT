using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _miniCubePrefab;
    [SerializeField]
    private GameObject _playerCube;

    [SerializeField]
    private GameObject _box;
    [SerializeField]
    private GameObject _plane;

    private void SpawnMiniCube(Vector3 position)
    {
        var cube = Instantiate(_miniCubePrefab, _box.transform);
        cube.transform.localPosition = position;
        cube.GetComponent<Attracted>().SetAttractedTo(_playerCube);
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

    // Start is called before the first frame update
    void Start()
    {
        Vector3 _planeSize = _plane.GetComponent<Renderer>().bounds.size / 2;
        Vector3 offset = new Vector3(-_planeSize.x / 2, 0, -_planeSize.z / 2);
        FillPlaneWithCubes(_plane.transform.localPosition + offset, (int)(_planeSize.x / 2), (int)(_planeSize.z / 2), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
