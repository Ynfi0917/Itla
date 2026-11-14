using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Tarea4;

public class ContactRepository
{
    public int Agregar(Contact contacto)
    {
        const string sql = @"
            INSERT INTO Contacts (Nombre, Apellido, Telefono, Correo, Direccion, Edad, EsFavorito)
            VALUES (@Nombre, @Apellido, @Telefono, @Correo, @Direccion, @Edad, @EsFavorito);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using SqlConnection conn = DatabaseConfig.ObtenerConexion();
        conn.Open();

        using SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nombre", contacto.Nombre);
        cmd.Parameters.AddWithValue("@Apellido", contacto.Apellido);
        cmd.Parameters.AddWithValue("@Telefono", contacto.Telefono);
        cmd.Parameters.AddWithValue("@Correo", contacto.Correo);
        cmd.Parameters.AddWithValue("@Direccion", contacto.Direccion);
        cmd.Parameters.AddWithValue("@Edad", contacto.Edad);
        cmd.Parameters.AddWithValue("@EsFavorito", contacto.EsFavorito);

        int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
        contacto.Id = nuevoId;
        return nuevoId;
    }

    public List<Contact> ObtenerTodos()
    {
        List<Contact> lista = new();
        const string sql = "SELECT Id, Nombre, Apellido, Telefono, Correo, Direccion, Edad, EsFavorito FROM Contacts ORDER BY Id;";

        using SqlConnection conn = DatabaseConfig.ObtenerConexion();
        conn.Open();

        using SqlCommand cmd = new SqlCommand(sql, conn);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(MapearContacto(reader));
        }

        return lista;
    }

    public Contact? BuscarPorId(int id)
    {
        const string sql = "SELECT Id, Nombre, Apellido, Telefono, Correo, Direccion, Edad, EsFavorito FROM Contacts WHERE Id = @Id;";

        using SqlConnection conn = DatabaseConfig.ObtenerConexion();
        conn.Open();

        using SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return MapearContacto(reader);
        }

        return null;
    }

    public List<Contact> BuscarPorCriterio(string texto)
    {
        List<Contact> lista = new();
        const string sql = @"
            SELECT Id, Nombre, Apellido, Telefono, Correo, Direccion, Edad, EsFavorito
            FROM Contacts
            WHERE Nombre LIKE @Filtro
               OR Apellido LIKE @Filtro
               OR Telefono LIKE @Filtro
               OR Correo LIKE @Filtro
               OR Direccion LIKE @Filtro
            ORDER BY Id;";

        using SqlConnection conn = DatabaseConfig.ObtenerConexion();
        conn.Open();

        using SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Filtro", $"%{texto.Trim()}%");

        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(MapearContacto(reader));
        }

        return lista;
    }

    public List<Contact> ObtenerFavoritos()
    {
        List<Contact> lista = new();
        const string sql = "SELECT Id, Nombre, Apellido, Telefono, Correo, Direccion, Edad, EsFavorito FROM Contacts WHERE EsFavorito = 1 ORDER BY Id;";

        using SqlConnection conn = DatabaseConfig.ObtenerConexion();
        conn.Open();

        using SqlCommand cmd = new SqlCommand(sql, conn);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(MapearContacto(reader));
        }

        return lista;
    }

    public bool Modificar(Contact contacto)
    {
        const string sql = @"
            UPDATE Contacts
            SET Nombre = @Nombre,
                Apellido = @Apellido,
                Telefono = @Telefono,
                Correo = @Correo,
                Direccion = @Direccion,
                Edad = @Edad,
                EsFavorito = @EsFavorito
            WHERE Id = @Id;";

        using SqlConnection conn = DatabaseConfig.ObtenerConexion();
        conn.Open();

        using SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", contacto.Id);
        cmd.Parameters.AddWithValue("@Nombre", contacto.Nombre);
        cmd.Parameters.AddWithValue("@Apellido", contacto.Apellido);
        cmd.Parameters.AddWithValue("@Telefono", contacto.Telefono);
        cmd.Parameters.AddWithValue("@Correo", contacto.Correo);
        cmd.Parameters.AddWithValue("@Direccion", contacto.Direccion);
        cmd.Parameters.AddWithValue("@Edad", contacto.Edad);
        cmd.Parameters.AddWithValue("@EsFavorito", contacto.EsFavorito);

        return cmd.ExecuteNonQuery() > 0;
    }

    public bool Eliminar(int id)
    {
        const string sql = "DELETE FROM Contacts WHERE Id = @Id;";

        using SqlConnection conn = DatabaseConfig.ObtenerConexion();
        conn.Open();

        using SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        return cmd.ExecuteNonQuery() > 0;
    }

    private static Contact MapearContacto(SqlDataReader reader)
    {
        return new Contact(
            id: reader.GetInt32(0),
            nombre: reader.GetString(1),
            apellido: reader.GetString(2),
            telefono: reader.GetString(3),
            correo: reader.GetString(4),
            direccion: reader.GetString(5),
            edad: reader.GetInt32(6),
            esFavorito: reader.GetBoolean(7)
        );
    }
}
