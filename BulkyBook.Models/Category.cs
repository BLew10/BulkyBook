using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace BulkyBook.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }

    public DateTime CreateDateTime { get; set; } = DateTime.Now;
}

