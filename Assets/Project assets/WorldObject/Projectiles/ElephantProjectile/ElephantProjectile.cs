/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class ElephantProjectile : Projectile 
{
	public float closeAreaEffect = 1.0f;
	public float mediumAreaEffect = 2.5f;
	public float farAreaEffect = 5.0f;

	public float closeDamage = 3.0f;
	public float mediumDamage = 2.0f;
	public float farDamage = 1.0f;

	private Transform explosive;

	public Vector3 spawnExplosion = new Vector3();

	void Awake ()
	{
		explosive = transform;
	}

	public void Start()
	{
		damage = 4;
	}

	public override void Update ()
	{
		base.Update ();
		explosive = transform;
	}

	protected override void takeDamage()
	{
		public static List< WorldObject > FindNearbyObjects(Vector3 position, float range) { //Fonction générant une liste d'objects proches, utilisé pour l'IA des unités detectant les cibles dans leur range.
			Collider[] hitColliders = Physics.OverlapSphere(position, range);
			HashSet< int > nearbyObjectIds = new HashSet< int >();
			List< WorldObject > nearbyObjects = new List< WorldObject >();
			for(int i = 0; i < hitColliders.Length; i++) {
				Transform parent = hitColliders[i].transform.parent;
				if(parent) {
					WorldObject parentObject = parent.GetComponent< WorldObject >();
					if(parentObject /*&& !nearbyObjectIds.Contains(parentObject.ObjectId)*//*) {
						//nearbyObjectIds.Add(parentObject.ObjectId);
						nearbyObjects.Add(parentObject);
					}
				}
			}
			return nearbyObjects;
		}
	}

	protected override bool HitSomething ()
	{
		/*
		Collider[] colls = Physics.OverlapSphere(explosive.position, farAreaEffect);
		foreach (Collider col in colls)
		{
			if (col.CompareTag("Ennemi"))
			{
				float distance = Vector3.Distance(col.transform.position, explosive.position);
				float damage = farDamage;

				if(distance <= closeAreaEffect)
				{
					damage = closeDamage;
				}

				else if(distance <= mediumAreaEffect)
				{
					damage = mediumDamage;
				}
				
				col.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			}
		}

		return base.HitSomething ();
	}
	
	protected override void InflictDamage ()
	{
		base.InflictDamage ();
	}*/
	/*	}*/