using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class enemyScript : MonoBehaviour
    {
        public NavMeshAgent agent;
        public ThirdPersonCharacter character;

        public enum State
        {
            PATROL,
            CHASE,
            INVESTIGATE
        }

        public State state;
        private bool alive;

        #region Camera Stuffs
        private bool playerInSight;
        public float fieldOfView = 110f;
        private SphereCollider sphereCollider;

        //Sight
        public GameObject player;
        public Collider playerCollider;
        public Camera myCamera;
        private Plane[] planes;
        #endregion

        #region Variables
        //Patrol
        public GameObject[] waypoints;
        private int waypointInd;
        public float patrolSpeed = 1.5f;

        //Chase
        public float chaseSpeed = 3f;
        public GameObject target;

        //Investigate
        private Vector3 investigateSpot;
        public float timer = 0;
        public const float investigateWait = 10;
    
        //Sight
        public float heightMultiplier;
        public float sightDistance = 5;
        #endregion

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            waypoints = GameObject.FindGameObjectsWithTag("Node");
            waypointInd = UnityEngine.Random.Range(0, waypoints.Length); //Might need System for Random

            playerCollider = player.GetComponent<Collider>();

            state = enemyScript.State.PATROL;

            alive = true;

            heightMultiplier = 1.4f;
        }

        private void Update()
        {
            StartCoroutine(FSM());
            Debug.Log(state);

            #region Camera
            planes = GeometryUtility.CalculateFrustumPlanes(myCamera);
            if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds))
            {
                Debug.Log("Player Spotted");
                CheckForPlayer();
            }
            else
            {

            }
            #endregion
        }

        IEnumerator FSM()
        {
            while (alive)
            {
                switch (state)
                {
                    case State.PATROL:
                        Patrol();
                        break;
                    case State.CHASE:
                        Chase();
                        break;
                    case State.INVESTIGATE:
                        Investigate();
                        break;
                    default:
                        break;
                }
                yield return null;
            }
        }

        void Patrol()
        {
            agent.speed = patrolSpeed;
            if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
                character.Move(agent.desiredVelocity, false, false);
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
            {
                waypointInd = UnityEngine.Random.Range(0, waypoints.Length); //Might need System for Random
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }

        void Chase()
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.transform.position);
            character.Move(agent.desiredVelocity, false, false);
        }

        void Investigate()
        {
            timer += Time.deltaTime;

            //agent.SetDestination(this.transform.position);
            agent.SetDestination(investigateSpot);
            //character.Move(Vector3.zero, false, false);
            transform.LookAt(investigateSpot);
            if (timer >= investigateWait)
            {
                state = enemyScript.State.PATROL;
                timer = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && state == State.PATROL)
            {
                state = enemyScript.State.INVESTIGATE;
                investigateSpot = other.gameObject.transform.position;
            }
        }

        #region Camera 
        void CheckForPlayer()
        {
            RaycastHit hit;
            //Debug.DrawRay(myCamera.transform.position, transform.forward * 10, Color.green);
            if (Physics.Raycast(myCamera.transform.position, transform.forward, out hit, 5) && (state != State.CHASE))
            {
                state = enemyScript.State.CHASE;
                target = hit.collider.gameObject;
            }
        }
        #endregion
    }
}