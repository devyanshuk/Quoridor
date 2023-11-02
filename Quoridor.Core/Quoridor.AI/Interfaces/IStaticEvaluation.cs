using System;
namespace Quoridor.AI.Interfaces
{
    public interface IStaticEvaluation<TAgent>
    {
        public double Evaluate(TAgent agent);
    }
}
