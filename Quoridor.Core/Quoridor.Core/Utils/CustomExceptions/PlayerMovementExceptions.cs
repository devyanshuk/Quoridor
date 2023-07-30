using System;

namespace Quoridor.Core.Utils.CustomExceptions
{
    public class InvalidAgentMoveException : Exception
    {
        public InvalidAgentMoveException(string message) : base(message) { }
        public InvalidAgentMoveException() : base() { }
    }

    public class NewMoveBlockedByWallException : Exception
    {
        public NewMoveBlockedByWallException(string message) : base(message) { }
        public NewMoveBlockedByWallException() : base() { }
    }
}
