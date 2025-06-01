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
    public GameObject flashlight;

    [Header("Spawn Points")]
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
            Debug.LogError("No estás en ninguna sala. Volviendo al menú principal.");
            return;
        }

        if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENEMY_KEY))
        {
            AssignRandomEnemy();
        }


        StartCoroutine(SpawnObjects());
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
            //playerInstance = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPointEnemy.position, spawnPointEnemy.rotation);
            playerInstance = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPointPlayer1.position, spawnPointPlayer1.rotation);
            Debug.Log("Has sido asignado como enemigo.");
        }
        else
        {
            int index = PhotonNetwork.LocalPlayer.ActorNumber % 3;

            switch (index)
            {
                case 0:
                    //playerInstance = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPointPlayer1.position, spawnPointPlayer1.rotation);
                    break;
                case 1:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab2.name, spawnPointPlayer2.position, spawnPointPlayer2.rotation);
                    break;
                case 2:
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab3.name, spawnPointPlayer3.position, spawnPointPlayer3.rotation);
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


    private IEnumerator SpawnObjects()
    {
        yield return new WaitForSeconds(0f);

        objectsParent = GameObject.Find("Objects")?.transform;

        Vector3 flashlightPosition = new Vector3(-860.280029f, 232.059998f, -723.179993f);
        Quaternion flashlightRotation = Quaternion.Euler(272.821106f, 269.999603f, 115.179367f);

        flashlight = PhotonNetwork.Instantiate(flashlight.name, flashlightPosition, flashlightRotation);

        if (objectsParent != null && flashlight != null)
        {
            flashlight.transform.SetParent(objectsParent);
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto padre 'Objects' o la linterna no fue instanciada correctamente.");
        }
    }


}
