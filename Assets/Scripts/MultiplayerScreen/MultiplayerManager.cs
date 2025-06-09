using Photon.Pun;
using UnityEngine;
using System.Collections;
using Photon.Realtime;
using System.Linq;

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

        if (PhotonNetwork.IsMasterClient)
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENEMY_KEY))
            {
                AssignRandomEnemy();
            }
            Debug.Log("Eres el Master Client. Asignando enemigo y creando objetos de juego.");
            StartCoroutine(SpawnObjects());
            Debug.Log("Objetos de juego creados correctamente.");
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
                    playerInstance = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPointPlayer1.position, spawnPointPlayer1.rotation);
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
        yield return new WaitForSeconds(0.1f);

        objectsParent = GameObject.Find("Objects")?.transform;
        if (objectsParent == null)
        {
            Debug.LogError("No se encontró el objeto 'Objects'");
            yield break;
        }

        Vector3 flashlightPosition = new Vector3(-860.280029f, 232.059998f, -723.179993f);
        Quaternion flashlightRotation = Quaternion.Euler(272.821106f, 269.999603f, 115.179367f);

        flashlight = PhotonNetwork.Instantiate("Flashlight", flashlightPosition, flashlightRotation);

        yield return new WaitForSeconds(0.5f); 

        if (flashlight != null)
        {
            flashlight.transform.SetParent(objectsParent);
            flashlight.name = "Flashlight";
            Debug.Log("Flashlight seteado correctamente como hijo de 'Objects'");
        }
        else
        {
            Debug.LogError("No se pudo instanciar la linterna");
        }
    }




}
