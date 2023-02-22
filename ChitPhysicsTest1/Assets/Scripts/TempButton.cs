using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TempButton : MonoBehaviour
{
    private void Start()
    {
    }

    public void StartHost()
    {
        Debug.Log("HELLO I AM HOST");
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartHost();
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void StartClient()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartClient();
        gameObject.transform.parent.gameObject.SetActive(false);

    }
}
