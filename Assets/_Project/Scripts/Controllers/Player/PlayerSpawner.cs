using UnityEngine;
using Unity.Cinemachine;   // Cinemachine 3.x

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] personagens;
    public Transform spawnPoint;

    // TROCA AQUI: em vez de CinemachineVirtualCameraBase
    public CinemachineCamera cineCam;

    const string CHAVE_PLAYER = "PersonagemSelecionado";

    void Start()
    {
        Time.timeScale = 1f;

        int indice = PlayerPrefs.GetInt(CHAVE_PLAYER, 0);
        if (personagens == null || personagens.Length == 0) return;
        indice = Mathf.Clamp(indice, 0, personagens.Length - 1);

        if (personagens[indice] != null && spawnPoint != null)
        {
            GameObject player = Instantiate(personagens[indice],
                                            spawnPoint.position,
                                            spawnPoint.rotation);

            player.tag = "Player";

            if (cineCam != null)
            {
                // API nova do Cinemachine 3.x
                cineCam.Target.TrackingTarget = player.transform;
            }
        }
    }
}
