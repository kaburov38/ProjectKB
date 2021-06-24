using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : Carnivore
{
    // Start is called before the first frame update
    
    void Start()
    {
        base.Initialize();
        minRunningSpeed = 5.0f;
        maxRunningSpeed = 6.5f;
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
        }
    }

    void Eat()
    {
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
    protected override void WalkAround()
    {
        base.WalkAround();
        if (hunger >= 0.8f)
        {
            SeekPrey();
            agent.speed = WorldController.RandomFloat(minRunningSpeed, maxRunningSpeed);
            state = State.Eat;
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
}
