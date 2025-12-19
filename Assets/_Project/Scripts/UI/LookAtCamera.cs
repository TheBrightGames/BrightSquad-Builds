using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam == null) return;

        // Sempre olha para a câmera, mas mantém “em pé”
        Vector3 dir = mainCam.transform.position - transform.position;
        dir.y = 0f; // trava rotação em Y para não deitar
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(-dir);
    }
}
