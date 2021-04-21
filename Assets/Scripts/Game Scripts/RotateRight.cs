using UnityEngine;

public class RotateRight : MonoBehaviour
{
    private const float speed = 60.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate object right by speed
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
