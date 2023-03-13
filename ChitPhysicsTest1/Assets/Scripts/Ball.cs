using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlipChit
{

    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        Rigidbody rb;
        public bool InPlay = false;

        public enum Direction: int
        {
            NEGATIVE = -1,
            POSITIVE = 1
        }
    

        public enum Axis
        {
            X,
            Z
        }

        public enum SpinDirection
        {
            CLOCKWISE,
            COUNTERCLOCKWISE
        }


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            Debug.DrawRay(transform.position, transform.forward, Color.blue, 10f, false);    
        }

        /// <summary>
        /// Throws the ball towards the targetPosition
        /// </summary>
        /// <param name="targetPosition">Where the ball should be thrown</param>
        /// <param name="xVariance">[Optional] How much should we vary on the X axis</param>
        /// <param name="zVariance">[Optional] How much should we vary on the Z axis</param>

        // Does not support the ball being lower than the target.
        public void ThrowAt(Vector3 targetPosition, float xVariance = 0, float zVariance = 0) {
        
            rb.useGravity = true;
            InPlay = true;

            float yDist = Mathf.Abs(targetPosition.y - transform.position.y);

            float xDist = Mathf.Abs(targetPosition.x - transform.position.x + xVariance);
            float correctVelocityX = xDist / Mathf.Sqrt(yDist / 5.12f);
            // This determines the direction of the way we need to throw pretty easily.
            correctVelocityX *= Mathf.Sign(targetPosition.x - transform.position.x);

            float zDist = Mathf.Abs(targetPosition.z - transform.position.z + zVariance);
            float correctVelocityZ = zDist / Mathf.Sqrt(yDist / 5.12f);
            correctVelocityZ *= Mathf.Sign(targetPosition.z - transform.position.z);

            rb.velocity = new Vector3(correctVelocityX, 0, correctVelocityZ);
        }

        // Torque doesn't seem to actually move the ball in the air regardless of drag value
        // Thus we apply force in the direction of the spin

        /// <summary>
        /// Spins the Ball and Adds a Force parallel to the spinning ball
        /// </summary>
        /// <param name="spinDirection">Direction the ball is spinning.</param>
        /// <param name="spinAmount">How much the ball is spinning.</param>
        /// <param name="forceAcceleration">How quickly the force should be applied to move the ball</param>
        public void Spin(SpinDirection spinDirection, float spinAmount, float forceAcceleration)
        {
            InPlay = true;

            switch (spinDirection)
            {
                case SpinDirection.CLOCKWISE:
                    rb.AddTorque(new Vector3(0f, spinAmount, 0f));
                    StartCoroutine(doSpinForce(spinAmount, transform.right, forceAcceleration));
                    break;
                case SpinDirection.COUNTERCLOCKWISE:
                    rb.AddTorque(new Vector3(0f, -spinAmount, 0f));
                    StartCoroutine(doSpinForce(spinAmount, -transform.right, forceAcceleration));
                    break;
                default:
                    Debug.LogError($"[Ball.cs] Invalid Spin Direction {spinDirection}");
                    break;
            }
        }

        /// <summary>
        /// Reset the ball's gravity status, velocity and position
        /// </summary>
        /// <param name="position">The position to place the ball</param>
        public void ResetBall(Vector3 position)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            InPlay = false;
            transform.position = position;
        }

        /// <summary>
        /// Adds a cumulative force overtime to the ball because the torque on the ball does not move it in the air.
        /// </summary>
        /// <param name="finalSpinForce">The maximum spin force that will be applied to the ball before it stops applying force.</param>
        /// <param name="forceVector">The direction the force will be applied in.</param>
        /// <param name="forceAcceleration">How quickly will the force be applied. (Often results in less force being added)</param>
        /// <returns></returns>
        private IEnumerator doSpinForce(float finalSpinForce, Vector3 forceVector, float forceAcceleration)
        {
            // stop the ball from spinning after reset.
            if (!InPlay)
            {
                yield return null;
            }

            float curSpinForce = forceAcceleration;
            while(curSpinForce < finalSpinForce)
            {
                curSpinForce += forceAcceleration;
                rb.AddForce(forceVector * curSpinForce);
                // We want this to sync up with the physics step.
                yield return new WaitForSeconds(Time.fixedDeltaTime);

            }
            yield return null;
        }
    }
}