using UnityEngine;

[CreateAssetMenu(menuName = "Rewards/CommunityRewardDB")]
public class CommunityRewardDB : ScriptableObject
{
    [System.Serializable]
    public class RewardEntry
    {
        public string id;          // mesmo id usado em MissionManager.Desbloqueio
        public Sprite icon;
        public string titulo;
        [TextArea]
        public string descricao;
    }

    public RewardEntry[] rewards;

    public RewardEntry GetById(string id)
    {
        if (rewards == null) return null;
        foreach (var r in rewards)
            if (r != null && r.id == id)
                return r;
        return null;
    }
}
