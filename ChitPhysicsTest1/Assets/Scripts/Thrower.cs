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
        public Camera cam;

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
        // A Request from the client to the server to throw the ball given the parameters
        [ServerRpc]
        void ThrowServerRpc(float varZ, float rotSpeed, Ball.SpinDirection direction)
        {
            Throw(varZ, rotSpeed, direction);
        }

        /// <summary>
        /// Throws the ball.
        /// </summary>
        /// <param name="varZ">The variance in the target location on z-axis.</param>
        /// <param name="rotSpeed">The rotation speed of te ball</param>
        /// <param name="direction">The spin direction of the ball</param>
        void Throw(float varZ, float rotSpeed, Ball.SpinDirection direction)
        {
            ballScript.Spin(Ball.SpinDirection.CLOCKWISE, rotSpeed, 5);
            ballScript.ThrowAt(target.transform.position, 0, varZ);
        }

        /// <summary>
        /// Calculates the rotation speed the ball should have.
        /// </summary>
        /// <param name="touch">The touch action</param>
        /// <returns></returns>
        float CalculateRotationSpeed(Touch touch)
        {
            // Get either the maxSpinSpeed or the touch velocity whichever is smaller.
            float speed = Mathf.Min(Mathf.Abs(touch.deltaPosition.x / (touch.deltaTime * 5f)), maxSpinSpeed);
            // If it is less than the minimumSpinSpeed return that.
            return speed < minSpinSpeed ? minSpinSpeed : speed;
        }

        /// <summary>
        /// Calculate the variance of the throw from the actual target point.
        /// </summary>
        /// <param name="touch">The touch action</param>
        /// <returns></returns>
        float CalculateVariance(Touch touch)
        {
            // send a ray from the touch point down to the world.
            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            // This lets us know where the player lifts their finger from the screen in world space.
            // based off that we calculate the distance from that point and the target's position.

            float distance = Vector3.Distance(hit.point, target.transform.position);
            // Then based on the direction, we know the variance away from the target.
            float variance = distance * Mathf.Sign(hit.point.z - target.transform.position.z);

            //Debug.Log(distance + " " + variance);
            return variance;
        }
    }
}
