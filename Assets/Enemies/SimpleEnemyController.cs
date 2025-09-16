using System;
using Player;
using UnityEngine;

namespace Enemies
{
    public class SimpleEnemyController : MonoBehaviour, IHealthAndDamage
    {
        //Animation parameters
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Died = Animator.StringToHash("Died");

        //Editor editable
        [SerializeField] private Animator animator;
        [SerializeField] private float attackThreshold = 15f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float maxDistanceToPlayer = 10f;
        [SerializeField] private float maxHealth = 100f;
        
        //Privates
        private bool m_canMove;
        private PlayerMovement m_player;
        private float m_health;
        private float m_damagedMoveSpeed = 10f;
        private bool m_doDamagedMove;
        private Vector3 m_damagedMoveToPosition;
        private SkinnedMeshRenderer m_renderer;
        
        void Start()
        {
            m_player = FindAnyObjectByType<PlayerMovement>();
            m_health = maxHealth;
            m_renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        void Update()
        {
            if (m_health < 0f)
                return;
            
            float distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position) + maxDistanceToPlayer;
            
            if (distanceToPlayer >= attackThreshold)
            {
                animator.SetBool(Move, true);
                animator.SetBool(Attack, false);
            }
            else if (distanceToPlayer <= attackThreshold)
            {
                animator.SetBool(Move, false);
                animator.SetBool(Attack, true);
            }
            
            if (m_canMove && !m_doDamagedMove)
            {
                Vector3 targetLocation = Vector3.MoveTowards(transform.position, m_player.transform.position, movementSpeed * Time.deltaTime);
                targetLocation.y = Physics.Raycast(transform.position + Vector3.up * 2f, transform.position-Vector3.down*100, 1000f, groundLayer) ? transform.position.y : targetLocation.y;
                transform.position = targetLocation;
            }

            if (m_doDamagedMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_damagedMoveToPosition, m_damagedMoveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, m_damagedMoveToPosition) < 0.1f)
                {
                    m_doDamagedMove = false;
                    m_renderer.material.color = Color.white;
                }
            }
            
            RotateTowardPlayer();
        }

        private void RotateTowardPlayer()
        {
            Quaternion targetRotation = Quaternion.LookRotation(m_player.transform.position - transform.position, Vector3.up);
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
            //Rotate toward player and shoot projectile
            print("SLIME tries to attack");
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            m_health -= damage;
            m_damagedMoveToPosition = hitDirection; 
            m_doDamagedMove = true;
            m_renderer.material.color = Color.crimson;
            
            if (m_health <= 0)
            {
                animator.SetTrigger(Died);
                print("SLIME WAS and IS DEADER THAN DISCO!");
            }
        }

        public void Heal(float healAmount)
        {
            //Does not heal
        }
    }
}
