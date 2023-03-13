using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
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
        // Initialize Unity Services, don't start until done.
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        // Sign into the auth service anon.
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<Result<string>> CreateRelay()
    {
       
        try
        {
            // NOTE: Might need modification (Making space for 3 players?)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            
            //Update the host information to the relay server.
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort) allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );
            
            // Start the host with the new information
            NetworkManager.Singleton.StartHost();
            
            // If we're able to make a new server return the game code task.
            string gameCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return new Result<string>(gameCode);


        } catch (RelayServiceException e)
        {
            
            Debug.Log(e);
            return new Result<string>(null);
        }

    }

    public async Task<Result<string>> JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with code: " + joinCode);
            
            // attempt to join the relay with the join code.
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            // set the client relay data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();

            return new Result<string>("SUCCESS");

        }
        catch (RelayServiceException e)
        {
            return new Result<string>(null);
        }
    }
        

    // Update is called once per frame
    void Update()
    {
        
    }
}
