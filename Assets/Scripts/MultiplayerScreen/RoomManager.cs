using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public PhotonView player;
    public PhotonView player2;
    public PhotonView player3;
    public PhotonView playerEnemy;
    public Transform spawnPoint;
    public Transform spawnPointEnemy;

    private bool isEnemy = false;
    private const string ENEMY_KEY = "EnemyActorNumber";

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (player == null || player2 == null || player3 == null || playerEnemy == null || spawnPoint == null || spawnPointEnemy == null)
        {
            Debug.LogError("Hay prefabs o spawn points sin asignar en el Inspector.");
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
    }

    private IEnumerator SpawnPlayerWhenReady()
    {
        while (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENEMY_KEY))
        {
            yield return null;
        }

        int enemyActorNumber = (int)PhotonNetwork.CurrentRoom.CustomProperties[ENEMY_KEY];
        isEnemy = (PhotonNetwork.LocalPlayer.ActorNumber == enemyActorNumber);

        GameObject _player = null;

        if (isEnemy)
        {
            _player = PhotonNetwork.Instantiate(playerEnemy.name, spawnPointEnemy.position, spawnPointEnemy.rotation);
        }
        else
        {
            int index = PhotonNetwork.LocalPlayer.ActorNumber % 3;

            switch (index)
            {
                case 0:
                    _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation);
                    break;
                case 1:
                    _player = PhotonNetwork.Instantiate(player3.name, spawnPoint.position, spawnPoint.rotation);
                    break;
                case 2:
                    _player = PhotonNetwork.Instantiate(player3.name, spawnPoint.position, spawnPoint.rotation);
                    break;
            };
        }

        PlayerSetup setup = _player.GetComponent<PlayerSetup>();
        if (setup != null)
        {
            setup.IsLocalPlayer();
        }
    }
}
