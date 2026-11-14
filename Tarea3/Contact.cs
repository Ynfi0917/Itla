namespace Tarea_3;

public class Contact
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public int Edad { get; set; }
    public bool EsFavorito { get; private set; }

    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

    public Contact()
    {
    }

    public Contact(int id, string nombre, string apellido, string telefono, string correo, string direccion, int edad, bool esFavorito)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Correo = correo;
        Direccion = direccion;
        Edad = edad;
        EsFavorito = esFavorito;
    }

    public void EstablecerFavorito(bool favorito)
    {
        EsFavorito = favorito;
    }

    public void Actualizar(string? nombre, string? apellido, string? telefono, string? correo, string? direccion, int? edad, bool? esFavorito)
    {
        if (!string.IsNullOrWhiteSpace(nombre)) Nombre = nombre.Trim();
        if (!string.IsNullOrWhiteSpace(apellido)) Apellido = apellido.Trim();
        if (!string.IsNullOrWhiteSpace(telefono)) Telefono = telefono.Trim();
        if (!string.IsNullOrWhiteSpace(correo)) Correo = correo.Trim();
        if (!string.IsNullOrWhiteSpace(direccion)) Direccion = direccion.Trim();
        if (edad.HasValue && edad.Value > 0) Edad = edad.Value;
        if (esFavorito.HasValue) EsFavorito = esFavorito.Value;
    }

    public override string ToString()
    {
        string fav = EsFavorito ? "Sí" : "No";
        return $"{Id,-4} | {NombreCompleto,-22} | {Telefono,-14} | {Correo,-24} | {Edad,-5} | {fav,-4} | {Direccion}";
    }
}
