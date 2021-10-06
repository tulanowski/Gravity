using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextAsset jsonDataFile;
    public GameObject celestialBodyPrefab;
    public GameObject starPrefab;
    public GameObject uiCanvas;
    public GameObject tracker;

    private GameObject panel;
    private Text nameText;
    private Dropdown dropdown;
    private GameObject selectedBody;
    private Text trackerNameText;
    private Text trackerDistanceText;

    private Dictionary<string, GameObject> celestialBodies = new Dictionary<string, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        panel = uiCanvas.transform.Find("CelestialBodyPanel").gameObject;
        nameText = panel.transform.Find("BodyName").GetComponent<Text>();
        dropdown = uiCanvas.transform.Find("CelestialBodyDropdown").GetComponent<Dropdown>();
        trackerNameText = tracker.transform.Find("TrackerName").GetComponent<Text>();
        trackerDistanceText = tracker.transform.Find("TrackerDistance").GetComponent<Text>();
        LoadCelestialBodies();
        InitializeDropdown();
    }

    void FixedUpdate()
    {

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100000))
            {
                Debug.Log("hit: " + hit.collider.gameObject.name);
                SelectCelestialBody(hit.collider.gameObject);
            } else
            {
                Debug.Log("no hit");
                DeselectCelestialBody();
            }
        }

    }

    private void Update()
    {
        if (selectedBody != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(selectedBody.transform.position);
            tracker.SetActive(!(screenPos.x < 0f | screenPos.y < 0f | screenPos.z < 0f));
            tracker.transform.position = new Vector3(screenPos.x, screenPos.y);

            float someRatio = 800f;
            float targetObjScale = selectedBody.transform.localScale.x;
            float dist = (Camera.main.transform.position - selectedBody.transform.position).magnitude;
            float size = Mathf.Clamp(someRatio * targetObjScale / dist, 30, 300);

            RectTransform rect = tracker.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(size, size);

            trackerDistanceText.text = "Distance: " + FormatNumber(dist) + " km";
        }
    }

    string FormatNumber(float number)
    {
        return number.ToString("F1") + " M";
    }

    void SelectCelestialBody(GameObject body)
    {
        panel.SetActive(true);
        nameText.text = body.name;
        trackerNameText.text = body.name;
        selectedBody = body;
    }

    void DeselectCelestialBody()
    {
        selectedBody = null;
        panel.SetActive(false);
        tracker.SetActive(false);
    }


    // Start is called before the first frame update
    void LoadCelestialBodies()
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
            celestialBodies.Add(newBody.name, newBody);
        }

        foreach (var planet in starSystemData.planets)
        {
            var newBody = Instantiate(celestialBodyPrefab);
            newBody.transform.parent = transform;
            var ctrl = newBody.GetComponent<CelestialBodyController>();
            ctrl.CreatePlanet(planet);
            celestialBodies.Add(newBody.name, newBody);
        }

        //firstStarController.BalanceMomentum();
    }

    void InitializeDropdown()
    {
        foreach (var pair in celestialBodies)
        {
            dropdown.options.Add(new Dropdown.OptionData(pair.Key));
        }
        dropdown.onValueChanged.AddListener(delegate { SelectCelestialBody(celestialBodies[dropdown.options[dropdown.value].text]); });
    }
}
