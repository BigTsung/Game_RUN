using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {
    [Range(0f, 1f)]
    public float rotateSpeed = 0.5f;
    public bool x = false;
    public bool y = true;
    public bool z = false;

    private Vector3 rotateAxis = Vector3.zero;

    private void Start()
    {
        if (x == true)
            rotateAxis.x = 1;
        if (y == true)
            rotateAxis.y = 1;
        if (z == true)
            rotateAxis.z = 1;
    }

    // Update is called once per frame
    void Update ()
    {
        transform.Rotate(rotateAxis, Time.deltaTime * rotateSpeed * 300f, Space.World);	
	}
}
