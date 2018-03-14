using System;

namespace Bot
{
    public interface IHelpCommandTextBuilder
    {
        string BuildHelpCommandText();
        string BuildServiceUnawailableMessage(Exception exception);
    }
}