using Microsoft.AspNetCore.SignalR;

namespace SyntaxCore.Infrastructure.ErrorExceptions
{
    /// <summary>
    /// Exception where send data was not found in database
    /// </summary>
    /// <param name="message"></param>
    public class NotFoundException(string message) : Exception(message);
    /// <summary>
    /// Exception where client has not access to certain data
    /// </summary>
    /// <param name="message"></param>
    public class ForbiddenException(string message) : Exception(message);

    /// <summary>
    /// specific exception for joining battle errors
    /// </summary>
    /// <param name="message"></param>
    public class JoinBattleException(string message) : Exception(message);
    /// <summary>
    /// custom exception for question creation errors
    /// </summary>
    /// <param name="message"></param>
    public class QuestionCreationException(string message) : Exception(message);
    /// <summary>
    /// custom exception for question not available errors
    /// </summary>
    /// <param name="message"></param>
    public class QuestionNotAvailableException(string message) : Exception(message);
}
