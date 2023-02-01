using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public bool chitHasBeenHit;
    Vector3 initUpDir;
    //Vector3 totalAngle = Vector3.zero;
    //Vector3 prevAngle = Vector3.zero;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float angleBetween = Vector3.Angle(initUpDir, transform.up);
        if (angleBetween > 90)
        {
            Debug.Log("FLIPPED");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            chitHasBeenHit = true;
            initUpDir = transform.up;
        }
    }
}
