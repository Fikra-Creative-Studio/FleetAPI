using Fleet.Enums;
using Fleet.Interfaces.Repository;
using Fleet.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Fleet.Validators;

public class WorkspaceValidator : AbstractValidator<Workspace>
{
    public WorkspaceValidator(IWorkspaceRepository workspaceRepository, WorkspaceRequestEnum request)
    {
        if (request != WorkspaceRequestEnum.Deletar) {
            RuleFor(x => x.Fantasia)
                .NotEmpty()
                .WithMessage("Campo nome-fantasia obrigatório");

            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .WithMessage("Campo cnpj obrigatório");
            
            RuleFor(x => x.Cnpj)
                .Must(IsValidCnpj)
                .WithMessage("Cnpj formato inválido");
        }
           
        if (request != WorkspaceRequestEnum.Criar)
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) => !await workspaceRepository.Existe(id))
                .WithMessage("Workspace inválido");
        
        if (request == WorkspaceRequestEnum.Criar)
            RuleFor(x => x.Cnpj)
                .MustAsync(async (cnpj, cancellationToken) => !await workspaceRepository.ExisteCnpj(cnpj))
                .WithMessage("CNPJ inválido");
        
        if (request ==WorkspaceRequestEnum.Atualizar)
            RuleFor(x => x)
                .MustAsync(async (x, cancellationToken) => !await workspaceRepository.ExisteCnpj(x.Cnpj, x.Id))
                .WithMessage("CNPJ inválido");
    }

    private static bool IsValidCnpj(string cnpj)
    {
        // Remove non-numeric characters
        cnpj = Regex.Replace(cnpj, "[^0-9]", "");

        // Check if the CNPJ has 14 digits
        if (cnpj.Length != 14)
            return false;

        // Validate the check digits
        int[] multipliers1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multipliers2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        int sum = 0;
        for (int i = 0; i < 12; i++)
            sum += int.Parse(cnpj[i].ToString()) * multipliers1[i];

        int remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        if (int.Parse(cnpj[12].ToString()) != remainder)
            return false;

        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += int.Parse(cnpj[i].ToString()) * multipliers2[i];

        remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        if (int.Parse(cnpj[13].ToString()) != remainder)
            return false;

        return true;
    }
    
}
