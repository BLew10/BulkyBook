using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace BulkyBook.Models;

public class CoverType
{
    [Key]
    public int Id { get; set; }

    [Required]
	[DisplayName("Cover Type")]
	[MaxLength(50)]
	public string Name { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
}

