using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyController : MonoBehaviour
{
    public static float planetScale = 500;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreatePlanet(CelestialBodyData planetData)
    {
        name = planetData.name;
        transform.position = new Vector3(planetData.distanceFromSun, 0, 0);
        //transform.localScale = planetData.diameter;
    }
}
