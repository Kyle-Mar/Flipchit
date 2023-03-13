using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update

    Lobby hostLobby;
    float heartbeatTimer;

    const float HEARTBEAT_TIMER_MAX = 15f;


    async void Start()
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

    void Update()
    {
        LobbyHeartbeat();
    }

    public async void CreateLobby(int maxPlayers, string lobbyName, bool isPrivate)
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions { 
                IsPrivate = isPrivate
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            hostLobby = lobby;

        } 
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async Task<Result<List<Lobby>>> ListLobbies(string maxPlayers, string availableSlots)
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 10,
                Filters = new List<QueryFilter> { 
                    new QueryFilter(QueryFilter.FieldOptions.MaxPlayers, maxPlayers, QueryFilter.OpOptions.EQ),
                    
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, availableSlots, QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> { 
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };


            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            return new Result<List<Lobby>>(queryResponse.Results);

        }
        catch (LobbyServiceException e) 
        {
            Debug.Log(e);
            return new Result<List<Lobby>>(null);
        }
    }
    public async void QuickJoin()
    {
        try 
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

            options.Filter = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };
            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    async void LobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f)
            {
                heartbeatTimer = HEARTBEAT_TIMER_MAX;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
}
