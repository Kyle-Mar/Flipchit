using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Thrower : MonoBehaviour
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
    void Start()
    {
        ballScript = ball.GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount <= 0)
        {
            return;
        }

        touch = Input.GetTouch(0);
        switch (touch.phase) {

            case TouchPhase.Began:
                beginTouchPosition = touch.position;
                break;

            case TouchPhase.Ended:
                endTouchPosition = touch.position;
                float varZ = CalculateVariance(touch);

                if (beginTouchPosition.x <= endTouchPosition.x && !ballScript.InPlay) {
                    ballScript.Spin(Ball.SpinDirection.CLOCKWISE, CalculateRotationSpeed(touch), 5);
                    ballScript.ThrowAt(target.transform.position, 0, varZ);
                }
                else if(beginTouchPosition.x > endTouchPosition.x && !ballScript.InPlay)
                {
                    ballScript.Spin(Ball.SpinDirection.COUNTERCLOCKWISE, CalculateRotationSpeed(touch), 5);
                    ballScript.ThrowAt(target.transform.position, 0, varZ);
                }

                if(beginTouchPosition == endTouchPosition)
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
