using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LixoSomDirecional : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 30f;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        float vol = Mathf.Clamp01(1f - dist / maxDistance);
        audioSource.volume = vol;
    }
}
