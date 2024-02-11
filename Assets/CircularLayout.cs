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
        Vector3 center = transform.position; // Centrum okr�gu w przestrzeni 3D

        for (int i = 0; i < numberOfElements; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfElements;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            position += center;

            // Zak�adamy, �e masz prefabrykat elementu UI, kt�ry chcesz u�y�
            // Je�li nie, musisz utworzy� element UI w kodzie jak w poprzednim przyk�adzie
            GameObject element = Instantiate(prefab, position, Quaternion.identity); // Zast�p tw�jPrefabrykat rzeczywistym prefabrykatem
            element.transform.SetParent(transform, false);
            element.transform.localPosition = position;
            element.transform.LookAt(center);
            element.transform.rotation = Quaternion.Euler(90, 0, 0);

        }
    }
}
