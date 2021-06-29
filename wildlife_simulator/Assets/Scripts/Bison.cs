using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bison : Herbivore
{
    float battlePower;
    public float victoryPower;

    protected GameObject predator;
    void Start()
    {
        base.Initialize();
    }
    private void Awake()
    {
        base.Initialize();
    }
    void FightBack()
    {
        agent.SetDestination(predator.transform.position);
        if(agent.remainingDistance <= 3.0f)
        {
            if(battlePower >= victoryPower)
            {
                predator.GetComponent<Lion>().LoseFromBison();
                state = State.Awake;
            }
            else
            {
                predator.GetComponent<Lion>().WinFromBison();
            }
        }

    }

    public override void chased(GameObject _predator)
    {
        base.chased(_predator);
        predator = _predator;
        state = State.FightBack;
        battlePower = WorldController.RandomFloat(0.0f, 1.0f);
        //Debug.Log(battlePower);
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
            case State.FightBack:
                FightBack();
                break;
        }
    }
}
