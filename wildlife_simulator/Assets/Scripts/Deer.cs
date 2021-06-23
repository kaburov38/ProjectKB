using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deer : Herbivore
{        
    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
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
                Debug.Log("Poop");
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
}
