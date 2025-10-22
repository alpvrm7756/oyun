using OyunRPG.Systems;
using UnityEngine;
using UnityEngine.Events;

namespace OyunRPG.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float attackRange = 1.75f;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float attackDamage = 12f;
        [SerializeField] private float maxHealth = 60f;
        [SerializeField] private UnityEvent onDeath;

        private CharacterController characterController;
        private Transform target;
        private float cooldownTimer;
        private float currentHealth;

        public UnityEvent<float, float> OnHealthChanged = new UnityEvent<float, float>();

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            currentHealth = maxHealth;
            OnHealthChanged.Invoke(currentHealth, maxHealth);
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            Vector3 toTarget = target.position - transform.position;
            Vector3 planar = new Vector3(toTarget.x, 0f, toTarget.z);
            float distance = planar.magnitude;

            if (distance > attackRange)
            {
                Vector3 move = planar.normalized * moveSpeed * Time.deltaTime;
                characterController.Move(move);
                if (move != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(move, Vector3.up);
                }
            }
            else
            {
                if (cooldownTimer <= 0f)
                {
                    AttemptAttack();
                }
            }

            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }
        }

        private void AttemptAttack()
        {
            if (!target.TryGetComponent<PlayerStats>(out var stats))
            {
                return;
            }

            stats.ApplyDamage(attackDamage);
            cooldownTimer = attackCooldown;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void ApplyDamage(float amount)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            OnHealthChanged.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0f)
            {
                onDeath?.Invoke();
                enabled = false;
            }
        }
    }
}
