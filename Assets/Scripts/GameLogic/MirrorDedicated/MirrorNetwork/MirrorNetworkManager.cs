using Mirror;
using UnityEngine;

public class MirrorNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);

        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
 
        if (player.TryGetComponent(out MirrorCubeAutoMovement autoMove))
        {
            autoMove.enabled = GameManager.Instance.GetCommandLineArguments().playerAuto;
        }

        NetworkServer.AddPlayerForConnection(conn, player);
        MirrorGameManager.instance.TargetRpcSetCurrentPlayerCube(conn, player.gameObject.GetComponent<MirrorCubeBehaviour>());
    }
}