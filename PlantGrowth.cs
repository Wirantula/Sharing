using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{

    public float growth = 1;
    public float growthMinimum = 0.3f;
    public float growthPerDay = 0.5f;
    public float growthTillMature = 2;
    public int resourceTypeNeeded;
    public float myResource;
    public float resourceNeeded = 1;
    public int day;
    public bool inEarthZone = false;
    public bool regrow = false;
    public bool isCreated;
    public Transform carrot;
    public GameObject carrotRespawn;
    public GameManager gameManager;
    public GroundData groundData;

    private void Awake()
    {
        resourceTypeNeeded = Mathf.RoundToInt(Random.Range(1, 3));
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        growthPerDay = 0.5f;
        day = gameManager.day;
        regrow = true;
    }

    private void FixedUpdate()
    {

        if (day < gameManager.day)
        {
            day++;
            Grow();
            //if (growth >= growthTillMature && regrow == true) { regrow = false; Procreate();}
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (growth < growthMinimum) { Die(); }
        if (groundData != null)
        {
            if (resourceTypeNeeded == 1) { myResource = groundData.resourceA; }
            else if (resourceTypeNeeded == 2) { myResource = groundData.resourceB; }
            else if (resourceTypeNeeded == 3) { myResource = groundData.resourceC; }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EarthZone")
        {
            groundData = other.GetComponent<GroundData>();
            inEarthZone = true;
        }

    }

    void Grow()
    {

        if (inEarthZone == true && myResource >= resourceNeeded)
        {
            growth += growthPerDay;
            if (resourceTypeNeeded == 1) { groundData.resourceA -= resourceNeeded; }
            else if (resourceTypeNeeded == 2) { groundData.resourceB -= resourceNeeded; }
            else if (resourceTypeNeeded == 3) {groundData.resourceC -= resourceNeeded; }
            carrot.localScale = new Vector3(growthPerDay * growth, growthPerDay * growth, growthPerDay * growth);
            if(growth > growthTillMature) { Procreate(); }
            
        }
        else if (inEarthZone == true  && myResource < resourceNeeded)
        {
            growth -= 0.4f;
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void Procreate()
    {
        if (!isCreated && growth >= growthTillMature)
        {
            regrow = true;
            isCreated = true;
            GameObject plant = Instantiate(carrotRespawn, this.transform.position, this.transform.rotation, GameObject.Find("EarthPlanet").transform);
            Debug.Log("instantiated");
            plant.transform.SetParent(GameObject.Find("EarthPlanet").transform);
            plant.transform.localPosition = new Vector3(Random.Range(-0.05f, 0.05f), this.transform.position.y + 0.2f, Random.Range(-0.001f, 0.02f) + 0.001f);
        }
    }
}
