using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CashCard.Models
{
    public class QuizGroup
    {
        public int Id { get; set; }
        [Required]
        [Remote("CheckName", "RegularGroup")]
        [Display(Name = "GL Account")]
        public string AccountNo { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string AccountDesription { get; set; }
        [Display(Name = "Group Type")]
        public GroupType GroupType { get; set; }
        public string Info { get; set; }
    }
}