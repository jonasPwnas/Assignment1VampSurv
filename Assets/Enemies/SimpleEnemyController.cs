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
        //Animation parameters
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int Damaged = Animator.StringToHash("Damaged");
        private static readonly int Reset = Animator.StringToHash("Reset");
        private static readonly int VariSpeed = Animator.StringToHash("VariSpeed");

        //Editor editable
        [SerializeField] private Animator animator;
        [SerializeField] private float rotationSpeed = 10f;
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
        private float m_damageMovementIntensity = 20f;
        private float m_attackThreshold = 4.5f;
        private bool m_canMove;
        private float m_health;
        private bool m_doDamagedMove;
        private bool m_isDead;
        private float m_gravityIntensity = -8f;
        private float m_fallSpeed = 50f;
        private float startY;
        private bool m_inAttackRange;
        private Rigidbody m_rigidbody;

        void Start()
        {
            m_player = FindAnyObjectByType<PlayerMovement>().transform;
            m_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            animator.SetTrigger(Reset);
            m_collider = GetComponent<Collider>();
            m_rigidbody = GetComponent<Rigidbody>();
            m_constraints = m_rigidbody.constraints;
            startY = transform.position.y;
            animator.SetFloat(VariSpeed, Random.Range(0.8f, 1.2f)); //small variations in movement anim speed to get less synced movements
            ResetEnemy();
        }

        private void ResetEnemy()
        {
            animator.SetBool(IsDead, false);
            m_health = maxHealth;
            m_isDead = false;
            m_collider.enabled = true;
        }

        void FixedUpdate()
        {
            m_targetPosition = transform.position;
            
            //USING RIGIDBODY INSTEAD FOR NOW!!!
            //transform.position = m_targetLocation;

            if (m_doDamagedMove)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None;
                
                m_targetPosition = Vector3.MoveTowards(transform.position, m_damagedMoveToPosition,
                    damagedMoveSpeed * Time.deltaTime);
                //transform.position = m_targetPosition;
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
                }
            }

            if (m_isDead)
                return;

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
                m_targetPosition = Vector3.MoveTowards(transform.position, m_player.transform.position,movementSpeed * Time.deltaTime);
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
            m_targetRotation = Quaternion.LookRotation(m_player.transform.position - transform.position, Vector3.up);
        }

        private void LateUpdate()
        {
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
            m_damagedMoveToPosition = hitDirection * m_damageMovementIntensity + transform.position;
            m_renderer.material.color = Color.crimson;
            m_doDamagedMove = true;
            animator.SetTrigger(Damaged);

            if (!m_isDead && m_health <= 0)
            {
                m_damagedMoveToPosition = hitDirection + -transform.forward * 10f + transform.position + Vector3.up * 10f;
                OnDeath();
            }
        }

        private void OnDeath()
        {
            animator.SetBool(IsDead, true);
            //m_collider.includeLayers
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
            if (Random.Range(0f, 1f) < 0.33f) //Randomly spawn XP drop
            {
                Vector3 resetHeight = new Vector3(transform.position.x, startY, transform.position.z);
                transform.position = resetHeight;
                Instantiate(xpDrop, transform.position, Quaternion.identity);
            }
            yield return null;
        }
    }
}