using FishNet;
using UnityEngine;

public class RestartGameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField _inputField;
    public void OnRestartButtonClicked()
    {
        if (!InstanceFinder.IsClientStarted) return;

        var gameManager = FishNetGameManager.Instance;
        if (gameManager == null) return;

        var player = gameManager.CurrentPlayerCube;
        if (player == null) return;

        if (uint.TryParse(_inputField.text, out uint cubeCount))
        {
            player.ServerRpcAskForRestartGame(cubeCount);
        }
    }
}
