using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    Rigidbody rb;
    [Range(300, 450)]
    public int horizontalforce = 100;
    [Range(50, 100)]
    public int verticalforce = 100;

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
            Debug.Log("HELLO");
            rb.useGravity = true;
            rb.AddForce(new Vector3(0, verticalforce, horizontalforce));
        }
    }
}
