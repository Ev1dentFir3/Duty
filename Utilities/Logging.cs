using System;
using Rocket.Core.Logging;

namespace EFG.Duty.Utilities;

/// <summary>
/// Custom logging class.
/// </summary>
public static class Logging
{
    /// <summary>
    /// Writes a message to console.
    /// </summary>
    /// <param name="source">The source of the message.</param>
    /// <param name="message">the message to write to console.</param>
    /// <param name="consoleColor">The color to use on console.</param>
    /// <param name="logInRocket">If the message should be logged in rocket.</param>
    /// <param name="rocketMessage">The message to log in rocket. If null, it uses the same message as console.</param>
    /// <param name="rocketColor">The color to use when logging in rocket. If null, it uses the same color as console.</param>
    public static void Write(object source, object message, ConsoleColor consoleColor = ConsoleColor.Green,
        bool logInRocket = true, object? rocketMessage = null, ConsoleColor? rocketColor = null)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine($"[{source}]: {message}");

        if (logInRocket)
            Logger.ExternalLog(rocketMessage ?? message, rocketColor ?? consoleColor);

        Console.ResetColor();
    }
}