using UnityEngine;
using UnityEngine.Events;

namespace OyunRPG.Gameplay
{
    [System.Serializable]
    public class StatChangedEvent : UnityEvent<float, float> { }

    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaRegenPerSecond = 15f;

        public StatChangedEvent OnHealthChanged = new StatChangedEvent();
        public StatChangedEvent OnStaminaChanged = new StatChangedEvent();
        public UnityEvent OnPlayerDied = new UnityEvent();

        private float currentHealth;
        private float currentStamina;

        private void Awake()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            Notify();
        }

        private void Update()
        {
            if (currentStamina < maxStamina)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenPerSecond * Time.deltaTime);
                OnStaminaChanged.Invoke(currentStamina, maxStamina);
            }
        }

        public bool TryConsumeStamina(float amount)
        {
            if (currentStamina < amount)
            {
                return false;
            }

            currentStamina -= amount;
            OnStaminaChanged.Invoke(currentStamina, maxStamina);
            return true;
        }

        public void ApplyDamage(float amount)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            OnHealthChanged.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0f)
            {
                OnPlayerDied.Invoke();
            }
        }

        public void RestoreHealth(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged.Invoke(currentHealth, maxHealth);
        }

        private void Notify()
        {
            OnHealthChanged.Invoke(currentHealth, maxHealth);
            OnStaminaChanged.Invoke(currentStamina, maxStamina);
        }
    }
}
