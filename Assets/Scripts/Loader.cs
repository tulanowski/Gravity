using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    public TextAsset jsonDataFile;
    public GameObject celestialBodyPrefab;
    public GameObject starPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StarSystemData starSystemData = JsonUtility.FromJson<StarSystemData>(jsonDataFile.text);
        CelestialBodyController firstStarController = null;

        foreach (var star in starSystemData.stars)
        {
            var newBody = Instantiate(starPrefab);
            newBody.transform.parent = transform;
            var ctrl = newBody.GetComponent<CelestialBodyController>();
            ctrl.CreateStar(star);
            if (firstStarController == null)
            {
                firstStarController = ctrl;
            }
        }

        foreach (var planet in starSystemData.planets)
        {
            var newBody = Instantiate(celestialBodyPrefab);
            newBody.transform.parent = transform;
            var ctrl = newBody.GetComponent<CelestialBodyController>();
            ctrl.CreatePlanet(planet);
        }

        firstStarController.BalanceMomentum();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
