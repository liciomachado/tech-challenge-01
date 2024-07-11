using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechChallenge01.Domain.ValueObjects;

namespace TechChallenge01.Domain;

[Table("contacts", Schema = "public")]
public class Contact
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

    public RegionDDD DDD { get; set; }

    public Contact() { }

    public Contact(string name, string phoneNumber, string email)
    {
        var value = int.Parse(phoneNumber.Substring(0, 2));
        DDD = new RegionDDD(value);
        PhoneNumber = phoneNumber;
        Name = name;
        Email = email;
    }
}
