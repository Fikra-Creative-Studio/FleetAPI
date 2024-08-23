using System.Diagnostics.CodeAnalysis;

namespace Fleet.Models
{
    [ExcludeFromCodeCoverage]
    public class BussinessException : Exception
    {
        public BussinessException(string mensagem) : base(mensagem)
        {

        }
    }
}