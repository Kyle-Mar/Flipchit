using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class TempButton : MonoBehaviour
{
    NetworkManager networkManager;
    public TMP_Text GameCodeText;
    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        GameCodeText.gameObject.SetActive(false);
    }

    public async void StartHost()
    {

        // ask the relay manager for a new code after creating a new relay server.
        Result<string> codeOrError = await networkManager.GetComponentInChildren<RelayManager>().CreateRelay();
        
        if (codeOrError.Success)
        {
            GameCodeText.gameObject.SetActive(true);
            GameCodeText.text = codeOrError.Value;
        }

        //TODO: WE NEED TO IMPLEMENT UNITY LOBBY I THINK.
        //gameObject.transform.parent.gameObject.SetActive(false);
    }

    public async void StartClient()
    {
        //GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartClient();
        await networkManager.GetComponentInChildren<RelayManager>().JoinRelay(GameCode.Code);
        GameCodeText.gameObject.SetActive(false);
        //gameObject.transform.parent.gameObject.SetActive(false);
    }
}
