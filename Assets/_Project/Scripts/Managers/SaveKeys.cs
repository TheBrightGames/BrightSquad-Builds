using UnityEngine;
//desbloqueia por missão
/* public static class SaveKeys
{
    // Tempo e score por PLAYER e MISSÃO
    public static string TimeKey(int playerId, int missionId)
        => $"P{playerId}_M{missionId}_BestTime";

    public static string ScoreKey(int playerId, int missionId)
        => $"P{playerId}_M{missionId}_BestScore";

    // Desbloqueios VISUAIS por missão (não por player)
    public static string UnlockKeyGlobal(int missionId, int unlockIndex)
        => $"M{missionId}_Unlock_{unlockIndex}";
} */



//desbloqueia global


public static class SaveKeys
{
    // Tempo e score por PLAYER e MISSÃO
    public static string TimeKey(int playerId, int missionId)
        => $"P{playerId}_M{missionId}_BestTime";

    public static string ScoreKey(int playerId, int missionId)
        => $"P{playerId}_M{missionId}_BestScore";

    // Desbloqueios globais por objeto (School, Hospital, etc.)
    public static string UnlockKey(string id) => $"Unlock_{id}";
}


