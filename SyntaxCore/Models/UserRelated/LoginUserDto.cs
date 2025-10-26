namespace SyntaxCore.Models.UserRelated
{
    public record LoginUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
