using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LancelotInteraction : ItemInteraction
{
    enum WalkingDir
    {
        LEFT,
        RIGHT
    };

    enum Toy
    {
        NONE,
        BALL,
        STICK,
        FlYING_HOUSE
    };

    private Toy activeToy;

    private NavMeshAgent _agent;
    private Animator _animator;
    private WalkingDir _walkingDir = WalkingDir.LEFT;

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject house;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _animator = transform.Find("Sprite").GetComponent<Animator>();
    }
    public override void OnTimeChanged(int time)
    {
        
    }

    //The toys are not consumed so they can be used several times
    public override void OnUsableItemDrop(Item item)
    {
        switch (item)
        {
            case Item.BALLS:
                SetToy(Toy.BALL);
                break;
            case Item.WALKING_STICK_CRUSHED:
                SetToy(Toy.STICK);
                break;
        }
    }

    private void SetToy(Toy toy)
    {
        activeToy = toy;
        switch (activeToy)
        {
            case Toy.NONE:
                ball.SetActive(false);
                stick.SetActive(false);
                house.SetActive(false);
                break;
            case Toy.BALL:
                ball.SetActive(true);
                stick.SetActive(false);
                house.SetActive(false);
                break;
            case Toy.STICK:
                ball.SetActive(false);
                stick.SetActive(true);
                house.SetActive(false);
                break;
            case Toy.FlYING_HOUSE:
                ball.SetActive(false);
                stick.SetActive(false);
                house.SetActive(true);
                break;
        }
    }

    public override void SpecificMouseDownBehaviour()
    {
        switch (activeToy)
        {
            case Toy.NONE:
                
                break;
            case Toy.BALL:
                
                break;
            case Toy.STICK:
               
                break;
            case Toy.FlYING_HOUSE:
               
                break;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
