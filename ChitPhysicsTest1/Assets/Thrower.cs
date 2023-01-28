using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject target;
    public GameObject ball;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ball.GetComponent<Ball>().ThrowAt(target.transform.position, Ball.Direction.POSITIVE, Ball.Axis.Z);
            ball.GetComponent<Ball>().Spin(Ball.SpinDirection.CLOCKWISE, 50, 5);
        }
    }
}
