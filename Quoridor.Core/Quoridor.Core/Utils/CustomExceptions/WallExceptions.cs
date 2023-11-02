using System;

namespace Quoridor.Core.Utils.CustomExceptions
{
    public class WallAlreadyPresentException : Exception
    {
        public WallAlreadyPresentException(string message) : base(message) {}

        public WallAlreadyPresentException() : base() { }
    }

    public class InvalidWallException : Exception
    {
        public InvalidWallException(string message) : base(message) { }

        public InvalidWallException() : base() { }
    }

    public class WallIntersectsException : Exception
    {
        public WallIntersectsException(string message) : base(message) { }

        public WallIntersectsException() : base() { }
    }

    public class WallNotPresentException : Exception
    {
        public WallNotPresentException(string message) : base(message) { }

        public WallNotPresentException() : base() { }
    }

    public class NewWallBlocksPlayerException : Exception
    {
        public NewWallBlocksPlayerException(string message) : base(message) { }

        public NewWallBlocksPlayerException() : base() { }
    }

    public class NoWallRemainingException : Exception
    {
        public NoWallRemainingException(string message) : base(message) { }
        public NoWallRemainingException() : base() { }
    }
}
