using System;
using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class SimpleEnemyController : MonoBehaviour, IHealthAndDamage
    {
        //Animation parameters
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int isDead = Animator.StringToHash("isDead");
        private static readonly int Damaged = Animator.StringToHash("Damaged");
        private static readonly int Reset = Animator.StringToHash("Reset");

        //Editor editable
        [SerializeField] private Animator animator;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private bool rangedAttack;
        [SerializeField] private float damageMovementIntensity = 2f;
        [SerializeField] private float damagedMoveSpeed = 25f;
        [SerializeField] private float damageToDeal = 1f;
        [SerializeField] private DamageIndicator damageIndicator;
        [SerializeField] private GameObject xpDrop; //Have to pool this

        //Privates
        private float m_attackThreshold = 2f;
        private bool m_canMove;
        private Transform m_player;
        private float m_health;
        private bool m_doDamagedMove;
        private bool m_isDead;
        private Vector3 m_damagedMoveToPosition;
        private SkinnedMeshRenderer m_renderer;
        private float m_gravityIntensity = -8f;
        private float m_fallSpeed = 50f;
        private Vector3 m_targetLocation;
        private Collider m_collider;
        private bool m_inAttackRange;
        
        private Rigidbody m_rigidbody; //testing

        void Start()
        {
            m_player = FindAnyObjectByType<PlayerMovement>().transform;
            m_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            animator.SetTrigger(Reset);
            m_collider = GetComponent<Collider>();
            ResetEnemy();
        }

        private void ResetEnemy()
        {
            animator.SetBool(isDead, false);
            m_health = maxHealth;
            m_isDead = false;
            m_collider.enabled = true;
        }

        void Update()
        {
            m_targetLocation = transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 2f,
                    transform.position + transform.forward + Vector3.down * 500, out hit,
                    1000f, groundLayer))
            {
                if (hit.point.y < transform.position.y)
                {
                    m_targetLocation.y += m_gravityIntensity * Time.deltaTime;
                    Vector3.MoveTowards(transform.position, m_player.transform.position, m_fallSpeed * Time.deltaTime);
                }
            }

            transform.position = m_targetLocation;

            if (m_doDamagedMove)
            {
                m_targetLocation = Vector3.MoveTowards(transform.position, m_damagedMoveToPosition,
                    damagedMoveSpeed * Time.deltaTime);
                transform.position = m_targetLocation;
                if (Vector3.Distance(transform.position, m_damagedMoveToPosition) < 0.1f)
                {
                    m_doDamagedMove = false;
                    m_renderer.material.color = Color.white;
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
                m_targetLocation = Vector3.MoveTowards(transform.position, m_player.transform.position,
                    movementSpeed * Time.deltaTime);
            }

            transform.position = m_targetLocation;

            RotateTowardPlayer();
        }

        private void RotateTowardPlayer()
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(m_player.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //Called from move animation events
        public void StartMove()
        {
            m_canMove = true;
        }

        public void EndMove()
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
            m_damagedMoveToPosition = hitDirection * damageMovementIntensity + transform.position;
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
            animator.SetBool(isDead, true);
            m_collider.enabled = false;
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
            if (Random.Range(0f, 1f) < 0.14f) //Randomly spawn XP drop
            {
                Instantiate(xpDrop, transform.position, Quaternion.identity);
            }
            yield return null;
        }
    }
}