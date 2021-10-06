using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyController : MonoBehaviour
{
    public static float G = 6.67408e-14f;
    public static float planetScale = 500;
    public static float starScale = 50;
    public static float timeScale = 3600 * 24 * 10;
    
    private Rigidbody myBody;
    private static List<Rigidbody> allBodies = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody == myBody) continue;
            
            Vector3 direction = otherBody.gameObject.transform.position - myBody.gameObject.transform.position;
            
            float magnitude = G * timeScale * timeScale * myBody.mass * otherBody.mass / direction.sqrMagnitude;
            //Debug.Log(myBody.gameObject.name + " <- " + otherBody.gameObject.name);
            //Debug.Log(myBody.mass + " * " + otherBody.mass + " / " + direction.sqrMagnitude);
            //Debug.Log(myBody.position);
            //Debug.Log(otherBody.position);
            //Debug.Log(direction);
            //Debug.Log(magnitude);
            Vector3 gravity = direction.normalized * magnitude;
            myBody.AddForce(gravity, ForceMode.Force);
        }
    }

    public void CreatePlanet(CelestialBodyData planetData)
    {
        name = planetData.name;
        transform.position = new Vector3(planetData.distanceFromSun, 0, 0);
        transform.localScale *= planetData.diameterNorm * planetScale;

        myBody = GetComponent<Rigidbody>();
        myBody.mass = planetData.mass;
        myBody.velocity = new Vector3(0, 0, planetData.orbitalVelocityNorm) * timeScale;

        allBodies.Add(myBody);

        var planetMat = FindMaterial(name);
        if (planetMat)
        {
            GetComponent<MeshRenderer>().material = planetMat;
        }
    }

    public void CreateStar(CelestialBodyData starData)
    {
        name = starData.name;
        transform.position = Vector3.zero;
        transform.localScale *= starData.diameterNorm * starScale;

        myBody = GetComponent<Rigidbody>();
        myBody.mass = starData.mass;

        allBodies.Add(myBody);

        var starMat = FindMaterial(name);
        if (starMat)
        {
            GetComponent<MeshRenderer>().material = starMat;
        }
    }

    //public void BalanceMomentum()
    //{
    //    Vector3 momentum = Vector3.zero;
    //    foreach (Rigidbody otherBody in allBodies)
    //    {
    //        if (otherBody == myBody) continue;
    //        momentum += otherBody.velocity * otherBody.mass;
    //    }
    //    Debug.Log(momentum);
    //    myBody.velocity = -momentum / myBody.mass;
    //    Debug.Log(myBody.velocity);
    //}

    Material FindMaterial(string name)
    {
        return Resources.Load<Material>("Materials/" + name);
    }
}
