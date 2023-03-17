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

    enum NavigationGoal
    {
        KNUT,
        BEGGER,
        KIOSK,
        STASH,
        HOME_KNUT
    };

    enum Toy
    {
        NONE,
        BALL,
        STICK,
        ASPERIN,
        FlYING_HOUSE
    };

    private NavigationGoal navGoal;
    private Toy activeToy;

    private NavMeshAgent _agent;
    private Animator _animator;
    private WalkingDir _walkingDir = WalkingDir.LEFT;
    private bool _isWalking = false;

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject aperin;

    [SerializeField] private Transform posKnut;
    [SerializeField] private Transform posBeggar;
    [SerializeField] private Transform posKioskSeller;
    [SerializeField] private Transform posStash;
    [SerializeField] private Transform posHomeKnut;

    [SerializeField] private GameObject prefabAsperin;

    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        GoToPosition(NavigationGoal.BEGGER);
    }

    private void Update()
    {
        if (_isWalking && _agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            if(navGoal == NavigationGoal.STASH && activeToy == Toy.BALL)
            {
                SetToy(Toy.ASPERIN);
                GoToPosition(NavigationGoal.HOME_KNUT);
            } 
            else if (navGoal == NavigationGoal.STASH && activeToy == Toy.STICK)
            {
                SetToy(Toy.FlYING_HOUSE);
                GoToPosition(NavigationGoal.KNUT);
            }
            else if (navGoal == NavigationGoal.HOME_KNUT && activeToy == Toy.ASPERIN)
            {
                Instantiate(prefabAsperin, posHomeKnut.position, Quaternion.identity);
                SetToy(Toy.NONE);
                GoToPosition(NavigationGoal.KIOSK);
            }
            else
            {
                _isWalking = false;
                _animator.SetBool("walk", false);
            }
           
        }
    }

    private void MoveCharacterToPosition(Vector3 position)
    {
        Debug.Log("LANCELOT GO TO " + position);
        if (position.x < transform.position.x)
        {
            _animator.SetBool("right", false);
            _animator.SetBool("walk", true);
            _walkingDir = WalkingDir.LEFT;
        }
        else
        {
            _animator.SetBool("right", true);
            _animator.SetBool("walk", true);
            _walkingDir = WalkingDir.RIGHT;
        }

        _isWalking = true;
        Debug.Log("Move Lancelot to " + position.x + " - " + position.y);
        _agent.SetDestination(new Vector3(position.x, position.y, 0f));
        Debug.Log("LANCELOT DESTIMATION" + _agent.nextPosition);
    }

    private void GoToPosition(NavigationGoal navGoal)
    {
        this.navGoal = navGoal;
        switch (navGoal)
        {
            case NavigationGoal.KNUT:
                MoveCharacterToPosition(posKnut.position);
                break;
            case NavigationGoal.HOME_KNUT:
                MoveCharacterToPosition(posHomeKnut.position);
                break;
            case NavigationGoal.BEGGER:
                MoveCharacterToPosition(posBeggar.position);
                break;
            case NavigationGoal.KIOSK:
                MoveCharacterToPosition(posKioskSeller.position);
                break;
            case NavigationGoal.STASH:
                MoveCharacterToPosition(posStash.position);
                break;
        }
    }

    public override void OnTimeChanged(int time)
    {
        
    }

    public override void OnUsableItemDrop(Item item)
    {
        RemoveFromInventory(item);
        switch (item)
        {
            case Item.BALLS:
                
                SetToy(Toy.BALL);
                GoToPosition(NavigationGoal.STASH);
                break;
            case Item.WALKING_STICK_CRUSHED:
                SetToy(Toy.STICK);
                GoToPosition(NavigationGoal.STASH);
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
                aperin.SetActive(false);
                break;
            case Toy.BALL:
                ball.SetActive(true);
                stick.SetActive(false);
                house.SetActive(false);
                aperin.SetActive(false);
                break;
            case Toy.STICK:
                ball.SetActive(false);
                stick.SetActive(true);
                house.SetActive(false);
                aperin.SetActive(false);
                break;
            case Toy.FlYING_HOUSE:
                ball.SetActive(false);
                stick.SetActive(false);
                house.SetActive(true);
                aperin.SetActive(false);
                break;
            case Toy.ASPERIN:
                ball.SetActive(false);
                stick.SetActive(false);
                house.SetActive(false);
                aperin.SetActive(true);
                break;
            default:
                ball.SetActive(false);
                stick.SetActive(false);
                house.SetActive(false);
                aperin.SetActive(false);
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
}
