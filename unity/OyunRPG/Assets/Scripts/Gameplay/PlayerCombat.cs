using OyunRPG.Systems;
using UnityEngine;

namespace OyunRPG.Gameplay
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float attackCooldown = 0.6f;
        [SerializeField] private float staminaCost = 20f;
        [SerializeField] private float attackRange = 2.5f;
        [SerializeField] private float attackAngle = 120f;
        [SerializeField] private float attackDamage = 35f;
        [SerializeField] private LayerMask enemyMask;

        private float cooldownTimer;
        private PlayerStats stats;

        public bool IsAttacking => cooldownTimer > 0f;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();
        }

        private void Update()
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0))
            {
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (cooldownTimer > 0f)
            {
                return;
            }

            if (!stats.TryConsumeStamina(staminaCost))
            {
                return;
            }

            cooldownTimer = attackCooldown;
            Vector3 origin = transform.position + Vector3.up;
            Collider[] hits = Physics.OverlapSphere(origin, attackRange, enemyMask);
            for (int i = 0; i < hits.Length; i++)
            {
                Vector3 toTarget = (hits[i].transform.position - transform.position);
                toTarget.y = 0f;
                float angle = Vector3.Angle(transform.forward, toTarget);
                if (angle > attackAngle * 0.5f)
                {
                    continue;
                }

                if (hits[i].TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.ApplyDamage(attackDamage);
                }
            }
        }
    }
}
