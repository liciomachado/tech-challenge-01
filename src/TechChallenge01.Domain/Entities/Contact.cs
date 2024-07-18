using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Domain.Entities;

[Table("contacts", Schema = "public")]
public class Contact
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    public PhoneNumber PhoneNumber { get; set; }
    [Column("email")]
    public string Email { get; set; }
    public Contact() { }

    public Contact(string name, PhoneNumber phoneNumber, string email)
    {
        PhoneNumber = phoneNumber;
        Name = name;
        Email = email;
    }

    public void Update(string name, PhoneNumber phoneNumber, string email)
    {
        PhoneNumber = phoneNumber;
        Name = name;
        Email = email;
    }
}
