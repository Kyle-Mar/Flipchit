using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlipChit
{
    public class GameManager : MonoBehaviour
    {
        // Presumably, you'd have two instances of a team with a score for each.
        int score = 0;
        // Start is called before the first frame update
        void Start()
        {
            ScoreDetection.OnPointsCalculated += UpdateScore;
        }

        void OnDisable()
        {
            ScoreDetection.OnPointsCalculated -= UpdateScore;    
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(score);
        }

        void UpdateScore(int points)
        {
            score += points;
        }
    }
}
