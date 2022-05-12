using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_pruebas : MonoBehaviour
{
    
    
    [SerializeField]  public Transform[] target;
    [SerializeField]  public float platformSpeed;

    int curPos = 0;
    int nextPos = 1;
    bool moveNext = true;
    public float timeToNext = 2.0f;


    private void FixedUpdate()
    {
        if (moveNext) { 
            transform.position = Vector3.MoveTowards(transform.position, target[nextPos].position, platformSpeed * Time.deltaTime); }

        if (Vector3.Distance(transform.position, target[nextPos].position) <= 0) {
            StartCoroutine(TimeMove());
            curPos = nextPos;
            nextPos++;
        }
        if (nextPos > target.Length-1) {
            nextPos = 0;
            
        }
    }
    IEnumerator TimeMove()
    {
        moveNext = false;
        yield return new WaitForSeconds(timeToNext);
        moveNext = true;
    }
}
