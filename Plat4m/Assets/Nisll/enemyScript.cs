using niallEnemyController;
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
        public enemyController character;

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
        public Plane[] planes;
        #endregion

        #region Variables
        //Patrol
        public GameObject[] waypoints;
        public int waypointInd;
        public float patrolSpeed = 1.5f;

        //Chase
        public float chaseSpeed = 3f;
        public GameObject target;

        //Investigate
        public Vector3 investigateSpot;
        public float timer = 0;
        public const float investigateWait = 4;

        public Vector3 playerLocation;

        //Sight
        public float heightMultiplier;
        public float sightDistance = 5;
        #endregion

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<enemyController>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            // will lead to issues later. if all eneyms look for all nodes
            // might help to allow the nodes to be added to the object in the inspector

            //waypoints = GameObject.FindGameObjectsWithTag("Node");
            waypointInd = UnityEngine.Random.Range(0, waypoints.Length); //Might need System for Random

            playerCollider = player.GetComponent<Collider>();

            state = enemyScript.State.PATROL;

            alive = true;

            //heightMultiplier = 1.4f;
        }

        private void Update()
        {
            StartCoroutine(FSM());
            Debug.Log(state);

            //playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
            playerLocation = player.transform.position - this.transform.position;
            
            //Debug.Log(playerLocation);

            Debug.DrawRay(myCamera.transform.position, playerLocation, Color.red);
            Debug.DrawRay(myCamera.transform.position, transform.forward * 10, Color.green);

            Debug.Log(player.transform.position);


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
            if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 0.5)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
                character.Move(agent.desiredVelocity, false, false);
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 0.5)
            {
                waypointInd = UnityEngine.Random.Range(0, waypoints.Length); //Might need System for Random
                //Need to fins a way to get the next gameobject.
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

            //waypointInd = UnityEngine.Random.Range(0, waypoints.Length);
            waypointInd = -100;

            agent.SetDestination(investigateSpot);
            character.Move(Vector3.zero, false, false);
            transform.LookAt(investigateSpot);
            if (timer >= investigateWait)
            {
                character.Move(this.transform.position, false, false);
                waypointInd = UnityEngine.Random.Range(0, waypoints.Length);
                state = enemyScript.State.PATROL;
                timer = 0;
            }
            else
            {

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player"/* && state == State.PATROL*/)
            {
                Debug.Log("Collider Enter");
                investigateSpot = other.gameObject.transform.position;
                state = enemyScript.State.INVESTIGATE;
            }
        }

        #region Camera 
        void CheckForPlayer()
        {
            RaycastHit frustumHit;
            RaycastHit playerHit;

            #region Commented Code
            //if (Physics.Raycast(myCamera.transform.position, transform.forward, out hit, 3) && (state != State.CHASE))
            //{
            //    target = player;
            //    state = enemyScript.State.CHASE;
            //}

            //Need to make it so when Player enters camera sightline he is set to seen
            //if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds))
            //{
            //    Debug.Log("CheckForPlayer Setting to chase");
            //    //state = enemyScript.State.CHASE;
            //}
            #endregion

            if (Physics.Raycast(myCamera.transform.position, playerLocation, out playerHit) && (state != State.CHASE))
            {
                target = player;
                Debug.Log("playerLocation Has been activated");
                //state = enemyScript.State.CHASE;
                if (Physics.Raycast(myCamera.transform.position, transform.forward, out frustumHit, 10) && (state != State.CHASE))
                {
                    target = player;
                    Debug.Log("Transform.Foward");
                    //state = enemyScript.State.CHASE;
                }
            }
            //if (Physics.Raycast(myCamera.transform.position, transform.forward, out frustumHit, 10) && (state != State.CHASE))
            //{
            //    target = player;
            //    Debug.Log("Transform.Foward");
            //    //state = enemyScript.State.CHASE;
            //}


        }
        #endregion
    }
}

/*
     Raycast form player to enemy as well as form enemy to end of frustum
     check if player Ray is shorter than Frustum Ray
*/
