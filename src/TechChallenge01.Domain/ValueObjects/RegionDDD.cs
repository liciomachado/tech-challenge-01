namespace TechChallenge01.Domain.ValueObjects;

public class RegionDDD
{
    /// <summary>
    ///  HashSet é usado para armazenar os DDDs válidos, pois oferece busca rápida.
    /// </summary>
    private static readonly HashSet<int> ValidDDDs = new HashSet<int>
    {
        11, 12, 13, 14, 15, 16, 17, 18, 19, // São Paulo
        21, 22, 24, // Rio de Janeiro
        27, 28, // Espírito Santo
        31, 32, 33, 34, 35, 37, 38, // Minas Gerais
        41, 42, 43, 44, 45, 46, // Paraná
        47, 48, 49, // Santa Catarina
        51, 53, 54, 55, // Rio Grande do Sul
        61, // Distrito Federal
        62, 64, // Goiás
        63, // Tocantins
        65, 66, // Mato Grosso
        67, // Mato Grosso do Sul
        68, // Acre
        69, // Rondônia
        71, 73, 74, 75, 77, // Bahia
        79, // Sergipe
        81, 87, // Pernambuco
        82, // Alagoas
        83, // Paraíba
        84, // Rio Grande do Norte
        85, 88, // Ceará
        86, 89, // Piauí
        91, 93, 94, // Pará
        92, 97, // Amazonas
        95, // Roraima
        96, // Amapá
        98, 99 // Maranhão
    };

    public int Value { get; set; }

    /// <summary>
    /// O construtor  DDD(int value)  valida se o DDD fornecido está na lista de DDDs válidos. Se não estiver, uma exceção  ArgumentException  é lançada.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    public RegionDDD(int value)
    {

        if (!ValidDDDs.Contains(value))
        {
            throw new ArgumentException("DDD inválido.");
        }

        Value = value;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (RegionDDD)obj;
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}