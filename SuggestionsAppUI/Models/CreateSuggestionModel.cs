using System.ComponentModel.DataAnnotations;

namespace SuggestionsAppUI.Models
{
    public class CreateSuggestionModel
    {
        [Required]
        [MaxLength(100)]
        public string Suggestion { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
