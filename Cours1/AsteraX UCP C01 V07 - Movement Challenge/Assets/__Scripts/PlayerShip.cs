using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{

    static private PlayerShip _S;
    static public PlayerShip S
    {
        get
        {
            return _S;
        }
        private set
        {
            if(_S != null)
            {
                Debug.LogWarning("Second attempt to set PlayerShip singleton _S");
            }
            _S = value;
        }
    }

    [Header("Set in Inspector")]
    public float speed = 10f;

    public GameObject bulletPrefab;

    Rigidbody rb;


    // Start is called before the first frame update
    void Awake()
    {
        _S = this;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float axisX = CrossPlatformInputManager.GetAxis("Horizontal");
        float axisY = CrossPlatformInputManager.GetAxis("Vertical");

        //transform.Translate(Vector3.right * axisX * speed * Time.deltaTime);
        //transform.Translate(Vector3.up * axisY * speed * Time.deltaTime);

        Vector3 vel = new Vector3(axisX, axisY);

        if(vel.magnitude>1)
        {
            vel.Normalize();
        }

        rb.velocity = vel * speed;

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }


    void Fire()
    {
        //prendre la direction de la souris
        Vector3 mPos = Input.mousePosition;
        mPos.z = -Camera.main.transform.position.z;
        Vector3 mPos3D = Camera.main.ScreenToWorldPoint(mPos);

        //instancier la balle et mettre ça direction
        GameObject go = Instantiate<GameObject>(bulletPrefab);
        go.transform.position = transform.position;
		Debug.Log("go position" + go.transform.position);
		Debug.Log( "position" + transform.position);

        go.transform.LookAt(mPos3D);
    }


    static public float MAX_SPEED
    {
        get
        {
            return S.speed;
        }
    }
}
