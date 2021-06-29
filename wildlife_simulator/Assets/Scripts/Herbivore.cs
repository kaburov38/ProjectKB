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
        if(state == State.Thirst)
        {
            water.GetComponent<DrinkingStation>().isAvailable = true;
        }
        if(state == State.Eat)
        {
            grass.GetComponent<DrinkingStation>().isAvailable = true;
        }
    }

    protected override void WalkAround()
    {
        agent.SetDestination(destination);
        float dist = agent.remainingDistance;
        if (agent.remainingDistance <= 3.0f)
        {
            generateNewDestination();
        }
        if (thirst >= 0.75f)
        {
            water = WorldControllerScript.FindClosestWater(transform.position);
            water.GetComponent<DrinkingStation>().isAvailable = false;
            //Debug.Log("drink");
            state = State.Thirst;
        }
        else if (hunger >= 0.8f)
        {
            grass = WorldControllerScript.FindClosestGrass(transform.position);
            grass.GetComponent<DrinkingStation>().isAvailable = false;
            //Debug.Log("eat");
            state = State.Eat;
        }
        else if (peeBar >= 1.0f)
        {
            //Debug.Log("pee");
            state = State.Pee;
        }
        else if (poopBar >= 1.0f)
        {
            //Debug.Log("poop");
            state = State.Poop;
        }
        else if (DayNightScript.isNight())
        {
            isSleeping = true;
            //Debug.Log("sleep");
            state = State.Sleep;
        }
    }
}
