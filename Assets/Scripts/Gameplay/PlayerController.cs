using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private float moveSpeed = 10f;
    
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void FixedUpdate()
    {
        MoveingAndRotation();
        //Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        ////rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        //transform.position += direction * speed * Time.deltaTime;
    }

    private void MoveingAndRotation()
    {
        float horizontal = variableJoystick.Horizontal;
        float vertical = variableJoystick.Vertical;

        Vector3 moveVector = (Vector3.forward * vertical + Vector3.right * horizontal);
        Vector3 rotateVector = (Vector3.forward * vertical + Vector3.right * horizontal);

        Vector3 moveDirection = new Vector3(vertical, 0, horizontal);
        //Vector3 rotateDirection = new Vector3(rotateJoystick.Vertical, 0, rotateJoystick.Horizontal);

        //float angle = Vector3.Angle(moveDirection, rotateDirection);


        transform.Translate(moveVector * moveSpeed / 2f * Time.deltaTime, Space.World);

        Vector3 resultRotate = Quaternion.LookRotation(rotateVector).eulerAngles;
        //resultRotate.y += rotateAngleFix;
        transform.rotation = Quaternion.Euler(resultRotate);

    }
}
