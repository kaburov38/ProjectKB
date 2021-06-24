using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : Animal
{
    protected GameObject grass;


    protected void Eat()
    {
        agent.SetDestination(grass.transform.position);
        if (agent.velocity == new Vector3(0.0f, 0.0f, 0.0f))
        {
            eatClock += Time.deltaTime;
        }
        if (eatClock >= eatDuration)
        {
            grass.GetComponent<DrinkingStation>().isAvailable = true;
            hunger = 0.0f;
            eatClock = 0.0f;
            afterEat = true;
            state = State.Awake;
        }
    }

    public virtual void chased(GameObject _predator)
    {

    }
    protected override void WalkAround()
    {
        base.WalkAround();
        if(hunger >= 0.8f)
        {
            grass = WorldControllerScript.FindClosestGrass(transform.position);
            grass.GetComponent<DrinkingStation>().isAvailable = false;
            state = State.Eat;
        }
    }
}
