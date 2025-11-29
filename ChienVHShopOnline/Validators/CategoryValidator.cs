using System;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Validators
{
    public class CategoryValidator : ICategoryValidator
    {
        private readonly ICategoryRepository _repository;

        public CategoryValidator(ICategoryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ValidationResult Validate(Category category)
        {
            var result = new ValidationResult();

            if (category == null)
            {
                result.IsValid = false;
                result.Errors.Add("Categoria inválida.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                result.IsValid = false;
                result.Errors.Add("Nome da categoria é obrigatório.");
            }

            if (_repository.ExistsByName(
                    category.Name,
                    category.CategoryId == 0 ? (int?)null : category.CategoryId))
            {
                result.IsValid = false;
                result.Errors.Add("Já existe uma categoria com este nome.");
            }

            return result;
        }
    }
}
