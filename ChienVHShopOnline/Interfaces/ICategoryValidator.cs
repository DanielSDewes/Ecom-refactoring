using System.Collections.Generic;
using ChienVHShopOnline.Models;

namespace ChienVHShopOnline.Interfaces
{
    public interface ICategoryValidator
    {
        ValidationResult Validate(Category category);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public IList<string> Errors { get; } = new List<string>();
    }
}
