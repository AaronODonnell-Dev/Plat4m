using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class FinalAIScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;

    public enum State
    {
        PATROL,
        CHASE,
        ATTACK
    }

    private float _attackRange = 2f;
    private float _rayDistance = 3f;
    private float _stoppingDistance = 3f;
    private float attackTimer = 3f;
    public float chaseDistance; 
    public bool debug = true;
    public bool targetedPlayer = false;

    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    public CapsuleCollider _target;
    public GameObject AttackCube;
    private State _currentState;
    [Range(0, 5)]
    public float SightRayHeight; //1.4 seems good for default character.

    #region Nodeish Codeish ()
    //public State state;
    //private bool alive;

    public GameObject[] waypoints;
    private int waypointInd;
    public float patrolSpeed = 1.5f;

    public float chaseSpeed = 3f;
    public GameObject target;
    #endregion

    void Start()
    {
        #region Start Code
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        

        agent.updatePosition = true;
        agent.updateRotation = true;

        waypoints = GameObject.FindGameObjectsWithTag("Node");
        waypointInd = Random.Range(0, waypoints.Length);

        _currentState = State.PATROL;
        //state = alexEnemyScript.State.PATROL;

        //alive = true;
        #endregion
    }

    private void Update()
    {
        StartCoroutine(FSM());

    }

    IEnumerator FSM()
    {
        //Debug.Log("Target is " + _target);
        //timer for attacking so there is a delay between attacs.
        attackTimer -= Time.deltaTime;

        // testing each update for if a target is within he raycasts
        // then setting _target to be the colider of the target.
        var targetToAggro = CheckForAggro();
        if (targetToAggro != null)
        {
            _target = targetToAggro.GetComponent<CapsuleCollider>();
            Debug.Log("Distance between objects : " + Vector3.Distance(transform.position, _target.transform.position));
            if (targetToAggro.tag == "Player" || targetToAggro.tag == "Player2")
            {
                targetedPlayer = true;
            }
            else
            {
                targetedPlayer = false;
            }
        }

        switch (_currentState)
        {
            case State.PATROL:

                if (targetedPlayer == true)
                {
                    _currentState = State.CHASE;
                }
                else if (targetedPlayer == false)
                {
                    #region Node Pathing
                    if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
                    {
                        agent.SetDestination(waypoints[waypointInd].transform.position);                   
                    }
                    else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
                    {
                        waypointInd = Random.Range(0, waypoints.Length);
                    }
                    else
                    {
                        //agent.SetDestination(waypoints[waypointInd].transform.position);
                        //character.Move(agent.desiredVelocity, false, false);
                    }
                    #endregion
                }
                break;

            case State.CHASE:
                if (targetedPlayer == false)
                {
                    _currentState = State.PATROL;
                }
                else
                {
                    transform.LookAt(_target.transform);
                    MoveToAttackDistance();
                }
                break;

            case State.ATTACK:
                
                if (targetedPlayer == false)
                {
                    _currentState = State.PATROL;
                }
                else if ((Vector3.Distance(transform.position, _target.transform.position) <= chaseDistance) && (Vector3.Distance(transform.position, _target.transform.position) >= _attackRange))
                {
                    _currentState = State.CHASE;
                }
                else if (Vector3.Distance(transform.position, _target.transform.position) >= _rayDistance)
                {
                    targetedPlayer = false;
                    _currentState = State.PATROL;
                }
                else if (targetedPlayer)
                {
                    Attack();
                }
                break;

            default:
                break;
        }
        yield return null;
    }

    #region-Methods-

    private void MoveToAttackDistance()
    {
        /* 
         * moves to a point a short distance from the player.
         * when at this point sets its destination to be its own destination
         * then changes state to be attack where the all attacking logic is held.
        */

        if (Vector3.Distance(transform.position, _target.transform.position) >= _attackRange)
        {
            agent.SetDestination(_target.transform.position);
        }
        else if ((Vector3.Distance(transform.position, _target.transform.position) <= _attackRange))
        {
            agent.SetDestination(transform.position);
            _currentState = State.ATTACK;
        }
    }

    private void Attack()
    {
        /* set the position of the accack cube to be a foward facing offset of the enemy.
         * set the rotation of the attack cube to be the same as the enemy.
         * instantiate the attack cube.
         * the cube will then exicute its own code
         */

        if (attackTimer <= 0)
        {
            AttackCube.transform.position = transform.position + transform.forward;
            AttackCube.transform.rotation = transform.rotation;
            Instantiate(AttackCube);
            attackTimer = 3f;
        }
    }

    private void GetDestination()
    {
        //Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
        //                       new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f,
        //                           UnityEngine.Random.Range(-4.5f, 4.5f));

        Vector3 testPosition = (transform.position + (transform.forward * 4f));

        _destination = new Vector3(testPosition.x, 1f, testPosition.z);

        _direction = Vector3.Normalize(_destination - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }

    private bool NeedsDestination()
    {
        if (_destination == Vector3.zero)
        {
            return true;
        }
        var distance = Vector3.Distance(transform.position, _destination);
        if (distance <= _stoppingDistance)
        {
            return true;
        }
        return false;
    }

    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector2.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    private Transform CheckForAggro()
    {
        float aggroRadius = 7f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos + new Vector3(0, SightRayHeight, 0), direction, out hit, aggroRadius))
            {
                var targetPlayer = hit.collider.GetComponent<CapsuleCollider>();
                if (targetPlayer != null && (targetPlayer.tag == "Player" || targetPlayer.tag == "Player2"))
                {
                    Debug.DrawRay(pos + new Vector3(0, SightRayHeight, 0), direction * hit.distance, Color.red);
                    Debug.Log("(if) targetPlayer is " + targetPlayer);
                    //return transform minus the players height as to get a position that is on the ground
                    return targetPlayer.transform;
                    // method can not return position. logic must be done in the switch
                }
                else
                {
                    Debug.DrawRay(pos + new Vector3(0, SightRayHeight, 0), direction * hit.distance, Color.yellow);
                    Debug.Log("(else) targetPlayer is " + targetPlayer);
                }
            }
            else
            {
                Debug.DrawRay(pos + new Vector3(0, SightRayHeight, 0), direction * aggroRadius, Color.black);
            }
            direction = stepAngle * direction;
        }
        return null;
    }
    #endregion

    #region-OldSwitch-
    //Debug.Log("Target is " + _target);

    //switch (_currentState)
    //{
    //    case State.PATROL:

    //        #region Node Pathing
    //        agent.speed = patrolSpeed;  
    //        if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
    //        {
    //            agent.SetDestination(waypoints[waypointInd].transform.position);
    //            character.Move(agent.desiredVelocity, false, false);
    //        }
    //        else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
    //        {
    //            waypointInd = Random.Range(0, waypoints.Length);
    //        }
    //        else
    //        {
    //            character.Move(Vector3.zero, false, false);
    //        }
    //        #endregion

    //        //if (NeedsDestination())
    //        {
    //            GetDestination();
    //        }

    //        //transform.rotation = _desiredRotation;

    //        //transform.Translate(Vector3.forward * Time.deltaTime * 5f);


    //        var targetToAggro = CheckForAggro();

    //        if (targetToAggro != null)
    //        {
    //            _target = targetToAggro.GetComponent<BoxCollider>();
    //            _currentState = State.CHASE;
    //        }
    //        break;

    //    case State.CHASE:
    //        if (_target == null)
    //        {
    //            Debug.Log("CHASE");
    //            _currentState = State.PATROL;
    //            return;
    //        }

    //        transform.LookAt(_target.transform);
    //        transform.Translate(Vector3.forward * Time.deltaTime * 5f);

    //        if (Vector3.Distance(transform.position, _target.transform.position) < _attackRange)
    //        {
    //            _currentState = State.ATTACK;
    //        }
    //        break;

    //    case State.ATTACK:
    //        if (_target != null && playerHealth.currentHealth != 0)
    //        {
    //            Debug.Log("ATTACK");
    //            //playerHealth.currentHealth--;
    //            //Destroy(_target.gameObject);
    //            //Need to set instance to lower player health and fix the 45 degree angle the enemy kills him at
    //        }

    //        _currentState = State.PATROL;
    //        break;

    //    default:
    //        break;
    //}

    #endregion

    #region-Old Code-
                //if (NeedsDestination())
                //{
                    //GetDestination();
                //}

                //transform.rotation = _desiredRotation;

                //transform.Translate(Vector3.forward * Time.deltaTime * 5f);


                //var targetToAggro = CheckForAggro();

                //if (targetToAggro != null)
                //{
                //    _target = targetToAggro.GetComponent<CapsuleCollider>();
                //    _currentState = State.CHASE;
                //}
                #endregion
}

/*
 Use Time.DeltaTime for the enemys attack. Deals 50 damage
     */
