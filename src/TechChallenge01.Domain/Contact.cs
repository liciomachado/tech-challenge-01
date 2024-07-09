using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechChallenge01.Domain.Core;

namespace TechChallenge01.Domain;

[Table("contacts", Schema = "public")]
public class Contact : IAggregateRoot
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
    [Column("email")]
    public string Email { get; set; }

    public Contact(string name, string phoneNumber, string email)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}
