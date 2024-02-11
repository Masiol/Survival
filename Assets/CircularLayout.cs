using UnityEngine;
using UnityEngine.UI;

public class CircularLayout : MonoBehaviour
{
    public int numberOfElements = 8;
    public float radius = 100f;
    public GameObject prefab;

    void Start()
    {
        ArrangeElements();
    }

    void ArrangeElements()
    {
        Vector3 center = transform.position; // Centrum okrêgu w przestrzeni 3D

        for (int i = 0; i < numberOfElements; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfElements;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            position += center;

            // Zak³adamy, ¿e masz prefabrykat elementu UI, który chcesz u¿yæ
            // Jeœli nie, musisz utworzyæ element UI w kodzie jak w poprzednim przyk³adzie
            GameObject element = Instantiate(prefab, position, Quaternion.identity); // Zast¹p twójPrefabrykat rzeczywistym prefabrykatem
            element.transform.SetParent(transform, false);
            element.transform.localPosition = position;
            element.transform.LookAt(center);
            element.transform.rotation = Quaternion.Euler(90, 0, 0);

        }
    }
}
