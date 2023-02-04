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
        float maxSpinSpeed = 25;
        float minSpinSpeed = 2;
        public float maxThrowVar = 15;

        Vector2 beginTouchPosition, endTouchPosition;


        private Touch touch;
        // Start is called before the first frame update
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

            if (Input.touchCount <= 0)
            {
                return;
            }

            touch = Input.GetTouch(0);
            switch (touch.phase)
            {

                case TouchPhase.Began:
                    beginTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;

                    float varianceZ = CalculateVariance(touch);
                    float rotSpeed = CalculateRotationSpeed(touch);

                    if (beginTouchPosition.x <= endTouchPosition.x && !ballScript.InPlay)
                    {
                        if (IsClient)
                        {
                            ThrowServerRpc(varianceZ, rotSpeed, Ball.SpinDirection.CLOCKWISE);
                        }
                        else
                        {
                            Throw(varianceZ, rotSpeed, Ball.SpinDirection.CLOCKWISE);
                        }
                    }

                    else if (beginTouchPosition.x > endTouchPosition.x && !ballScript.InPlay)
                    {
                        if (IsClient)
                        {
                            ThrowServerRpc(varianceZ, rotSpeed, Ball.SpinDirection.COUNTERCLOCKWISE);
                        }
                        else
                        {
                            Throw(varianceZ, rotSpeed, Ball.SpinDirection.COUNTERCLOCKWISE);
                        }
                    }

                    if (beginTouchPosition == endTouchPosition)
                    {
                        ballScript.ResetBall(StartPos.transform.position);
                    }
                    break;

                default:
                    break;

            }

            /*
            if (Input.GetKeyDown(KeyCode.Space) && !ballScript.InPlay)
            {
                ballScript.ThrowAt(target.transform.position);
                ballScript.Spin(Ball.SpinDirection.CLOCKWISE, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ballScript.ResetBall(StartPos.transform.position);
            }*/

        }
        // This is a lot of data to send... How fix?
        [ServerRpc]
        void ThrowServerRpc(float varZ, float rotSpeed, Ball.SpinDirection direction)
        {
            Throw(varZ, rotSpeed, direction);
        }

        void Throw(float varZ, float rotSpeed, Ball.SpinDirection direction)
        {
            ballScript.Spin(Ball.SpinDirection.CLOCKWISE, rotSpeed, 5);
            ballScript.ThrowAt(target.transform.position, 0, varZ);
        }


        float CalculateRotationSpeed(Touch touch)
        {
            float speed = Mathf.Min(Mathf.Abs(touch.deltaPosition.x / (touch.deltaTime * 5f)), maxSpinSpeed);
            return speed < minSpinSpeed ? minSpinSpeed : speed;
        }

        float CalculateVariance(Touch touch)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);


            float distance = Vector3.Distance(hit.point, target.transform.position);
            float variance = distance * Mathf.Sign(hit.point.z - target.transform.position.z);

            Debug.Log(distance + " " + variance);
            return variance;
        }
    }
}
