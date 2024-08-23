namespace Fleet.Interfaces.Service;

public interface IEmailService
{
    Task EnviarEmail(string email,string nome, string assunto, string message);
}
