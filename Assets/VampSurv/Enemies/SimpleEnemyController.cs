using System;
using System.Collections;
using Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class SimpleEnemyController : MonoBehaviour, IHealthAndDamage
    {
        public delegate void EnemyDeath();

        public static event EnemyDeath OnEnemyDeath;
        
        //Animation parameters
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int Damaged = Animator.StringToHash("Damaged");
        private static readonly int Reset = Animator.StringToHash("Reset");
        private static readonly int VariSpeed = Animator.StringToHash("VariSpeed");

        //Editor editable
        [SerializeField] private Animator animator;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private LayerMask traceMask;
        [SerializeField] private LayerMask deadMask;
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float damagedMoveSpeed = 25f;
        [SerializeField] private float damageToDeal = 1f;
        [SerializeField] private DamageIndicator damageIndicator;
        [SerializeField] private GameObject xpDrop; //Have to pool this

        //Privates
        private SkinnedMeshRenderer m_renderer;
        private RigidbodyConstraints m_constraints;
        private Transform m_player;
        private Collider m_collider;
        private Quaternion m_targetRotation;
        private Vector3 m_damagedMoveToPosition;
        private Vector3 m_targetPosition;
        private float m_maxDamageMovementIntensity = 10f;
        private float m_minDamageMovementIntensity = 4f;
        private float m_attackThreshold = 3f;
        private bool m_canMove;
        private float m_health;
        private bool m_doDamagedMove;
        private bool m_isDead;
        private float startY = -1.6f;
        private bool m_inAttackRange;
        private Rigidbody m_rigidbody;
        private SimpleEnemyPool m_pool;
        private bool m_startHappaned;
        private float m_maxDistance = 185f;
        private float m_distanceCheckTimer = 4f;

        public void OnSpawn(SimpleEnemyPool pool, Vector3 spawnPosition)
        {
            m_player = FindAnyObjectByType<PlayerController>().transform;
            m_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            m_collider = GetComponent<Collider>();
            m_rigidbody = GetComponent<Rigidbody>();
            m_constraints = m_rigidbody.constraints;
            animator.SetFloat(VariSpeed, Random.Range(0.8f, 1.2f)); //small variations in movement anim speed to get less synced movements
            m_pool = pool;
            transform.position = spawnPosition;
        }

        public void OnRespawn(Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            ResetEnemy();
        }

        private void ResetEnemy()
        {
            m_doDamagedMove = false;
            m_targetPosition = transform.position;
            m_health = maxHealth;
            m_rigidbody.linearVelocity = Vector3.zero;
            animator.SetTrigger(Reset);
            animator.SetBool(IsDead, false);
            m_collider.enabled = true;
            m_renderer.enabled = true;
            m_isDead = false;
            m_renderer.material.color = Color.white;
            gameObject.SetActive(true);
            StartCheckDistance();
        }

        void FixedUpdate()
        {
            if (m_isDead)
                return;
            
            m_targetPosition = transform.position;

            if (m_doDamagedMove)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None;
                
                m_targetPosition = Vector3.MoveTowards(transform.position, m_damagedMoveToPosition,
                    damagedMoveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, m_damagedMoveToPosition) < 1f)
                {
                    m_doDamagedMove = false;
                    m_renderer.material.color = Color.white;
                    m_rigidbody.constraints = m_constraints;
                    if (transform.position.y > startY)
                    {
                        Vector3 resetHeight = new Vector3(transform.position.x, startY, transform.position.z);
                        transform.position = resetHeight;
                    }
                    if (!m_isDead && m_health <= 0)
                    {
                        OnDeath();
                    }
                }
            }

            float distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);

            if (distanceToPlayer >= m_attackThreshold)
            {
                animator.SetBool(Move, true);
                animator.SetBool(Attack, false);
                m_inAttackRange = false;
            }
            else if (distanceToPlayer <= m_attackThreshold)
            {
                animator.SetBool(Move, false);
                animator.SetBool(Attack, true);
                EndMove();
                m_inAttackRange = true;
            }

            if (m_canMove && !m_doDamagedMove)
            {
                m_targetPosition = Vector3.MoveTowards(transform.position, 
                    m_player.transform.position,movementSpeed * Time.deltaTime);
            }
            
            RotateTowardPlayer();
            
            if (m_canMove || m_doDamagedMove)
            {
                m_rigidbody.MovePosition(m_targetPosition);
            }
            m_rigidbody.MoveRotation(m_targetRotation.normalized);
        }

        private void RotateTowardPlayer()
        {
            if (m_isDead)
                return;
            
            m_targetRotation = Quaternion.LookRotation(m_player.transform.position - 
                                                       transform.position, Vector3.up);
        }

        private void LateUpdate()
        {
            if (m_isDead)
                return;
            
            m_rigidbody.linearVelocity = Vector3.zero;
        }


        //Called from move animation events
        public void StartMove()
        {
            m_canMove = true;
        }

        public void EndMove() //Don't listen to rider, this has to be public
        {
            m_canMove = false;
        }

        public void DoAttack()
        {
            if (m_inAttackRange)
            {
                m_player.GetComponent<IHealthAndDamage>().TakeDamage(damageToDeal, transform.position);
            }
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            damageIndicator.PlayDamageEffect();
            m_health -= damage;
            m_damagedMoveToPosition = hitDirection * Random.Range(m_minDamageMovementIntensity, 
                m_maxDamageMovementIntensity) + transform.position;
            m_renderer.material.color = Color.red;
            m_doDamagedMove = true;
            animator.SetTrigger(Damaged);
        }

        private void OnDeath()
        {
            OnEnemyDeath?.Invoke();
            animator.SetBool(IsDead, true);
            m_isDead = true;
            StartCoroutine(BlinkMesh(false, 2f, 0.12f));
        }

        public void Heal(float healAmount)
        {
            //Does not heal
        }
        
        IEnumerator BlinkMesh(bool endValue, float duration, float blinkTime)
        {
            bool isEnabled = false;
            for (float t = 0f; t < duration; t += blinkTime)
            {
                m_renderer.enabled = isEnabled;
                yield return new WaitForSeconds(blinkTime);
                isEnabled = !isEnabled;
            }
            m_renderer.enabled = endValue; //Set end value
            
            Vector3 resetHeight = new Vector3(transform.position.x, startY, transform.position.z);
            transform.position = resetHeight;
            if (Random.Range(0f, 1f) < 0.33f) //Randomly spawn XP drop
            {
                Instantiate(xpDrop, transform.position, Quaternion.identity);
            }
            
            StartCoroutine(WaitThenSendToPool(1f));
        }

        IEnumerator WaitThenSendToPool(float waitTime)
        {
            StopCoroutine(CheckDistanceToPlayer());
            yield return new WaitForSeconds(waitTime);
            m_rigidbody.linearVelocity = Vector3.zero;
            m_pool.ReturnToPool(this);
            gameObject.SetActive(false);
        }

        private void StartCheckDistance()
        {
            StartCoroutine(CheckDistanceToPlayer());
        }
        
        IEnumerator CheckDistanceToPlayer()
        {
            yield return new WaitForSeconds(m_distanceCheckTimer);
            
            float distance = Vector3.Distance(m_player.position, transform.position);
            if (distance > m_maxDistance)
            {
                WaitThenSendToPool(0.1f);
            }
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}