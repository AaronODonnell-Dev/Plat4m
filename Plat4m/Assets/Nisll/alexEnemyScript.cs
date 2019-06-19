using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class alexEnemyScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;

    public enum State
    {
        PATROL,
        CHASE,
        ATTACK
    }

    private float _attackRange = 3f;
    private float _rayDistance = 3f;
    private float _stoppingDistance = 3f;

    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private alexEnemyScript _target;
    private State _currentState;

    //public State state;
    //private bool alive;

    //public GameObject[] waypoints;
    //private int waypointInd;
    //public float patrolSpeed = 1.5f;

    //public float chaseSpeed = 3f;
    //public GameObject target;

    void Start()
    {
        #region comment Start Code
        //agent = GetComponent<NavMeshAgent>();
        //character = GetComponent<ThirdPersonCharacter>();

        //agent.updatePosition = true;
        //agent.updateRotation = false;

        //waypoints = GameObject.FindGameObjectsWithTag("Node");
        //waypointInd = Random.Range(0, waypoints.Length);

        //state = alexEnemyScript.State.PATROL;

        //alive = true;
        #endregion
    }

    private void Update()
    {

        switch (_currentState)
        {
            case State.PATROL:

                if (NeedsDestination())
                {
                    GetDestination();
                }

                transform.rotation = _desiredRotation;

                transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                #region
                //var rayColor = IsPathBlocked() ? Color.red : Color.green;
                //Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                //while (IsPathBlocked())
                //{
                //    Debug.Log("Path Blocked");
                //    GetDestination();
                //}
                #endregion

                var targetToAggro = CheckForAggro();
                if (targetToAggro != null)
                {
                    _target = targetToAggro.GetComponent<alexEnemyScript>();
                    _currentState = State.CHASE;
                }
                break;

            case State.CHASE:
                if (_target == null)
                {
                    _currentState = State.PATROL;
                    return;
                }

                transform.LookAt(_target.transform);
                transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                if (Vector3.Distance(transform.position, _target.transform.position) < _attackRange)
                {
                    _currentState = State.ATTACK;
                }
                break;

            case State.ATTACK:
                if (_target != null)
                {
                    Destroy(_target.gameObject);
                }

                _currentState = State.PATROL;
                break;

            default:
                break;
        }
    }

    //private bool IsPathBlocked()
    //{
    //    Ray ray = new Ray(transform.position, _direction);
    //    var hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
    //    return hitSomething.Any();
    //}

    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
                               new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f,
                                   UnityEngine.Random.Range(-4.5f, 4.5f));

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
        float aggroRadius = 5f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                var drone = hit.collider.GetComponent<alexEnemyScript>();
                if (drone != null)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return drone.transform;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * aggroRadius, Color.black);
            }
            direction = stepAngle * direction;
        }

        return null;
    }
}

#region
/*
    using System.Linq;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Team Team => _team;
    [SerializeField] private Team _team;
    [SerializeField] private LayerMask _layerMask;
    
    private float _attackRange = 3f;
    private float _rayDistance = 5.0f;
    private float _stoppingDistance = 1.5f;
    
    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private Drone _target;
    private DroneState _currentState;
    

    private void Update()
    {
        switch (_currentState)
        {
            case DroneState.Wander:
            {
                if (NeedsDestination())
                {
                    GetDestination();
                }

                transform.rotation = _desiredRotation;

                transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                var rayColor = IsPathBlocked() ? Color.red : Color.green;
                Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                while (IsPathBlocked())
                {
                    Debug.Log("Path Blocked");
                    GetDestination();
                }

                var targetToAggro = CheckForAggro();
                if (targetToAggro != null)
                {
                    _target = targetToAggro.GetComponent<Drone>();
                    _currentState = DroneState.Chase;
                }
                
                break;
            }
            case DroneState.Chase:
            {
                if (_target == null)
                {
                    _currentState = DroneState.Wander;
                    return;
                }
                
                transform.LookAt(_target.transform);
                transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                if (Vector3.Distance(transform.position, _target.transform.position) < _attackRange)
                {
                    _currentState = DroneState.Attack;
                }
                break;
            }
            case DroneState.Attack:
            {
                if (_target != null)
                {
                    Destroy(_target.gameObject);
                }
                
                // play laser beam
                
                _currentState = DroneState.Wander;
                break;
            }
        }
    }

    private bool IsPathBlocked()
    {
        Ray ray = new Ray(transform.position, _direction);
        var hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
        return hitSomething.Any();
    }

    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
                               new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f,
                                   UnityEngine.Random.Range(-4.5f, 4.5f));

        _destination = new Vector3(testPosition.x, 1f, testPosition.z);

        _direction = Vector3.Normalize(_destination - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }

    private bool NeedsDestination()
    {
        if (_destination == Vector3.zero)
            return true;

        var distance = Vector3.Distance(transform.position, _destination);
        if (distance <= _stoppingDistance)
        {
            return true;
        }

        return false;
    }
    
    
    
    Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);
    
    private Transform CheckForAggro()
    {
        float aggroRadius = 5f;
        
        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for(var i = 0; i < 24; i++)
        {
            if(Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                var drone = hit.collider.GetComponent<Drone>();
                if(drone != null && drone.Team != gameObject.GetComponent<Drone>().Team)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return drone.transform;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);
            }
            direction = stepAngle * direction;
        }

        return null;
    }
}

public enum Team
{
    Red,
    Blue
}

public enum DroneState
{
    Wander,
    Chase,
    Attack
}
*/
#endregion

#region //IEnumerator FSM()
//{
//    while (alive)
//    {
//        switch (state)
//        {
//            case State.PATROL:
//                Patrol();
//                break;
//            case State.CHASE:
//                Chase();
//                break;
//            default:
//                break;
//        }
//        yield return null;
//    }
//}

//void Patrol()
//{
//    agent.speed = patrolSpeed;
//    if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
//    {
//        agent.SetDestination(waypoints[waypointInd].transform.position);
//        character.Move(agent.desiredVelocity, false, false);
//    }
//    else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
//    {
//        waypointInd = Random.Range(0, waypoints.Length);
//    }
//    else
//    {
//        character.Move(Vector3.zero, false, false);
//    }
//}

//void Chase()
//{
//    agent.speed = chaseSpeed;
//    agent.SetDestination(target.transform.position);
//    character.Move(agent.desiredVelocity, false, false);
//}

//private void OnTriggerEnter(Collider other)
//{
//    if (other.tag == "Player")
//    {
//        Debug.Log("OnTriggerEnter");
//        state = alexEnemyScript.State.CHASE;
//        target = other.gameObject;
//    }
//}
#endregion