using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyena : Carnivore
{
    public GameObject lion;
    float battlePower;
    public float victoryPower;
    public float LionMinDistance;
    void Start()
    {
        base.Initialize();
        minRunningSpeed = 6.0f;
        maxRunningSpeed = 7.5f;
        lion = GameObject.FindGameObjectsWithTag("lion")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleeping)
        {
            thirst += thirstRate * Time.deltaTime;
            hunger += hungerRate * Time.deltaTime;
            if (afterDrink)
                peeBar += peeRate * Time.deltaTime;
            if (afterEat)
                poopBar += poopRate * Time.deltaTime;
        }
        switch (state)
        {
            case State.Awake:
                WalkAround();
                break;
            case State.Thirst:
                //Debug.Log("drink");
                Drink();
                break;
            case State.Pee:
                //Debug.Log("pee");
                Pee();
                break;
            case State.Poop:
                //Debug.Log("Poop");
                Poop();
                break;
            case State.Sleep:
                //Debug.Log("Sleep");
                Sleep();
                break;
            case State.Eat:
                //Debug.Log("Eat");
                Eat();
                break;
            case State.Fight:
                FightLion();
                break;
        }
    }

    void Eat()
    {
        if(prey == null)
        {
            SeekPrey();
        }
        agent.SetDestination(prey.transform.position);
        if(agent.remainingDistance <= 3.0f && prey.GetComponent<Animal>().getState() == State.Run)
        {
            Destroy(prey);
            hunger = 0.0f;
            eatClock = 0.0f;
            afterEat = true;
            agent.speed = 3.5f;
            state = State.Awake;
        }
    }
    public void FightLion()
    {
        agent.SetDestination(lion.transform.position);
        if (agent.remainingDistance <= 3.0f)
        {
            if (battlePower >= victoryPower)
            {
                lion.GetComponent<Lion>().LoseFromHyena();
                state = State.Awake;
            }
            else
            {
                lion.GetComponent<Lion>().WinFromHyena();
            }
        }
    }
    protected override void WalkAround()
    {
        if (isIdle)
        {
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
            agent.SetDestination(destination);
            float dist = agent.remainingDistance;
            if (agent.remainingDistance <= 3.0f)
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
        if (thirst >= 0.75f)
        {
            water = WorldControllerScript.FindClosestWater(transform.position);
            water.GetComponent<DrinkingStation>().isAvailable = false;
            //Debug.Log("drink");
            state = State.Thirst;
        }
        else if (hunger >= 0.8f)
        {
            SeekPrey();
            agent.speed = WorldController.RandomFloat(minRunningSpeed, maxRunningSpeed);
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
        else if (Vector3.Distance(transform.position,lion.transform.position) <= LionMinDistance && lion.GetComponent<Lion>().getState() != State.Run)
        {
            if (agent.isStopped)
                agent.isStopped = false;
            battlePower = WorldController.RandomFloat(0.0f, 1.0f);
            //Debug.Log(lion.GetComponent<Lion>().getState());
            state = State.Fight;
            lion.GetComponent<Lion>().FightHyena(this.gameObject);
        }
    }

    public override void SeekPrey()
    {
        prey = WorldControllerScript.FindClosestDeer(transform.position);
        prey.GetComponent<Herbivore>().chased(this.gameObject);
    }

    public override void SeekNewPrey(GameObject excluded)
    {
        prey = WorldControllerScript.FindNewClosestDeer(transform.position, excluded);
        prey.GetComponent<Herbivore>().chased(this.gameObject);
    }
}
