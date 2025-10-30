namespace SyntaxCore.Models.BattleRelated
{
    public class BattleParticipantsDto
    {
        public string CurrentJoinedPlayerUserName { get; set; } = string.Empty;
        public List<string> PlayersUserNames { get; set; } = new List<string>();
        public int MaxParticipants { get; set; }
    }
}