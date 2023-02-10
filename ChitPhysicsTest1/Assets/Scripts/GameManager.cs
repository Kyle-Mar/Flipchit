using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace FlipChit
{

    // Return type wrapper
    public struct SpawnPos{

        public bool isSuccessful;
        public GameObject gameObject;
        public SpawnPosition spawnPosition;
    }

    public class GameManager : MonoBehaviour
    {
        // Presumably, you'd have two instances of a team with a score for each.
        int score = 0;
        // Start is called before the first frame update
        public GameObject[] TwoPlayerTeamSpawnPositions;
        public GameObject[] OnePlayerTeamSpawnPositions;

        void Start()
        {
            ScoreDetection.OnPointsCalculated += UpdateScore;
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        }

        void OnDisable()
        {
            ScoreDetection.OnPointsCalculated -= UpdateScore;    
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(score);
        }

        void UpdateScore(int points)
        {
            score += points;
        }

        // see https://docs-multiplayer.unity3d.com/netcode/current/basics/connection-approval/index.html
        // TODO: Modify spawn position.
        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log("HELLO");
            // The client identifier to be authenticated
            var clientId = request.ClientNetworkId;

            // Additional connection data defined by user code
            var connectionData = request.Payload;

            // Your approval logic determines the following values
            response.Approved = true;
            response.CreatePlayerObject = true;

            // The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
            response.PlayerPrefabHash = null;

            // Position to spawn the player object (if null it uses default of Vector3.zero)
            SpawnPos spawnPos = GetSpawnPosition(ref OnePlayerTeamSpawnPositions);
            response.Position = spawnPos.gameObject.transform.position;

            // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
            response.Rotation = Quaternion.identity;

            // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
            // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
            response.Reason = "Some reason for not approving the client";

            // If additional approval steps are needed, set this to true until the additional steps are complete
            // once it transitions from true to false the connection approval response will be processed.
            
            response.Pending = false;
        }
        
        public SpawnPos GetSpawnPosition(ref GameObject[] spawnPositions)
        {
            GameObject spawnPosition = null;

            for(int i = 0; i < spawnPositions.Length; i++)
            {
                if (!spawnPositions[i].GetComponent<SpawnPosition>().IsOccupied)
                {
                    spawnPosition = spawnPositions[i];
                    spawnPosition.GetComponent<SpawnPosition>().IsOccupied = true;
                    break;
                }
            }

            if (spawnPosition == null)
            {
                Debug.LogError("[GameManager.cs] No valid spawn position found on connection");
            }

            SpawnPos ret = new SpawnPos();
            ret.gameObject = spawnPosition;
            ret.spawnPosition = spawnPosition.GetComponent<SpawnPosition>();
            ret.isSuccessful = spawnPosition != null;

            return ret;

        }
    }
}
