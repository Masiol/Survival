using UnityEngine;

public class AnimalAttack : MonoBehaviour
{
    [SerializeField] private float attackRadius = 2f; // Promieñ obszaru ataku
    [SerializeField] private float attackDamage = 10f; // Iloœæ odejmowanego zdrowia

    private void Update()
    {
        // SprawdŸ, czy s¹ obiekty w zasiêgu ataku
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player")) // Jeœli trafiony obiekt to gracz
            {
                HealthManager.Instance.ChangeHealth(-attackDamage); // Odejmij zdrowie
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Wyœwietl zasiêg ataku w edytorze
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}