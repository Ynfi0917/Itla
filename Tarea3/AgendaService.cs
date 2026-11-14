using System.Collections.Generic;
using System.Linq;

namespace Tarea_3;

public class AgendaService
{
    private readonly List<Contact> _contactos = new();
    private int _siguienteId = 1;

    public AgendaService()
    {
        CargarDatosIniciales();
    }

    public int TotalContactos => _contactos.Count;

    public Contact Agregar(string nombre, string apellido, string telefono, string correo, string direccion, int edad, bool esFavorito)
    {
        Contact nuevo = new Contact(_siguienteId++, nombre, apellido, telefono, correo, direccion, edad, esFavorito);
        _contactos.Add(nuevo);
        return nuevo;
    }

    public List<Contact> ObtenerTodos()
    {
        return _contactos.OrderBy(c => c.Id).ToList();
    }

    public Contact? BuscarPorId(int id)
    {
        return _contactos.FirstOrDefault(c => c.Id == id);
    }

    public List<Contact> BuscarPorCriterio(string texto)
    {
        string filtro = texto.Trim().ToLower();
        return _contactos
            .Where(c => c.NombreCompleto.ToLower().Contains(filtro)
                     || c.Telefono.ToLower().Contains(filtro)
                     || c.Correo.ToLower().Contains(filtro)
                     || c.Direccion.ToLower().Contains(filtro))
            .ToList();
    }

    public List<Contact> ObtenerFavoritos()
    {
        return _contactos.Where(c => c.EsFavorito).ToList();
    }

    public bool Modificar(int id, string? nombre, string? apellido, string? telefono, string? correo, string? direccion, int? edad, bool? esFavorito)
    {
        Contact? contacto = BuscarPorId(id);
        if (contacto == null) return false;

        contacto.Actualizar(nombre, apellido, telefono, correo, direccion, edad, esFavorito);
        return true;
    }

    public bool Eliminar(int id)
    {
        Contact? contacto = BuscarPorId(id);
        if (contacto == null) return false;

        return _contactos.Remove(contacto);
    }

    private void CargarDatosIniciales()
    {
        Agregar("Ana", "Martínez", "809-555-0101", "ana.martinez@email.com", "Av. Winston Churchill #45", 24, true);
        Agregar("Carlos", "Gómez", "829-555-0202", "carlos.gomez@email.com", "Calle El Conde #112", 29, false);
        Agregar("Luis", "Fernández", "849-555-0303", "luis.fernandez@email.com", "Autopista Las Américas Km 27", 21, true);
    }
}
