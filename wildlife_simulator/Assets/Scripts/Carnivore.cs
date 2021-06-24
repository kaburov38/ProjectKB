using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : Animal
{
    protected GameObject prey; 
    protected float minRunningSpeed;
    protected float maxRunningSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SeekPrey()
    {
        prey = WorldControllerScript.FindClosestPrey(transform.position);
        prey.GetComponent<Herbivore>().chased(this.gameObject);
    }

    public void SeekNewPrey(GameObject excluded)
    {
        prey = WorldControllerScript.FindNewClosestPrey(transform.position, excluded);
        prey.GetComponent<Herbivore>().chased(this.gameObject);
    }
}
