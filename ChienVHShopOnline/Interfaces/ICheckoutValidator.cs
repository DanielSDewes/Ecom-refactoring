using ChienVHShopOnline.ViewModels;

namespace ChienVHShopOnline.Interfaces
{
    public interface ICheckoutValidator
    {
        ValidationResult Validate(CheckoutViewModel checkout);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public System.Collections.Generic.IList<string> Errors { get; } = new System.Collections.Generic.List<string>();
    }
}
