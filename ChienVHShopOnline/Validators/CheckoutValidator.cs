using System;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.ViewModels;

namespace ChienVHShopOnline.Validators
{
    public class CheckoutValidator : ICheckoutValidator
    {
        public ValidationResult Validate(CheckoutViewModel checkout)
        {
            var result = new ValidationResult();

            if (checkout == null)
            {
                result.IsValid = false;
                result.Errors.Add("Dados de checkout inválidos.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(checkout.CustomerName))
            {
                result.IsValid = false;
                result.Errors.Add("O nome do cliente é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(checkout.CustomerPhone))
            {
                result.IsValid = false;
                result.Errors.Add("O telefone do cliente é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(checkout.CustomerEmail))
            {
                result.IsValid = false;
                result.Errors.Add("O e-mail do cliente é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(checkout.CustomerAddress))
            {
                result.IsValid = false;
                result.Errors.Add("O endereço do cliente é obrigatório.");
            }

            return result;
        }
    }
}
