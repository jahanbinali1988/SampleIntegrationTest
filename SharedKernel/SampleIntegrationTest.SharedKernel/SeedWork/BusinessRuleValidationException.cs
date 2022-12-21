using System;

namespace SampleIntegrationTest.SharedKernel.SeedWork
{
    public class BusinessRuleValidationException : Exception
    {
        public IBusinessRule BrokenRule { get; }

        public string Details { get; }

        public string[] Properties { get; set; }

        public string ErrorType { get; set; }

        public BusinessRuleValidationException(IBusinessRule brokenRule, string[] properties, string errorType) : base(brokenRule.Message)
        {
            BrokenRule = brokenRule;
            Details = brokenRule.Message;
            Properties = properties;
            ErrorType = errorType;
        }

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
        }
    }
}
