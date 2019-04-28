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

        public GameObject[] waypoints;
        private int waypointInd;
        public float patrolSpeed = 1.5f;

        public float chaseSpeed = 3f;
        public GameObject target;

        private Vector3 investigateSpot;
        private float timer = 0;
        public float investigateWait = 10;

        public float heightMultiplier;
        public float sightDistance = 10; 

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            waypoints = GameObject.FindGameObjectsWithTag("Node");
            waypointInd = UnityEngine.Random.Range(0, waypoints.Length); //Might need System for Random

            state = enemyScript.State.PATROL;

            alive = true;

            heightMultiplier = 1.4f;
        }

        private void Update()
        {
            StartCoroutine(FSM());
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
            RaycastHit hit;
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDistance, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized * sightDistance, Color.green);
            Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized * sightDistance, Color.green);
            if (Physics.Raycast (transform.position + Vector3.up * heightMultiplier, transform.forward, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = enemyScript.State.CHASE;
                    target = hit.collider.gameObject;
                }
            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = enemyScript.State.CHASE;
                    target = hit.collider.gameObject;
                }
            }
            if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    state = enemyScript.State.CHASE;
                    target = hit.collider.gameObject;
                }
            }
            agent.SetDestination(this.transform.position);
            character.Move(Vector3.zero, false, false);
            transform.LookAt(investigateSpot);
            if (timer >= investigateWait)
            {
                state = enemyScript.State.PATROL;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                //state = enemyScript.State.CHASE;
                //target = other.gameObject;
                //st
            }
        }
    }
}