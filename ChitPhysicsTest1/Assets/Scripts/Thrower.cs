using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace FlipChit
{
    public class Thrower : NetworkBehaviour
    {
        // Start is called before the first frame update

        public GameObject target;
        public GameObject ball;
        public GameObject StartPos;
        Ball ballScript;
        void Start()
        {
            target = GameObject.Find("Target");
            ball = GameObject.Find("Ball");
            ballScript = ball.GetComponent<Ball>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !ballScript.InPlay)
            {
                if (IsClient) {
                    ThrowServerRpc();        
                }
                else
                {
                    ballScript.ThrowAt(target.transform.position);
                    ballScript.Spin(Ball.SpinDirection.CLOCKWISE, 0, 0);
                }
                
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ballScript.ResetBall(StartPos.transform.position);
            }
        }

        // Ask the server to throw the ball pretty please :)
        [ServerRpc]
        void ThrowServerRpc()
        {
            ballScript.ThrowAt(target.transform.position);
            ballScript.Spin(Ball.SpinDirection.CLOCKWISE, 0, 0);
        }

    }
}
