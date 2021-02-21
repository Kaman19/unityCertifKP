using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltWithVelocity : MonoBehaviour
{

    [Tooltip("le nombre de degrée que le vaiseau vas basuculer à la vitesse max")]
    public int degrees = 30;
    public bool tiltTowards = true;

    private int prevDegrees = int.MaxValue;
    private float tan;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(degrees!=prevDegrees)
        {
            prevDegrees = degrees;
            tan = Mathf.Tan(Mathf.Deg2Rad * degrees);
        }
        Vector3 pitchDir = (tiltTowards) ? -rb.velocity : rb.velocity;
        pitchDir += Vector3.forward / tan * PlayerShip.MAX_SPEED;
        transform.LookAt(transform.position + pitchDir);
    }
}
