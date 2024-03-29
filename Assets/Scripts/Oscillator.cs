﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float movementPeriod = 2f;

    float movementFactor;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovementCall();
    }

    private void MovementCall()
    {
        if (movementPeriod <= Mathf.Epsilon) { return; } //protect against period is zero
        float cycles = Time.time / movementPeriod; // grows continuoally from 0

        const float tau = Mathf.PI * 2f;//about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);// goes from -1 to +1

        movementFactor = rawSinWave / 2f + .5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
