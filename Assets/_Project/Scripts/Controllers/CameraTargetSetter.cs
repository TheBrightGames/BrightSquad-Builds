using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetSetter : MonoBehaviour
{
    public CinemachineCamera cineCam;

    public void DefinirAlvo(Transform novoAlvo)
    {
        if (cineCam != null && novoAlvo != null)
        {
            // API do Cinemachine 3.x
            cineCam.Target.TrackingTarget = novoAlvo;
        }
    }
}
