using System.ComponentModel.DataAnnotations;

namespace ChienVHShopOnline.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
