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
    //public alexEnemyScript _target; //GameObject
    public GameObject _target;
    private State _currentState;
    [Range(0,5)]
    public float SightRayHeight; //1.4 seems good for default character.

    #region Legacy Code (All Commented)
    //public State state;
    //private bool alive;

    //public GameObject[] waypoints;
    //private int waypointInd;
    //public float patrolSpeed = 1.5f;

    //public float chaseSpeed = 3f;
    //public GameObject target;
    #endregion

    void Start()
    {
        #region Start Code (All Commonted)
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
        Debug.Log("Target is " + _target);

        switch (_currentState)
        {
            case State.PATROL:

                if (NeedsDestination())
                {
                    GetDestination();
                }

                transform.rotation = _desiredRotation;

                transform.Translate(Vector3.forward * Time.deltaTime * 5f);
                

                var targetToAggro = CheckForAggro();
                if (targetToAggro != null)
                {
                    //_target = targetToAggro.GetComponent<alexEnemyScript>();
                    _target = targetToAggro.GetComponent<GameObject>();
                    _currentState = State.CHASE;
                }
                break;

            case State.CHASE:
                if (_target == null)
                {
                    Debug.Log("CHASE");
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
                    Debug.Log("ATTACK");
                    Destroy(_target.gameObject);
                }

                _currentState = State.PATROL;
                break;

            default:
                break;
        }
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
            if (Physics.Raycast(pos + new Vector3(0, SightRayHeight, 0), direction, out hit, aggroRadius))
            {
                //var targetEnemy = hit.collider.GetComponent<alexEnemyScript>();
                var targetEnemy = hit.collider.GetComponent<GameObject>();
                if (targetEnemy != null && (targetEnemy.tag == "Player" || targetEnemy.tag == "Player2"))
                {
                    Debug.DrawRay(pos + new Vector3(0, SightRayHeight, 0), direction * hit.distance, Color.red);
                    return targetEnemy.transform;
                }
                else
                {
                    Debug.DrawRay(pos + new Vector3(0, SightRayHeight, 0), direction * hit.distance, Color.yellow);
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
}



