using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspitionTime =3f;
        [SerializeField] float agroCoolDownTime =3f;
        [SerializeField] float moveToTheNextWayPointTime = 2f;

         [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float shoutDistance = 2f;

        Health health;
        Fighter fighter;
        GameObject player;

        Mover mover;

        LazyValue <Vector3> guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;

        float timeSinceLastWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;

        int currentWaypointIndex = 0;

        private void Awake() {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");   
            health = GetComponent<Health>(); 
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start() 
        {
            guardPosition.ForceInit();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspitionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
            timeSinceAggrevated +=Time.deltaTime;

        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;
            if (patrolPath != null)
            {
                
                
                if (AtWaypoint())
                {
                    timeSinceLastWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
                if(timeSinceLastWaypoint > moveToTheNextWayPointTime)
                {
                    mover.StartMoveAction(nextPosition, patrolSpeedFraction);
                }
                
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
           currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
           float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
           return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
           RaycastHit[] hits = Physics.SphereCastAll(transform.position,shoutDistance, Vector3.up, 0);
           foreach (RaycastHit hit in hits)
           {
               AIController ai = hit.collider.GetComponent<AIController>();
               if(ai == null) continue;
               ai.Aggrevate();
           }
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCoolDownTime;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);

        }

    }
} 
