using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : Animal
{
    protected GameObject grass;
    protected GameObject Lion;

    protected override void Initialize()
    {
        base.Initialize();
        Lion = GameObject.FindGameObjectsWithTag("lion")[0];
    }
    protected void Eat()
    {
        agent.SetDestination(grass.transform.position);
        if (agent.velocity == Vector3.zero && !agent.pathPending)
        {
            eatClock += Time.deltaTime;
            anim.SetBool("isEat", true);
        }
        if (eatClock >= eatDuration)
        {
            grass.GetComponent<DrinkingStation>().isAvailable = true;
            hunger = 0.0f;
            eatClock = 0.0f;
            afterEat = true;
            anim.SetBool("isEat", false);
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
        if (agent.isStopped)
            agent.isStopped = false;
    }

    protected override void WalkAround()
    {
        if (isIdle)
        {
            anim.SetFloat("movSpeed", 0.0f);
            agent.isStopped = true;
            idle_clock += Time.deltaTime;
            if (idle_clock >= idle_time)
            {
                agent.isStopped = false;
                float temp = WorldController.RandomFloat(0.0f, 1.0f);
                if (temp <= 0.33)
                {
                    isIdle = true;
                    idle_clock = 0.0f;
                }
                else
                {
                    generateNewDestination();
                    isIdle = false;
                }
            }
        }
        else
        {
            anim.SetFloat("movSpeed", agent.speed);
            agent.SetDestination(destination);
            float dist = agent.remainingDistance;
            if (agent.remainingDistance <= WorldController.remainingDistance)
            {
                float temp = WorldController.RandomFloat(0.0f, 1.0f);
                if (temp <= 0.33)
                {
                    isIdle = true;
                    idle_clock = 0.0f;
                }
                else
                {
                    generateNewDestination();
                    isIdle = false;
                }
            }
        }
        if(Vector3.Distance(transform.position, Lion.transform.position) <= 15.0f)
        {
            Vector3 DirToPlayer = transform.position - Lion.transform.position;

            Vector3 newPos = transform.position + DirToPlayer;

            destination = newPos;
            agent.SetDestination(destination);
        }
        else if (thirst >= 0.75f)
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
