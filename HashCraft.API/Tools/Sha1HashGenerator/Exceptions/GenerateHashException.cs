using System;

namespace HashCraft.API.Tools.Sha1HashGenerator.Exceptions
{
    public class GenerateHashException : Exception
    {
        public GenerateHashException(Exception exception) 
            : base ("Error ocured during hash generation. For more details see inner exception", exception)
        {
        }
    }
}
