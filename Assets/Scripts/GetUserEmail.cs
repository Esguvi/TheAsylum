using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetUserEmail : MonoBehaviour
{
    private string apiUrl = "https://theasylum.vercel.app/api/getUserEmail"; 
    private string userEmail = "";

    void Start()
    {
        StartCoroutine(CheckForUserEmail());
    }

    IEnumerator CheckForUserEmail()
    {
        while (true) 
        {
            using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    userEmail = request.downloadHandler.text;
                    Debug.Log("Correo recibido: " + userEmail);

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        
                        break; 
                    }
                }
                else
                {
                    Debug.Log("Esperando email...");
                }
            }

            yield return new WaitForSeconds(5); 
        }
    }
}
