using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlipChit
{
    public class Thrower : MonoBehaviour
    {
        // Start is called before the first frame update

        public GameObject target;
        public GameObject ball;
        public GameObject StartPos;
        Ball ballScript;
        void Start()
        {
            ballScript = ball.GetComponent<Ball>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !ballScript.InPlay)
            {
                ballScript.ThrowAt(target.transform.position);
                ballScript.Spin(Ball.SpinDirection.CLOCKWISE, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ballScript.ResetBall(StartPos.transform.position);
            }
        }
    }
}
