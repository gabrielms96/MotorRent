#region Mainetence
/*
Comment: Created Service for Brazilian document validations.
Updated: 10/03/2025
Author:  Gabriel MS
*/
#endregion
using System.Text.RegularExpressions;

namespace MotorRentService.Services;

/// <summary>
/// Service for Brazilian document validations
/// </summary>
public class ValidationService : IValidationService
{

    public bool IsValidCNPJ(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = Regex.Replace(cnpj, @"[^\d]", "");

        if (cnpj.Length != 14)
            return false;

        if (cnpj.All(c => c == cnpj[0]))
            return false;

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;
        for (int i = 0; i < 12; i++)
        {
            soma += int.Parse(cnpj[i].ToString()) * multiplicador1[i];
        }
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cnpj[12].ToString()) != digito1)
            return false;

        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;
        for (int i = 0; i < 13; i++)
        {
            soma += int.Parse(cnpj[i].ToString()) * multiplicador2[i];
        }
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return int.Parse(cnpj[13].ToString()) == digito2;
    }

    public bool IsValidCNHNumber(string cnhNumber)
    {
        if (string.IsNullOrWhiteSpace(cnhNumber))
            return false;

        cnhNumber = Regex.Replace(cnhNumber, @"[^\d]", "");

        if (cnhNumber.Length != 11)
            return false;

        if (cnhNumber.All(c => c == cnhNumber[0]))
            return false;

        return true;
    }

    public bool IsValidCNHTypeForMotorcycle(string cnhType)
    {
        if (string.IsNullOrWhiteSpace(cnhType))
            return false;

        var validTypes = new[] { "A", "AB"};
        return validTypes.Contains(cnhType.ToUpper().Trim());
    }

    public bool IsValidAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age >= 18;
    }

    public bool IsValidBirthDate(DateTime birthDate)
    {
        var today = DateTime.Today;
        var minDate = today.AddYears(-120);
        var maxDate = today.AddYears(-16);

        return birthDate >= minDate && birthDate <= maxDate;
    }

    public bool IsValidLicensePlate(string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            return false;

        licensePlate = licensePlate.Trim().ToUpper();

        var oldPattern = @"^[A-Z]{3}-?\d{4}$";

        var mercosulPattern = @"^[A-Z]{3}\d[A-Z]\d{2}$";

        return Regex.IsMatch(licensePlate, oldPattern) || 
               Regex.IsMatch(licensePlate, mercosulPattern);
    }
}

