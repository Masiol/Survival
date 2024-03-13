using UnityEngine;

public class AnimalAttack : MonoBehaviour
{
    [SerializeField] private float attackRadius = 2f; // Promie� obszaru ataku
    [SerializeField] private float attackDamage = 10f; // Ilo�� odejmowanego zdrowia

    private void Update()
    {
        // Sprawd�, czy s� obiekty w zasi�gu ataku
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player")) // Je�li trafiony obiekt to gracz
            {
                HealthManager.Instance.ChangeHealth(-attackDamage); // Odejmij zdrowie
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Wy�wietl zasi�g ataku w edytorze
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}