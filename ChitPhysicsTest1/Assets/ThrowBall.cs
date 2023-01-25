using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ThrowBall : MonoBehaviour
{
    Rigidbody rb;
    public GameObject target;
    DateTime start;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            start = DateTime.Now;
            rb.useGravity = true;
            float zDist = Mathf.Abs(target.transform.position.z - transform.position.z);
            float yDist = Mathf.Abs(target.transform.position.y - transform.position.y);
            //Debug.Log(yDist + " " + Mathf.Sqrt(yDist / Physics.gravity.magnitude) + " " + zDist);
            Vector3 correct_velocity = new Vector3(0, 0, zDist / Mathf.Sqrt(yDist / 5.12f));
            rb.velocity = new Vector3(0, 0, correct_velocity.z);
            //Debug.Log(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(DateTime.Now - start);
        //Debug.Log(collision.contacts[0].point);
    }
}
