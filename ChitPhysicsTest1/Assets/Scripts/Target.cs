using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlipChit
{
    public class Target : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject ObjectToTarget;
        public float DevianceAmount = 3f;

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PlaceAroundObject(DevianceAmount);
            }
        }

        public void PlaceAroundObject(float devianceAmount)
        {
            Vector3 devianceCircle = new Vector3(Random.insideUnitCircle.x, 0f, Random.insideUnitCircle.y) * devianceAmount;
            transform.position = ObjectToTarget.transform.position + devianceCircle;
        }
    }
}
