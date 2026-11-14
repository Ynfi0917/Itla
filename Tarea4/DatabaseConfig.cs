using Microsoft.Data.SqlClient;

namespace Tarea4;

public static class DatabaseConfig
{
    private const string ServerName = "localhost";
    private const string DatabaseName = "AgendaDB";

    public static string MasterConnectionString =>
        $"Server={ServerName};Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

    public static string ConnectionString =>
        $"Server={ServerName};Database={DatabaseName};Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(ConnectionString);
    }

    public static void AsegurarBaseDeDatos()
    {
        using (SqlConnection masterConn = new SqlConnection(MasterConnectionString))
        {
            masterConn.Open();
            string crearDbSql = $"IF DB_ID('{DatabaseName}') IS NULL CREATE DATABASE {DatabaseName};";
            using SqlCommand cmdDb = new SqlCommand(crearDbSql, masterConn);
            cmdDb.ExecuteNonQuery();
        }

        using (SqlConnection dbConn = ObtenerConexion())
        {
            dbConn.Open();
            string crearTablaSql = @"
                IF OBJECT_ID('dbo.Contacts', 'U') IS NULL
                BEGIN
                    CREATE TABLE Contacts (
                        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        Nombre NVARCHAR(50) NOT NULL,
                        Apellido NVARCHAR(50) NOT NULL,
                        Telefono NVARCHAR(25) NOT NULL,
                        Correo NVARCHAR(100) NOT NULL,
                        Direccion NVARCHAR(200) NOT NULL,
                        Edad INT NOT NULL CHECK (Edad > 0),
                        EsFavorito BIT NOT NULL DEFAULT 0
                    );
                END";
            using SqlCommand cmdTabla = new SqlCommand(crearTablaSql, dbConn);
            cmdTabla.ExecuteNonQuery();

            string contarSql = "SELECT COUNT(*) FROM Contacts;";
            using SqlCommand cmdContar = new SqlCommand(contarSql, dbConn);
            int total = (int)cmdContar.ExecuteScalar();

            if (total == 0)
            {
                string seedSql = @"
                    INSERT INTO Contacts (Nombre, Apellido, Telefono, Correo, Direccion, Edad, EsFavorito)
                    VALUES 
                        ('Ana', 'Martínez', '809-555-0101', 'ana.martinez@email.com', 'Av. Winston Churchill #45', 24, 1),
                        ('Carlos', 'Gómez', '829-555-0202', 'carlos.gomez@email.com', 'Calle El Conde #112', 29, 0),
                        ('Luis', 'Fernández', '849-555-0303', 'luis.fernandez@email.com', 'Autopista Las Américas Km 27', 21, 1);";
                using SqlCommand cmdSeed = new SqlCommand(seedSql, dbConn);
                cmdSeed.ExecuteNonQuery();
            }
        }
    }
}
