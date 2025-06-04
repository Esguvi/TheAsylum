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

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        
                        break; 
                    }
                }
            }

            yield return new WaitForSeconds(5); 
        }
    }
}
