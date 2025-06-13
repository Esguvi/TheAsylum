using Photon.Pun;
using UnityEngine;
using System.Collections;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject enemyPrefab;
    public GameObject flashlight;

    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;
    public Transform spawnPointPlayer3;
    public Transform spawnPointEnemy;
    public Transform objectsParent;

    private const string ENEMY_KEY = "EnemyActorNumber";
    private bool isEnemy = false;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENEMY_KEY))
            {
                AssignRandomEnemy();
            }
            //StartCoroutine(SpawnObjects());
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

        GameObject playerInstance = null;

        if (isEnemy)
        {
            playerInstance = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPointEnemy.position, spawnPointEnemy.rotation);
        }
        else
        {
            int index = PhotonNetwork.LocalPlayer.ActorNumber % 3;

            switch (index)
            {
                case 0:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPointPlayer1.position, spawnPointPlayer1.rotation);
                    break;
                case 1:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab2.name, spawnPointPlayer2.position, spawnPointPlayer2.rotation);
                    break;
                case 2:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab3.name, spawnPointPlayer3.position, spawnPointPlayer3.rotation);
                    break;
            }
        }

        PlayerSetup setup = playerInstance.GetComponent<PlayerSetup>();
        if (setup != null)
        {
            setup.IsLocalPlayer();
        }
    }

    private IEnumerator SpawnObjects()
    {
        yield return new WaitForSeconds(0.1f);

        objectsParent = GameObject.Find("Objects")?.transform;
        if (objectsParent == null)
        {
            yield break;
        }

        Vector3 flashlightPosition = new Vector3(-860.280029f, 232.059998f, -723.179993f);
        Quaternion flashlightRotation = Quaternion.Euler(272.821106f, 269.999603f, 115.179367f);

        flashlight = PhotonNetwork.Instantiate("Flashlight", flashlightPosition, flashlightRotation);

        yield return new WaitForSeconds(0.2f);

        if (flashlight != null)
        {
            flashlight.transform.SetParent(objectsParent);
            flashlight.name = "Flashlight";
        }
    }
}