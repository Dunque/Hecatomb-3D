using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHookTrail : MonoBehaviour
{
    LineRenderer lr;
    Transform pos1;
    Transform pos2;
    Vector3 pos2v;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SetPositions(Transform pos1, Transform pos2)
    {
        this.pos1 = pos1;
        this.pos2 = pos2;
    }
    public void SetPositions(Transform pos1, Vector3 pos2)
    {
        this.pos1 = pos1;
        this.pos2v = pos2;
    }

    // Update is called once per frame
    void Update()
    {
        if (pos2 != null)
            lr.SetPositions(new Vector3[2] { pos1.position, pos2.position});
        else
            lr.SetPositions(new Vector3[2] { pos1.position, pos2v });
    }
}
