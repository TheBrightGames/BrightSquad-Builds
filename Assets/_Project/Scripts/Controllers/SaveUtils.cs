using UnityEngine;

public static class SaveUtils
{
    public static void LimparQuickSave()
    {
        int playerId = PlayerPrefs.GetInt("PersonagemSelecionado", 0);

        // mesma função P() que você usa no GameManager
        string P(string key) => $"P{playerId}_{key}";

        PlayerPrefs.DeleteKey(P("HasQuickSave"));

        PlayerPrefs.DeleteKey(P("Last_Player_PosX"));
        PlayerPrefs.DeleteKey(P("Last_Player_PosY"));
        PlayerPrefs.DeleteKey(P("Last_Player_PosZ"));
        PlayerPrefs.DeleteKey(P("Last_Player_RotY"));

        PlayerPrefs.DeleteKey(P("Last_TempoDecorrido"));

        PlayerPrefs.DeleteKey(P("Last_LixoPlastico"));
        PlayerPrefs.DeleteKey(P("Last_LixoMetal"));
        PlayerPrefs.DeleteKey(P("Last_LixoPapel"));
        PlayerPrefs.DeleteKey(P("Last_LixoVidro"));

        // se não usar mais as versões antigas, pode também limpar
        PlayerPrefs.DeleteKey("HasQuickSave");
        PlayerPrefs.DeleteKey("Last_QuickSave_PlayerId");

        PlayerPrefs.Save();
    }
}
