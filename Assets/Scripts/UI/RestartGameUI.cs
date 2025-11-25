using UnityEngine;

public class RestartGameUI : MonoBehaviour
{
    [SerializeField] private MirrorGameManager _gameManager;
    [SerializeField] private TMPro.TMP_InputField _inputField;
    public void OnRestartButtonClicked()
    {
        if (uint.TryParse(_inputField.text, out uint cubeCount))
        {
            _gameManager.GetPlayer().OnRestartButtonPressed(cubeCount);
        }
    }
}
