using CommonComponents.Interfaces;
using UnityEngine;

namespace CommonComponents
{
	public delegate void DamageTaken(float amount);
	public class Damagable : MonoBehaviour
	{

		
		public delegate void HPChanged(float changeBy, float newHP) ;
		public delegate void Death(Damagable obj);
		private bool isDead = false;
		protected virtual void Awake()
		{
			DamageTaken += OnDamageTaken;
		}

		protected void OnDamageTaken(float amount)
		{
			CurrentHP -= amount;
			HPChangedEvent?.Invoke(amount, CurrentHP);
			if (CurrentHP < 0.0f && !isDead)
			{
				HPEmpty?.Invoke(this);
				isDead = true;
			}
		}

		public event DamageTaken DamageTaken;

		protected virtual void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.TryGetComponent<IDamageDealer>(out var damage))
			{
				DamageTaken?.Invoke(damage.Damage);
			}
		}
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDamageDealer>(out var damage)&& other.gameObject.CompareTag("Traps"))
            {
				DamageTaken?.Invoke(damage.Damage);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
			if (collision.gameObject.TryGetComponent<IDamageDealer>(out var damage))
			{
				DamageTaken?.Invoke(damage.Damage);
			}
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.TryGetComponent<IDamageDealer>(out var damage))
			{
				DamageTaken?.Invoke(damage.Damage);
			}
		}

		[field: SerializeField] public float MaxHP { get; set; }
		[field: SerializeField]public float CurrentHP { get; set; }
		public event HPChanged HPChangedEvent;
		public event Death HPEmpty;
	}
}