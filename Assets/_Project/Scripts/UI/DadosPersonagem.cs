using UnityEngine;

[System.Serializable]
public class DadosPersonagem
{
    public string nome;                  // NALDINHO
    public string titulo;                // O AGENTE DE CAMPO
    [TextArea] public string historia;   // Morador da comunidade...
    [TextArea] public string motivacao;  // Motivo para estar na Bright Squad
    [TextArea] public string habilidades;// Habilidades principais
    public GameObject prefab3D;          // Prefab do personagem
}
