using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W)) // can thrust while rotating
        {
            print("thrusting");
        }

        if (Input.GetKey(KeyCode.A))
        {
            print("rotating left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("rotating right");
        }
    }
}
