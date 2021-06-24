using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float RandomFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }

    public GameObject FindClosestWater(Vector3 position)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("water");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && go.GetComponent<DrinkingStation>().isAvailable)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public GameObject FindClosestGrass(Vector3 position)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("grass");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && go.GetComponent<DrinkingStation>().isAvailable)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public GameObject FindClosestPrey(Vector3 position)
    {
        GameObject[] deer;
        deer = GameObject.FindGameObjectsWithTag("deer");
        GameObject[] bison;
        bison = GameObject.FindGameObjectsWithTag("bison");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in deer)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        foreach (GameObject go in bison)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public GameObject FindNewClosestPrey(Vector3 position, GameObject exclude)
    {
        GameObject[] deer;
        deer = GameObject.FindGameObjectsWithTag("deer");
        GameObject[] bison;
        bison = GameObject.FindGameObjectsWithTag("bison");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in deer)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && !ReferenceEquals(go, exclude))
            {
                closest = go;
                distance = curDistance;
            }
        }
        foreach (GameObject go in bison)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && !ReferenceEquals(go, exclude))
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
