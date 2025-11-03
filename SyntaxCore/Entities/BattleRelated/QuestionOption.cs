using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.BattleRelated
{
    [Table("QuestionOptions")]
    [Comment("Stores answer options for questions used in battles.")]
    public class QuestionOption
    {
        [Key]
        public Guid QuestionOptionId { get; set; } = Guid.NewGuid();
        public Guid QuestionFK { get; set; }
        public string OptionAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        //navigation 
        [ForeignKey(nameof(QuestionFK))]
        public Question Question { get; set; } = null!;
    }
}
