using CLAP;
using System;

using Quoridor.AI;
using Quoridor.Common.Helpers;

namespace Quoridor.ConsoleApp.Utils
{
    public class StrategyValidationAttribute : ValidationAttribute
    {
        public override string Description => $"Strategy is one of {String.Join(", ", Enum.GetNames(typeof(AITypes)))}";

        public override IValueValidator GetValidator()
        {
            return new StrategyValidator();
        }
    }

    public class StrategyValidator : IValueValidator
    {
        public void Validate(ValueInfo info)
        {
            try
            {
                var strategyType = EnumHelper.ParseEnum<AITypes>((string)info.Value);
            }
            catch(Exception)
            {
                throw new ValidationException($"Strategy should be one of {String.Join(", ", Enum.GetNames(typeof(AITypes)))}");
            }

        }
    }
}
