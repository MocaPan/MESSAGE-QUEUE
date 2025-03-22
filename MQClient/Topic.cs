namespace EstructurasParaMQBroker;

/// <summary>
/// Representa un tema al que los clientes pueden suscribirse en MQBroker.
/// </summary>
public class Topic
{
    /// <summary>
    /// Nombre del tema.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructor que inicializa un tema con un nombre.
    /// </summary>
    /// <param name="name">Nombre del tema.</param>
    public Topic(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del tema no puede estar vacío.");

        Name = name;
    }

    /// <summary>
    /// Sobrescribe Equals para comparar temas por su nombre.
    /// </summary>
    /// <param name="obj">Objeto a comparar.</param>
    /// <returns>True si los nombres de los temas son iguales, False en caso contrario.</returns>
    public override bool Equals(object obj)
    {
        if (obj is Topic other)
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);

        return false;
    }

    /// <summary>
    /// Sobrescribe GetHashCode para garantizar consistencia en estructuras de datos.
    /// </summary>
    /// <returns>HashCode basado en el nombre del tema.</returns>
    public override int GetHashCode()
    {
        return Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Representación en cadena del tema.
    /// </summary>
    /// <returns>Nombre del tema como string.</returns>
    public override string ToString()
    {
        return Name;
    }
}
