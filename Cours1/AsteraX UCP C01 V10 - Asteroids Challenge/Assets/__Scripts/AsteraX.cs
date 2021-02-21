//#define DEBUG_Asterax_LogMethods
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteraX : MonoBehaviour {

	static private AsteraX _S;

	static List<Asteroid> ASTEROIDS;
	static List<Bullet> BULLETS;

	const float MIN_ASTEROID_DIST_FROM_PLAYER_SHIP = 5;



	[Header("Set in Inspector")]
	[Tooltip("This sets the AsteroidsScriptableObject to be used throughout the game.")]
	public AsteroidsScriptableObject asteroidsSO;


	private void Awake()
	{
#if DEBUG_Asterax_LogMethods
		Debug.Log("Asterax:Aweke()");
#endif
		S = this;
	}


	// Use this for initialization
	void Start () {
		ASTEROIDS = new List<Asteroid>();
		for(int i=0; i<3;i++)
		{
			SpawnParentAsteroid(i);
		}
	}

	void SpawnParentAsteroid(int i)
	{
		Asteroid ast = Asteroid.SpawnAsteroid();
		ast.gameObject.name = "Asteroid_" + i.ToString("00");

		Vector3 pos;
		do
		{
			pos = ScreenBounds.RANDOM_ON_SCREEN_LOC;
		} while ((pos - PlayerShip.POSITION).magnitude < MIN_ASTEROID_DIST_FROM_PLAYER_SHIP);

		ast.transform.position=pos;
		ast.size = asteroidsSO.initialSize;
	}



	static private AsteraX S
	{
		get
		{
			if(_S==null)
			{
				return null;
			}

			return _S;
		}
		set
		{
			if(_S != null)
			{
				Debug.LogError("S doit etre set un truc dan sle genre");
			}
			_S = value;
			
		}
		
	}

	static public AsteroidsScriptableObject AsteroidsSO
	{
		get
		{
			if(S!=null)
			{
				return S.asteroidsSO;
			}
			return null;
		}
	}

	static public void AddAsteroid( Asteroid asteroid)
	{
		if(ASTEROIDS.IndexOf(asteroid)==-1)
		{
			ASTEROIDS.Add(asteroid);
			
		}
	}

	static public void RemoveAsteroid(Asteroid asteroid)
	{
		if(ASTEROIDS.IndexOf(asteroid)!=-1)
		{
			ASTEROIDS.Remove(asteroid);
		}
	}
}
