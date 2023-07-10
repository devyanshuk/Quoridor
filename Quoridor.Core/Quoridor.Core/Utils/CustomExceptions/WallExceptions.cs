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
}
