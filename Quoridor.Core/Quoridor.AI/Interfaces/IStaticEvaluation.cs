using System;
namespace Quoridor.AI.Interfaces
{
    public interface IStaticEvaluation<TAgent>
        where TAgent : class, IEquatable<TAgent>
    {
        public double Evaluate(TAgent agent);
    }
}
