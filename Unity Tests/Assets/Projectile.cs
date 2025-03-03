/*using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchForce = 20f;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.forward * launchForce, ForceMode.Impulse);
        }
    }
}*/

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchForce = 20f;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.forward * launchForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Projectile hit {collision.gameObject.name} with force {rb.velocity.magnitude}");
    }
}
