using System;
using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    private static GameObject templatLaser;
    LineRenderer line;
    Ray orientRay;
    private float velocity = 0.25f;
    private int numBounces = 0;
    int lifeTime = 10000;
    public static Laser MakeLaser(Ray ray)
    {
        templatLaser = GameObject.Find("laser");
        GameObject templateLaser = Instantiate(templatLaser);
        GameObject go = new GameObject("laser");
        Laser laser = go.AddComponent<Laser>();
        laser.orientRay = ray;
        laser.line = templateLaser.GetComponent<LineRenderer>();
        laser.line.SetPosition(0, ray.origin);
        laser.line.SetPosition(1, ray.GetPoint(1));
        laser.lifeTime = 5;
        Rigidbody body = go.AddComponent<Rigidbody>();
        body.drag = 0;
        body.angularDrag = 0;
        body.mass = 20;

        BoxCollider collider = go.AddComponent<BoxCollider>();
        collider.material = (PhysicMaterial) Resources.Load("Prefabs/perfect_bounce");

        Destroy(templateLaser,5);
        return laser;
    }
	// Use this for initialization
	void Start()
	{
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update ()
    {
	    if (line)
	    {
	        RaycastHit hit;
	        if (Physics.Raycast(orientRay, out hit, velocity))
	        {
                print("HIT");
                AudioSource source = hit.transform.gameObject.GetComponent<AudioSource>();
	            if (source)
	            {
	                source.pitch = (float) Math.Pow(1.25f, numBounces);
	                source.Play();
	            }
	            numBounces++;

                Vector3 normal = hit.normal;
                orientRay.direction = Vector3.Reflect(orientRay.direction, normal);
	            if (hit.rigidbody)
	            {

                   

                    hit.rigidbody.AddForceAtPosition(transform.forward * GetComponent<Rigidbody>().mass * 10, hit.point);
	                hit.rigidbody.velocity += new Vector3(0,20,0);
	            }
            }

	        line.SetPosition(0, orientRay.origin);
	        line.SetPosition(1, orientRay.GetPoint(1));
	        orientRay.origin = orientRay.GetPoint(velocity);
	    }
    }
}
