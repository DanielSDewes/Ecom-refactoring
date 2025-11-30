using System.ComponentModel.DataAnnotations;

namespace ChienVHShopOnline.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        public string CustomerAddress { get; set; }
    }
}
