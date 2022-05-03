using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_TP : MonoBehaviour
{


    [SerializeField] public Transform target;
    [SerializeField] public Transform origin;
    [SerializeField] public float TrainSpeed;
    bool moveNext = true;
    public float timeToNext = 2.0f;
    // Update is called once per frame
    void Update()
    {
  
            if (moveNext)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, TrainSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, target.position) <= 0)
            {
                StartCoroutine(TimeTMove());
               transform.position = origin.position;

            }

        
        IEnumerator TimeTMove()
        {
            moveNext = false;
            yield return new WaitForSeconds(timeToNext);
            moveNext = true;
        }
    }
}

