using UnityEngine;

public class MenuAudioSettings : MonoBehaviour
{
    [Range(0f, 1f)]
    public float volumeMenu = 0.35f;

    void Start()
    {
        if (SoundController.Instance != null)
            SoundController.Instance.DefinirVolume(volumeMenu);
    }
}
