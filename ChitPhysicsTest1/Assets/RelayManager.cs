using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> CreateRelay()
    {
       
        try
        {
            // NOTE: Might need modification (Making space for 3 players?)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            return await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        } catch (RelayServiceException e)
        {
            Task<string> task = new Task<string>(() => "ERROR");
            Debug.Log(e);
            return await task;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
