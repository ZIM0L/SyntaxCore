namespace SyntaxCore.Models.BattleRelated
{
    public class BattleDto
    {
        public int BattleId { get; set; }          
        public Guid PlayerId1 { get; set; }        
        public Guid? PlayerId2 { get; set; }        
        public string Status { get; set; }        
        public DateTime CreatedAt { get; set; }    
        public DateTime? EndedAt { get; set; }      
    }

}