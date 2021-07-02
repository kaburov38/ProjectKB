using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject deer;
    public GameObject bison;
    public GameObject hyena;
    private int drinking_station;
    private int grass;
    private float clock = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        drinking_station = GameObject.FindGameObjectsWithTag("water").Length;
        grass = GameObject.FindGameObjectsWithTag("grass").Length;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;
        int bison = GameObject.FindGameObjectsWithTag("bison").Length;
        int deer = GameObject.FindGameObjectsWithTag("deer").Length;
        int hyena = GameObject.FindGameObjectsWithTag("hyena").Length;
        if (clock >= 20.0f && bison+deer+hyena+1 <= drinking_station - 3 && bison+deer <= grass - 3)
        {
            if(deer <= hyena)
            {
                //respawnDeer();
                clock = 0.0f;
            }
            else if(deer + bison <= hyena + 1)
            {
                //respawnHerbivore();
                clock = 0.0f;
            }
            else
            {
                //respawn();
                clock = 0.0f;
            }
        }
    }

    public void respawnDeer()
    {
        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-50.0f, 50.0f);
            float z = Random.Range(-50.0f, 20.0f);
            Instantiate(deer, new Vector3(x, 1.0f, z), Quaternion.identity);
        }
    }
    public void respawn()
    {
        int count = Random.Range(0, 3);
        for(int i = 0;i < count;i++)
        {
            int type = Random.Range(0, 5);
            float x = Random.Range(-50.0f, 50.0f);
            float z = Random.Range(-50.0f, 20.0f);
            if(type <= 2)
            {
                Instantiate(deer, new Vector3(x, 1.0f, z), Quaternion.identity);
            }
            else if(type <= 3)
            {
                Instantiate(bison, new Vector3(x, 1.0f, z), Quaternion.identity);
            }
            else
            {
                Instantiate(hyena, new Vector3(x, 1.0f, z), Quaternion.identity);
            }
        }
    }
    public void respawnHerbivore()
    {
        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            int type = Random.Range(0, 3);
            float x = Random.Range(-50.0f, 50.0f);
            float z = Random.Range(-50.0f, 20.0f);
            if (type <= 1)
            {
                Instantiate(deer, new Vector3(x, 1.0f, z), Quaternion.identity);
            }
            else 
            {
                Instantiate(bison, new Vector3(x, 1.0f, z), Quaternion.identity);
            }
        }
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

    public GameObject FindClosestDeer(Vector3 position)
    {
        GameObject[] deer;
        deer = GameObject.FindGameObjectsWithTag("deer");
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
        return closest;
    }

    public GameObject FindNewClosestDeer(Vector3 position, GameObject exclude)
    {
        GameObject[] deer;
        deer = GameObject.FindGameObjectsWithTag("deer");
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
        return closest;
    }
}
