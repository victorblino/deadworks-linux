namespace DeadworksManaged.Api;

/// <summary>
/// Context passed to [ConCommand] and [ConVar] handlers.
/// </summary>
public sealed unsafe class ConCommandContext
{
    /// <summary>Player slot of the caller, or -1 if invoked from server console.</summary>
    public int CallerSlot { get; }

    /// <summary>The command name that was typed (args[0]).</summary>
    public string Command { get; }

    /// <summary>All arguments including the command name at index 0, as tokenized by the engine. A double-quoted run is a single argument with the quotes stripped.</summary>
    public string[] Args { get; }

    /// <summary>The arguments after the command name, joined with single spaces. Quoting is not preserved, so use <see cref="Args"/> to tell <c>"a b"</c> from <c>a b</c>. Empty if no args.</summary>
    public string ArgString => Args.Length > 1 ? string.Join(" ", Args, 1, Args.Length - 1) : "";

    /// <summary>True when invoked from server console (no player).</summary>
    public bool IsServerCommand => CallerSlot < 0;

    /// <summary>The player controller, or null if invoked from server console.</summary>
    public CCitadelPlayerController? Controller
    {
        get
        {
            if (CallerSlot < 0 || NativeInterop.GetPlayerController == null)
                return null;
            var ptr = NativeInterop.GetPlayerController(CallerSlot);
            return ptr != null ? new CCitadelPlayerController((nint)ptr) : null;
        }
    }

    internal ConCommandContext(int callerSlot, string command, string[] args)
    {
        CallerSlot = callerSlot;
        Command = command;
        Args = args;
    }
}
