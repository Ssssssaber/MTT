using UnityEngine;
using Fusion;

public class RestartGameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField _inputField;
    public void OnRestartButtonClicked()
    {
		var gameManager = GameManager.Instance;
		if (gameManager == null) return;

		var player = gameManager.CurrentPlayerCube;
		if (player == null) return;

		if (uint.TryParse(_inputField.text, out uint cubeCount))
		{
			player.AskForRestart(cubeCount);
		}
    }
}
