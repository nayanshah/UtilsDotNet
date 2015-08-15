using System;

namespace Utils.CommandLine
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public sealed class DetailsAttribute : Attribute
    {
        public string Details { get; private set; }

        public DetailsAttribute(string details)
        {
            Details = details;
        }
    }
}