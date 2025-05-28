using Photon.Pun;
using UnityEngine;
using System.Collections;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    [Header("Prefabs")]
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject enemyPrefab;

    [Header("Spawn Points")]
    public Transform spawnPointPlayers;
    public Transform spawnPointEnemy;

    private const string ENEMY_KEY = "EnemyActorNumber";
    private bool isEnemy = false;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogError("No estás en ninguna sala. Volviendo al menú principal.");
            return;
        }

        if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENEMY_KEY))
        {
            AssignRandomEnemy();
        }

        StartCoroutine(SpawnPlayerWhenReady());
    }

    private void AssignRandomEnemy()
    {
        int randomIndex = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        int enemyActorNumber = PhotonNetwork.PlayerList[randomIndex].ActorNumber;

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(ENEMY_KEY, enemyActorNumber);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        Debug.Log($"Jugador con ActorNumber {enemyActorNumber} asignado como enemigo.");
    }

    private IEnumerator SpawnPlayerWhenReady()
    {
        while (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENEMY_KEY))
        {
            yield return null;
        }

        int enemyActorNumber = (int)PhotonNetwork.CurrentRoom.CustomProperties[ENEMY_KEY];
        isEnemy = (PhotonNetwork.LocalPlayer.ActorNumber == enemyActorNumber);

        GameObject playerInstance = null;

        if (isEnemy)
        {
            playerInstance = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPointEnemy.position, spawnPointEnemy.rotation);
            Debug.Log("Has sido asignado como enemigo.");
        }
        else
        {
            int index = PhotonNetwork.LocalPlayer.ActorNumber % 3;

            switch (index)
            {
                case 0:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPointPlayers.position, spawnPointPlayers.rotation);
                    break;
                case 1:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab2.name, spawnPointPlayers.position, spawnPointPlayers.rotation);
                    break;
                case 2:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab3.name, spawnPointPlayers.position, spawnPointPlayers.rotation);
                    break;
            }

            Debug.Log("Este jugador ha sido asignado como jugador normal.");
        }

        PlayerSetup setup = playerInstance.GetComponent<PlayerSetup>();
        if (setup != null)
        {
            setup.IsLocalPlayer();
        }
        else
        {
            Debug.LogWarning("Prefab no tiene PlayerSetup.");
        }
    }
}
