using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    Rigidbody rb;

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
    /// <param name="depthVariance">[Optional] The inaccuracy value in the axis being thrown at</param>
    /// <param name="direction">In the positive or negative direction</param>
    /// <param name="axis">Axis X or Z</param>
    public void ThrowAt(Vector3 targetPosition,  Direction direction, Axis axis, float depthVariance = 0) {
        
        rb.useGravity = true;

        float correctVelocity = 0;
        float yDist = Mathf.Abs(targetPosition.y - transform.position.y);

        switch (axis)
        {
            case Axis.X:
                float xDist = Mathf.Abs(targetPosition.x - transform.position.x + depthVariance);
                correctVelocity = xDist / Mathf.Sqrt(yDist / 5.12f);
                
                rb.velocity = new Vector3( (int) direction * correctVelocity, 0, 0);
                break;

            case Axis.Z:
                float zDist = Mathf.Abs(targetPosition.z - transform.position.z + depthVariance);
                correctVelocity = zDist / Mathf.Sqrt(yDist / 5.12f);
                
                rb.velocity = new Vector3(0, 0, correctVelocity * (int) direction);
                break;
            default:
                Debug.LogError($"[Ball.cs] Invalid Axis Value: {axis}");
                break;
        }

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

    private IEnumerator doSpinForce(float finalSpinForce, Vector3 forceVector, float forceAcceleration)
    {
        float curSpinForce = forceAcceleration;
        while(curSpinForce < finalSpinForce)
        {
            curSpinForce += forceAcceleration;
            rb.AddForce(forceVector * curSpinForce);
            yield return new WaitForSeconds(Time.fixedDeltaTime);

        }
        yield return null;
    }
}
