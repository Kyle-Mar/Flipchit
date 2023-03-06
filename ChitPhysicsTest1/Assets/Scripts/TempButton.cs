using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class TempButton : MonoBehaviour
{
    NetworkManager networkManager;
    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

    }

    public async void StartHost()
    {
        networkManager.StartHost();
        string code = await networkManager.GetComponentInChildren<RelayManager>().CreateRelay();
        Debug.Log(code);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void StartClient()
    {
        //GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartClient();
        gameObject.transform.parent.gameObject.SetActive(false);

    }
}
