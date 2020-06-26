using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace septa.Auth.Domain.ViewModel
{
    public class RoleViewModel
    {
        [HiddenInput]
        public string Id { set; get; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام نقش")]
        public string Name { set; get; }
    }
}
