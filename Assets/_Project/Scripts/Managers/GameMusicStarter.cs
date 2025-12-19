using UnityEngine;

public class GameMusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip musicaJogo;

    void Start()
    {
        if (SoundController.Instance != null)
        {
            SoundController.Instance.TrocarMusica(musicaJogo);
        }
    }
}
