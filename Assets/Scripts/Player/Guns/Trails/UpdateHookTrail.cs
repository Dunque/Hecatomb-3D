using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHookTrail : MonoBehaviour
{
    LineRenderer lr;
    Transform pos1;
    Transform pos2;


    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SetPositions(Transform pos1, Transform pos2)
    {
        this.pos1 = pos1;
        this.pos2 = pos2;

    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPositions(new Vector3[2] { pos1.position, pos2.position});
    }
}
