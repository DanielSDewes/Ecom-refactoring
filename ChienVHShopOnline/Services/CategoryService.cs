using System;
using System.Collections.Generic;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ICategoryValidator _validator;

        public CategoryService(ICategoryRepository repository, ICategoryValidator validator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public IEnumerable<Category> GetAllOrdered()
        {
            return _repository.GetAllOrdered();
        }

        public Category GetById(int id)
        {
            return _repository.GetById(id);
        }

        public ServiceResult Create(Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = string.Join("; ", validation.Errors)
                };
            }

            try
            {
                _repository.Add(category);
                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public ServiceResult Update(Category category)
        {
            var validation = _validator.Validate(category);
            if (!validation.IsValid)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = string.Join("; ", validation.Errors)
                };
            }

            try
            {
                _repository.Update(category);
                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public ServiceResult Delete(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "Categoria não encontrada."
                };
            }

            try
            {
                _repository.Delete(category);
                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
