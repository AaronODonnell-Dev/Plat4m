using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class cameraScript : MonoBehaviour
    {
        public NavMeshAgent agent;
        public ThirdPersonCharacter character;

        public enum State
        {
            PATROL,
            CHASE
        }

        public State state;
        private bool alive;
        private bool playerInSight;
        public float fieldOfView = 110f;
        private SphereCollider sphereCollider;

        //Patrol
        public GameObject[] waypoints;
        private int waypointInd;
        public float patrolSpeed = 1.5f;

        //Chase
        public float chaseSpeed = 3f;
        public GameObject target;

        //Sight
        public GameObject player;
        public Collider playerCollider;
        public Camera myCamera;
        private Plane[] planes;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            waypoints = GameObject.FindGameObjectsWithTag("Node");
            waypointInd = UnityEngine.Random.Range(0, waypoints.Length);

            playerCollider = player.GetComponent<Collider>();

            state = cameraScript.State.PATROL;

            alive = true;
        }

        private void Update()
        {
            StartCoroutine(FSM());
            planes = GeometryUtility.CalculateFrustumPlanes(myCamera);
            if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds))
            {
                Debug.Log("Player Spotted");
                CheckForPlayer();
            }
            else
            {

            }
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
                waypointInd = UnityEngine.Random.Range(0, waypoints.Length);
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

        void CheckForPlayer()
        {
            RaycastHit hit;
            Debug.DrawRay(myCamera.transform.position, transform.forward * 10, Color.green);
            if (Physics.Raycast(myCamera.transform.position, transform.forward, out hit, 100))
            {
                state = cameraScript.State.CHASE;
                target = hit.collider.gameObject;
            }
        }

        #region To Do with Unity Stealth Level
        //private void OnTriggerStay(Collider other)
        //{
        //    if (other.gameObject == player)
        //    {
        //        playerInSight = false;

        //        Vector3 direction = other.transform.position - transform.position;
        //        float angle = Vector3.Angle(direction, transform.forward);

        //        if (angle < fieldOfView * 0.5f)
        //        {
        //            RaycastHit hit;

        //            if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, sphereCollider.radius)
        //            {
        //                if (hit.collider.gameObject == player)
        //                {
        //                    playerInSight = true;
        //                    lastPlayerSighting
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}