using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlipChit
{
    [RequireComponent(typeof(Rigidbody))]
    public class ScoreDetection : MonoBehaviour
    {
        // Start is called before the first frame update

        bool chitHasBeenHit;
        bool chitHasFlipped;
        Vector3 initUpDir;
        Rigidbody rb;

        // this is called after the points are calculated.
        // This will be used in order to send new information to the 
        // Game manager script.
        public delegate void OnScore(int points);
        public static event OnScore OnPointsCalculated;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (chitHasBeenHit == false)
            {
                return;
            }
            float angleBetween = Vector3.Angle(initUpDir, transform.forward);

            // this is basically a guess
            // 100 and not 90 to account for the degree of stability of the chit.

            if (angleBetween > 100)
            {
                chitHasFlipped = true;
            }
            if (rb.IsSleeping())
            {
                CalculatePoints(chitHasBeenHit, chitHasFlipped);
                chitHasBeenHit = false;
            }
        }

        private void CalculatePoints(bool hit, bool flipped)
        {
            int points = 0;
            if (hit)
            {
                points++;
            }
            if (flipped)
            {
                points += 2;
            }
            OnPointsCalculated?.Invoke(points);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                chitHasBeenHit = true;
                chitHasFlipped = false;
                initUpDir = transform.forward;
            }
        }
    }
}