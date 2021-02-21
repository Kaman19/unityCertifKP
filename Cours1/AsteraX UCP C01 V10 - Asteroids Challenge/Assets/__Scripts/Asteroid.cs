using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OffScreenWrapper))]
public class Asteroid : MonoBehaviour
{

	[Header("Set Dynamically")]
	public int size = 3;
	public bool immune = false;

	Rigidbody rb;
	OffScreenWrapper offScreenWrapper;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		offScreenWrapper = GetComponent<OffScreenWrapper>();
	}

	// Use this for initialization
	void Start()
	{
		AsteraX.AddAsteroid(this);

		transform.localScale = Vector3.one * size * AsteraX.AsteroidsSO.asteroidScale;
		if (parentIsAsteroid)
		{
			InitAsteroidChild();
		}
		else
		{
			InitAsteroidParent();
		}

		if (size > 1)
		{
			Asteroid ast;
			for (int i = 0; i < AsteraX.AsteroidsSO.numSmallerAsteroidsToSpawn; i++)
			{
				ast = SpawnAsteroid();
				ast.size = size - 1;
				ast.transform.SetParent(transform);
				Vector3 relPos = Random.onUnitSphere / 2;
				ast.transform.rotation = Random.rotation;
				ast.transform.localPosition = relPos;

				ast.gameObject.name = gameObject.name + "_" + i.ToString("00");
			}
		}
	}



	static public Asteroid SpawnAsteroid()
	{
		GameObject aGO = Instantiate<GameObject>(AsteraX.AsteroidsSO.GetAsteroidPrefab());
		Asteroid ast = aGO.GetComponent<Asteroid>();
		return ast;
	}

	bool parentIsAsteroid
	{
		get
		{
			return (parentAsteroid != null);
		}
	}

	Asteroid parentAsteroid
	{
		get
		{
			if (transform.parent != null)
			{
				Asteroid parentAsteroid = transform.parent.GetComponent<Asteroid>();
				if (parentAsteroid != null)
				{
					return parentAsteroid;
				}

			}
			return null;
		}
	}

	private void OnDestroy()
	{
		AsteraX.RemoveAsteroid(this);
	}

	public void InitAsteroidParent()
	{

		offScreenWrapper.enabled = true;
		rb.isKinematic = false;

		Vector3 pos = transform.position;
		pos.z = 0;
		transform.position = pos;

		InitVelocity();
	}

	public void InitAsteroidChild()
	{
		offScreenWrapper.enabled = false;
		rb.isKinematic = true;

		transform.localScale = transform.localScale.ComponentDivide(transform.parent.lossyScale);
	}

	public void InitVelocity()
	{

		Vector3 vel;
		if (ScreenBounds.OOB(transform.position))
		{
			vel = ((Vector3)Random.insideUnitSphere * 4) - transform.position;
			vel.Normalize();
		}
		else
		{
			do
			{
				vel = Random.insideUnitSphere;
				vel.Normalize();
			} while (Mathf.Approximately(vel.magnitude, 0f));

			vel = vel * Random.Range(AsteraX.AsteroidsSO.minVel, AsteraX.AsteroidsSO.maxVel) / (float)size;
			rb.velocity = vel;

			rb.angularVelocity = Random.insideUnitSphere * AsteraX.AsteroidsSO.maxAngularVel;


		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(parentIsAsteroid)
		{
			parentAsteroid.OnCollisionEnter(collision);
			return;
		}

		GameObject otherGO = collision.gameObject;

		if(otherGO.tag=="Bullet" || otherGO.transform.root.gameObject.tag=="Player")
		{
			if(otherGO.tag=="Bullet")
			{
				Destroy(otherGO);
			}

			if(size>1)
			{
				Asteroid[] children = GetComponentsInChildren<Asteroid>();
				for(int i=0;i<children.Length;i++)
				{
					if(children[i]==this || children[i].transform.parent != transform)
					{
						continue;
					}
					children[i].transform.SetParent(null, true);
					children[i].InitAsteroidParent();
				}
			}

			Destroy(gameObject);
		}
	}
}
