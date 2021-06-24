using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deer : Herbivore
{
    private float RunDistance = 10.0f;
    private bool isRunning = false;
    private float minRunningSpeed = 6.0f;
    private float maxRunningSpeed = 4.0f;

    protected GameObject predator;
    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
    }

    void Run()
    {
        float distance = Vector3.Distance(transform.position, predator.transform.position);
        if(distance <= RunDistance && !isRunning)
        {
            agent.speed = WorldController.RandomFloat(minRunningSpeed, maxRunningSpeed);
            isRunning = true;            
        }
        if(isRunning)
        {
            Vector3 DirToPlayer = transform.position - predator.transform.position;

            Vector3 newPos = transform.position + DirToPlayer;

            agent.SetDestination(newPos);
            if(distance >= 2 * RunDistance)
            {
                predator.GetComponent<Carnivore>().SeekNewPrey(this.gameObject);
                state = State.Awake;
                isRunning = false;
                agent.speed = 3.5f;
            }
        }
    }

    public override void chased(GameObject _predator)
    {
        predator = _predator;
        state = State.Run;
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
            case State.Run:
                Run();
                break;
        }
    }
}
