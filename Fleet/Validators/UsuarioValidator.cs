using Fleet.Interfaces.Repository;
using Fleet.Models;
using FluentValidation;
using Fleet.Resources;
using Fleet.Enums;
using System.Text.RegularExpressions;


namespace Fleet.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator(IUsuarioRepository usuarioRepository, UsuarioRequestEnum request)
        {   
            if(request != UsuarioRequestEnum.Deletar) {
                if (request != UsuarioRequestEnum.AtualizarSenha) {
                    RuleFor(x => x.CPF).Must(cpf => IsValidCPF(cpf))
                                    .WithMessage("CPF invalido");
                
                    RuleFor(x => x.Nome).NotEmpty()
                                    .WithMessage("Campo nome obrigatorio");
                }
             
                RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage(Resource.usario_emailInvalido);
            }

            if (request == UsuarioRequestEnum.AtualizarSenha) {
                RuleFor(x => x.Email).MustAsync(async (email, cancellationToken) => await usuarioRepository.ExisteEmail(email))
                                     .WithMessage("Email nao encontrado na base");
            }               

            if(request == UsuarioRequestEnum.Criar) {
                RuleFor(x => x.Email).MustAsync(async (email, cancellationToken) => !await usuarioRepository.ExisteEmail(email))
                                     .WithMessage(Resource.usuario_emailduplicado);
                RuleFor(x => x.CPF).MustAsync(async (cpf,cancellationToken)  => !await usuarioRepository.ExisteCpf(cpf))
                                .WithMessage("CPF ja existe");
            }
               

            if(request == UsuarioRequestEnum.Deletar || request == UsuarioRequestEnum.Atualizar) {
                RuleFor(x => x.Id).MustAsync(async (id, cancellationToken) => await usuarioRepository.Existe(id))
                                .WithMessage("Usuario nao existe");
                if(request == UsuarioRequestEnum.Atualizar) {
                    RuleFor(x => x).MustAsync(async (x, cancellationToken) => !await usuarioRepository.ExisteEmail(x.Email, x.Id))
                                        .WithMessage(Resource.usuario_emailduplicado);
                    RuleFor(x => x).MustAsync(async (x,cancellationToken)  => !await usuarioRepository.ExisteCpf(x.CPF, x.Id))
                                .WithMessage("CPF ja existe");
                }
            }
        }
        private static bool IsValidCPF(string cpf)
        {

            if (string.IsNullOrWhiteSpace(cpf))
            return false;

            // Remove non-numeric characters
            cpf = Regex.Replace(cpf, "[^0-9]", string.Empty);

            if (cpf.Length != 11)
                return false;

            // Invalid CPFs (e.g., 111.111.111-11)
            string[] invalidCpfs =
            {
                "00000000000", "11111111111", "22222222222", "33333333333",
                "44444444444", "55555555555", "66666666666", "77777777777",
                "88888888888", "99999999999"
            };

            if (invalidCpfs.Contains(cpf))
                return false;

            // Calculating the first digit
            int[] multipliers1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * multipliers1[i];

            int remainder = sum % 11;
            int firstDigit = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[9].ToString()) != firstDigit)
                return false;

            // Calculating the second digit
            int[] multipliers2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * multipliers2[i];

            remainder = sum % 11;
            int secondDigit = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[10].ToString()) != secondDigit)
                return false;

            return true;
        }
    }
}