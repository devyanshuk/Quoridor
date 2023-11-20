using System;

namespace Quoridor.Core.Utils.CustomExceptions
{
    public class MoveException : Exception
    {
        public MoveException(string message) : base(message) { }
        public MoveException() : base() { }
    }
    public class InvalidAgentMoveException : MoveException
    {
        public InvalidAgentMoveException(string message) : base(message) { }
        public InvalidAgentMoveException() : base() { }
    }

    public class NewMoveBlockedByWallException : MoveException
    {
        public NewMoveBlockedByWallException(string message) : base(message) { }
        public NewMoveBlockedByWallException() : base() { }
    }
}
