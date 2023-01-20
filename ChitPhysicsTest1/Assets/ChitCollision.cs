using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitCollision : MonoBehaviour
{
    Rigidbody rb;
    [Range(1, 30)]
    public int forceMultiplicationAmount = 10;
    [Range(1, 30)]
    public int maxForceMagnitude = 30;
    public bool multiplicationEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && multiplicationEnabled)
        {
            // multiply the force by distance between center of mass and the point of contact
            Vector3 forceToApply = collision.impulse * Vector3.Distance(rb.centerOfMass, collision.GetContact(0).point);
            forceToApply *= forceMultiplicationAmount;
            forceToApply = Vector3.ClampMagnitude(forceToApply, maxForceMagnitude);
            rb.AddForce(forceToApply);
        }
    }
}
