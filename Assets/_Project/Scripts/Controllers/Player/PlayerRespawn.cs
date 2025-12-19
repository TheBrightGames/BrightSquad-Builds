using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Transform spawnPoint;

    void Start()
    {
        var sp = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (sp != null)
            spawnPoint = sp.transform;
    }

    void Update()
    {
        if (spawnPoint == null) return;

        if (transform.position.y < -10f)
        {
            transform.position = spawnPoint.position;
            var rb = GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = Vector3.zero; // versão nova
        }
    }
}
