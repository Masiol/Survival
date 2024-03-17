using UnityEngine;

public class RotateUIWorldSpaceToPlayer: MonoBehaviour
{
    private Transform player;
    private Quaternion startRotation;
    private float smoothness = 5f; 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < 10f)
            {
                Vector3 direction = player.position - transform.position;
                direction.y = 0f;

                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime * smoothness);
            }
        }
    }
}
