using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10f;
    
    public float zBound = 9;
    public float xBound = 23;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // If projectile goes out of bounds destroy it
        if (transform.position.z > zBound || transform.position.z < -zBound
            || transform.position.x > xBound || transform.position.x < -xBound)
        {
            Destroy(gameObject);
        }
    }
}
