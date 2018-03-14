using System;

namespace Bot
{
    public interface IHelpCommandTextBuilder
    {
        string BuildHelpCommandText();
        string BuildErrorMessage(Exception exception);
    }
}