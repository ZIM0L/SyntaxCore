using Microsoft.AspNetCore.SignalR;

namespace SyntaxCore.Entities;

public class User   
{

    // 🔑 Dane profilowe
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public string Email { get; set; } = string.Empty;

    // 🎮 Dane gry / postaci
    public int Level { get; set; } = 1;            // Poziom
    public int Experience { get; set; } = 0;       // Punkty doświadczenia
    public int Wins { get; set; } = 0;             // Liczba wygranych bitew
    public int Losses { get; set; } = 0;           // Liczba przegranych bitew

    // 🌐 Preferencje / ustawienia
    public bool IsPublicProfile { get; set; } = true;

}
