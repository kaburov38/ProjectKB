using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    
    // Start is called before the first frame update
    protected float hunger = 0.0f;
    protected float thirst = 0.0f;
    protected NavMeshAgent agent;
    protected Vector3 destination;
    protected float drinkClock, poopClock, peeClock, eatClock; 
    protected float thirstRate, peeRate, poopRate, hungerRate;
    protected bool afterDrink = false;
    protected bool afterEat = false;
    protected float peeBar;
    protected float poopBar;
    protected bool isSleeping;
    protected DayNightCycle DayNightScript;
    protected WorldController WorldControllerScript;
    protected GameObject water; 
    protected int state = State.Awake;
    protected bool isIdle = false;
    protected float idle_time = 4.0f;
    protected float idle_clock = 0.0f;
    protected Animator anim;

    public GameObject DayNightController;
    public GameObject WorldControllerObject;
    public float drinkDuration = 2.0f;
    public float eatDuration = 2.0f;
    public float poopDuration = 2.0f;
    public float peeDuration = 2.0f;
    public float thirstTime;
    public float hungerTime;
    public float peeTime;
    public float poopTime;
    protected virtual void Initialize()
    {
        DayNightController = GameObject.FindGameObjectsWithTag("DayNightCycle")[0];
        WorldControllerObject = GameObject.FindGameObjectsWithTag("WorldController")[0];
        DayNightScript = DayNightController.GetComponent<DayNightCycle>();
        WorldControllerScript = WorldControllerObject.GetComponent<WorldController>();
        agent = GetComponent<NavMeshAgent>();
        generateNewDestination();
        thirstRate = 1.0f / thirstTime;
        hungerRate = 1.0f / hungerTime;
        peeRate = 1.0f / peeTime;
        poopRate = 1.0f / poopTime;
        anim = gameObject.GetComponent<Animator>();
    }

    public int getState()
    {
        return state;
    }
    protected virtual void WalkAround()
    {
        //if(isIdle)
        //{
        //    anim.SetFloat("movSpeed", 0.0f);
        //    agent.isStopped = true;
        //    idle_clock += Time.deltaTime;
        //    if(idle_clock >= idle_time)
        //    {
        //        agent.isStopped = false;
        //        float temp = WorldController.RandomFloat(0.0f, 1.0f);
        //        if (temp <= 0.33)
        //        {
        //            isIdle = true;
        //            idle_clock = 0.0f;
        //        }
        //        else
        //        {
        //            generateNewDestination();
        //            isIdle = false;
        //        }
        //    }
        //}
        //else
        //{
        //    anim.SetFloat("movSpeed", agent.speed);
        //    agent.SetDestination(destination);
        //    float dist = agent.remainingDistance;
        //    if (agent.remainingDistance <= WorldController.remainingDistance)
        //    {
        //        float temp = WorldController.RandomFloat(0.0f, 1.0f);
        //        if (temp <= 0.33)
        //        {
        //            isIdle = true;
        //            idle_clock = 0.0f;
        //        }
        //        else
        //        {
        //            generateNewDestination();
        //            isIdle = false;
        //        }
        //    }
        //}        
        //if(thirst >= 0.75f)
        //{
        //    water = WorldControllerScript.FindClosestWater(transform.position);
        //    water.GetComponent<DrinkingStation>().isAvailable = false;
        //    //Debug.Log("drink");
        //    state = State.Thirst;
        //}
        //else if(peeBar >= 1.0f)
        //{
        //    //Debug.Log("pee");
        //    state = State.Pee;
        //}
        //else if(poopBar >= 1.0f)
        //{
        //    //Debug.Log("poop");
        //    state = State.Poop;
        //}
        //else if(DayNightScript.isNight())
        //{
        //    isSleeping = true;
        //    //Debug.Log("sleep");
        //    state = State.Sleep;
        //}
    }

    protected void generateNewDestination()
    {
        destination.x = Random.Range(-200, 200); 
        destination.y = 1.0f;
        destination.z = Random.Range(-200, 200);        
    }

    protected void Drink()
    {
        agent.SetDestination(water.transform.position);
        if (agent.velocity == Vector3.zero && !agent.pathPending)
        {
            drinkClock += Time.deltaTime;
            anim.SetBool("isDrink", true);
        }
        if (drinkClock >= drinkDuration)
        {
            water.GetComponent<DrinkingStation>().isAvailable = true;
            thirst = 0.0f;
            drinkClock = 0.0f;
            afterDrink = true;
            anim.SetBool("isDrink", false);
            state = State.Awake;
        }
    }

    protected void Pee()
    {
        //anim.Play("Pee");
        anim.SetBool("isPee", true);
        agent.isStopped = true;
        peeClock += Time.deltaTime;
        if(peeClock >= peeDuration)
        {
            peeClock = 0.0f;
            peeBar = 0.0f;
            afterDrink = false;
            agent.isStopped = false;
            anim.SetBool("isPee", false);
            state = State.Awake;
        }
    }

    protected void Poop()
    {
        //anim.Play("Pee");
        anim.SetBool("isPoo", true);
        agent.isStopped = true;
        poopClock += Time.deltaTime;
        if (poopClock >= poopDuration)
        {
            poopClock = 0.0f;
            poopBar = 0.0f;
            afterEat = false;
            agent.isStopped = false;
            anim.SetBool("isPoo", false);
            state = State.Awake;
        }
    }

    protected virtual void Sleep()
    {
        //anim.Play("Sleep");
        anim.SetBool("isSleeping", true);
        agent.isStopped = true;
        if(!DayNightScript.isNight())
        {
            agent.isStopped = false;
            isSleeping = false;
            anim.SetBool("isSleeping", false);
            state = State.Awake;
        }
    }
}
