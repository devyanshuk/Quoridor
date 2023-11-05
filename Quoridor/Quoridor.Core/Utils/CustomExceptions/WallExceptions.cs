using System;

namespace Quoridor.Core.Utils.CustomExceptions
{
    public abstract class WallException : Exception
    {
        public WallException(string message) : base(message) { }
        public WallException() { }
    }

    public sealed class WallAlreadyPresentException : WallException
    {
        public WallAlreadyPresentException(string message) : base(message) {}

        public WallAlreadyPresentException() : base() { }
    }

    public class InvalidWallException : WallException
    {
        public InvalidWallException(string message) : base(message) { }

        public InvalidWallException() : base() { }
    }

    public class WallIntersectsException : WallException
    {
        public WallIntersectsException(string message) : base(message) { }

        public WallIntersectsException() : base() { }
    }

    public class WallNotPresentException : WallException
    {
        public WallNotPresentException(string message) : base(message) { }

        public WallNotPresentException() : base() { }
    }

    public class NewWallBlocksPlayerException : WallException
    {
        public NewWallBlocksPlayerException(string message) : base(message) { }

        public NewWallBlocksPlayerException() : base() { }
    }

    public class NoWallRemainingException : WallException
    {
        public NoWallRemainingException(string message) : base(message) { }
        public NoWallRemainingException() : base() { }
    }
}
