using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairAnimation : MonoBehaviour
{
    private const float rotationSpeed = 60f;
    private Vector3 scaleAmount = new Vector3(0.05f, 0.05f);

    // Update is called once per frame
    void Update()
    {
        // Rotate on z-axis slowly
        transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0)) {
            Scale(-scaleAmount);
        }

        if (Input.GetMouseButtonUp(0)) {
            Scale(scaleAmount);
        }
    }

    void Scale(Vector3 scaleAmount)
    {
        transform.localScale += scaleAmount;
    }
}
