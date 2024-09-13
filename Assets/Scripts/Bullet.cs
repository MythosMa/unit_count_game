using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int groundRange = 30;
    [SerializeField] float bulletSpeed = 2000f;
    private Rigidbody bulletRb;
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        bulletRb.AddRelativeForce(Vector3.up * bulletSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -groundRange || transform.position.x > groundRange || transform.position.z < -groundRange || transform.position.z > groundRange)
        {
            Destroy(gameObject);
        }
    }
}
