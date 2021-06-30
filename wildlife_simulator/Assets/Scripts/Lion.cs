using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : Carnivore
{
    // Start is called before the first frame update
    private GameObject enemy;
    void Start()
    {
        base.Initialize();
        minRunningSpeed = 6.0f;
        maxRunningSpeed = 7.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
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
                Fight();
                break;
            case State.Run:
                RunAway();
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
        agent.speed -= Time.deltaTime / 10;
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
    }

    public void LoseFromBison()
    {
        SeekNewPrey(prey);
    }

    public void WinFromBison()
    {
        Destroy(prey);
        hunger = 0.0f;
        eatClock = 0.0f;
        afterEat = true;
        agent.speed = 3.5f;
        state = State.Awake;
    }

    public void LoseFromHyena()
    {
        //Debug.Log("run");
        agent.ResetPath();
        state = State.Run;
        agent.speed = 6.0f;
        generateNewDestination();
    }

    public void WinFromHyena()
    {
        Destroy(enemy);
        state = State.Awake;
    }

    public void RunAway()
    {
        agent.SetDestination(destination);
        if (agent.remainingDistance <= 3.0f && !agent.pathPending)
        {
            agent.speed = 3.5f;
            //Debug.Log(agent.remainingDistance);
            state = State.Awake;
        }
    }

    public void FightHyena(GameObject _hyena)
    {
        if (agent.isStopped)
            agent.isStopped = false;
        if (state == State.Thirst)
        {
            water.GetComponent<DrinkingStation>().isAvailable = true;
        }
        if (state != State.Run)
        {
            enemy = _hyena;
            state = State.Fight;
        }
    }

    public void Fight()
    {
        agent.SetDestination(enemy.transform.position);
    }
}
