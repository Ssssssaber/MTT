using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGameUI : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMPro.TMP_InputField _inputField;

   
    public void OnRestartButtonClicked()
    {
        if (uint.TryParse(_inputField.text, out uint cubeCount))
        {
            _gameManager.RestartTheGame(cubeCount);
        }
    }
}
