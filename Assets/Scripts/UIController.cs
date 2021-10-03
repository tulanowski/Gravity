using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject panel;
    private Text nameText;


    // Start is called before the first frame update
    void Start()
    {
        nameText = panel.transform.Find("BodyName").GetComponent<Text>();
    }

    void FixedUpdate()
    {

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100000))
            {
                panel.SetActive(true);
                Debug.Log("hit: " + hit.collider.gameObject.name);
                nameText.text = hit.collider.gameObject.name;
            } else
            {
                Debug.Log("no hit");
                panel.SetActive(false);
            }
        }
    }
}
