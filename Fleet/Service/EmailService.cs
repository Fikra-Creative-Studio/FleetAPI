using System.Net;
using System.Net.Mail;
using Fleet.Interfaces.Service;

namespace Fleet.Service;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private string Email_Emissor { get => _configuration.GetValue<string>("Credentials:Email_Envio"); }
    private string Email_Servidor { get => _configuration.GetValue<string>("Credentials:Email_Servidor"); }
    private int Email_Porta { get =>int.Parse(_configuration.GetValue<string>("Credentials:Email_Porta")); }
    private string Email_Senha { get => _configuration.GetValue<string>("Credentials:Email_Senha"); }

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task EnviarEmail(string email,string nome, string assunto, string message)
    {

        // Configuração do e-mail
        var fromAddress = new MailAddress(Email_Emissor, "No Reply Fleet");
        var toAddress = new MailAddress(email, nome);

        // Cria o objeto SMTP
        var smtp = new SmtpClient
        {
            Host = Email_Servidor, // Exemplo: smtp.gmail.com
            Port = Email_Porta, // Porta SMTP (padrão para TLS)
            EnableSsl = true, // Habilitar SSL para segurança
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, Email_Senha)
        };

        // Cria o objeto MailMessage
        using var m = new MailMessage(fromAddress, toAddress)
        {
            Subject = assunto,
            Body = message
        };
        await smtp.SendMailAsync(m); // Envia o e-mail
    }
}
