namespace SyntaxCore.Constants
{
    public static class BattleStatuses 
    {
        public static string Waiting => "waiting";
        public static string InProgress => "in_progress";
        public static string Finished => "finished";
        public static string Cancelled => "cancelled";

        // Implementacja interfejsu (domyślnie np. Waiting)
        public static string Status => Waiting;
    }
}
