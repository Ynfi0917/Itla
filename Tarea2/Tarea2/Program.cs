/// Christopher 2026-0917///
using System;
using System.Collections.Generic;

var ids = new List<int>();
var nombres = new Dictionary<int, string>();
var apellidos = new Dictionary<int, string>();
var telefonos = new Dictionary<int, string>();
var correos = new Dictionary<int, string>();
var direcciones = new Dictionary<int, string>();
var edades = new Dictionary<int, int>();
var favoritos = new Dictionary<int, bool>();

int siguienteId = 1;
bool running = true;

CargarDatosIniciales();

while (running)
{
    try
    {
        Console.Clear();
        Console.WriteLine("=== AGENDA DE CONTACTOS ===");
        Console.WriteLine("1. Agregar contacto");
        Console.WriteLine("2. Listar contactos");
        Console.WriteLine("3. Buscar contacto");
        Console.WriteLine("4. Modificar contacto");
        Console.WriteLine("5. Eliminar contacto");
        Console.WriteLine("6. Salir");
        Console.Write("Elige una opción: ");

        int opcion = Convert.ToInt32(Console.ReadLine());

        switch (opcion)
        {
            case 1:
                AgregarContacto();
                break;
            case 2:
                ListarContactos();
                break;
            case 3:
                BuscarContacto();
                break;
            case 4:
                ModificarContacto();
                break;
            case 5:
                EliminarContacto();
                break;
            case 6:
                Console.Write("\n¿Seguro que quieres salir? (S/N): ");
                string respuesta = (Console.ReadLine() ?? "").Trim().ToUpper();
                if (respuesta == "S")
                {
                    running = false;
                    Console.WriteLine("Cerrando programa...");
                }
                break;
            default:
                Console.WriteLine("\nOpción no válida. Elige un número del 1 al 6.");
                Pausar();
                break;
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("\nError: debes escribir un número válido.");
        Pausar();
    }
    catch (OverflowException)
    {
        Console.WriteLine("\nError: el número es demasiado grande.");
        Pausar();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nOcurrió un error: {ex.Message}");
        Pausar();
    }
}

void CargarDatosIniciales()
{
    GuardarContacto("Ana", "Martínez", "809-555-0101", "ana@email.com", "Av. Churchill #45", 24, true);
    GuardarContacto("Carlos", "Gómez", "829-555-0202", "carlos@email.com", "Calle El Conde #112", 29, false);
    GuardarContacto("Luis", "Fernández", "849-555-0303", "luis@email.com", "Las Américas Km 27", 21, true);
}

void GuardarContacto(string nombre, string apellido, string telefono, string correo, string direccion, int edad, bool favorito)
{
    int id = siguienteId++;
    ids.Add(id);
    nombres.Add(id, nombre);
    apellidos.Add(id, apellido);
    telefonos.Add(id, telefono);
    correos.Add(id, correo);
    direcciones.Add(id, direccion);
    edades.Add(id, edad);
    favoritos.Add(id, favorito);
}

void AgregarContacto()
{
    Console.Clear();
    Console.WriteLine("--- AGREGAR CONTACTO ---");

    Console.Write("Nombre: ");
    string nombre = (Console.ReadLine() ?? "").Trim();

    Console.Write("Apellido: ");
    string apellido = (Console.ReadLine() ?? "").Trim();

    Console.Write("Teléfono: ");
    string telefono = (Console.ReadLine() ?? "").Trim();

    Console.Write("Correo: ");
    string correo = (Console.ReadLine() ?? "").Trim();

    Console.Write("Dirección: ");
    string direccion = (Console.ReadLine() ?? "").Trim();

    if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(telefono))
    {
        Console.WriteLine("\nNombre, apellido y teléfono son obligatorios.");
        Pausar();
        return;
    }

    Console.Write("Edad: ");
    int edad = Convert.ToInt32(Console.ReadLine());
    if (edad <= 0)
    {
        Console.WriteLine("\nLa edad debe ser mayor a 0.");
        Pausar();
        return;
    }

    Console.Write("¿Es favorito? (1. Sí / 2. No): ");
    int opFav = Convert.ToInt32(Console.ReadLine());
    bool esFavorito = opFav == 1 ? true : false;

    GuardarContacto(nombre, apellido, telefono, correo, direccion, edad, esFavorito);

    Console.WriteLine($"\nContacto guardado con el ID: {siguienteId - 1}");
    Pausar();
}

void ListarContactos()
{
    Console.Clear();
    Console.WriteLine("--- LISTA DE CONTACTOS ---");

    if (ids.Count == 0)
    {
        Console.WriteLine("No hay contactos registrados.");
    }
    else
    {
        MostrarEncabezado();
        foreach (int id in ids)
        {
            MostrarFila(id);
        }
        Console.WriteLine($"\nTotal: {ids.Count} contactos");
    }

    Pausar();
}

void BuscarContacto()
{
    Console.Clear();
    Console.WriteLine("--- BUSCAR CONTACTO ---");

    if (ids.Count == 0)
    {
        Console.WriteLine("No hay contactos registrados.");
        Pausar();
        return;
    }

    Console.WriteLine("1. Por ID");
    Console.WriteLine("2. Por Nombre o Apellido");
    Console.WriteLine("3. Por Teléfono");
    Console.WriteLine("4. Por Correo");
    Console.WriteLine("5. Solo Favoritos");
    Console.Write("Elige cómo buscar: ");

    int criterio = Convert.ToInt32(Console.ReadLine());
    bool encontrado = false;

    switch (criterio)
    {
        case 1:
            Console.Write("Escribe el ID: ");
            int idBuscado = Convert.ToInt32(Console.ReadLine());
            if (ids.Contains(idBuscado))
            {
                MostrarEncabezado();
                MostrarFila(idBuscado);
                encontrado = true;
            }
            break;

        case 2:
            Console.Write("Escribe el nombre o apellido: ");
            string textoNombre = (Console.ReadLine() ?? "").Trim().ToLower();
            foreach (int id in ids)
            {
                if (nombres[id].ToLower().Contains(textoNombre) || apellidos[id].ToLower().Contains(textoNombre))
                {
                    if (!encontrado) MostrarEncabezado();
                    MostrarFila(id);
                    encontrado = true;
                }
            }
            break;

        case 3:
            Console.Write("Escribe el teléfono: ");
            string textoTel = (Console.ReadLine() ?? "").Trim();
            foreach (int id in ids)
            {
                if (telefonos[id].Contains(textoTel))
                {
                    if (!encontrado) MostrarEncabezado();
                    MostrarFila(id);
                    encontrado = true;
                }
            }
            break;

        case 4:
            Console.Write("Escribe el correo: ");
            string textoCorreo = (Console.ReadLine() ?? "").Trim().ToLower();
            foreach (int id in ids)
            {
                if (correos[id].ToLower().Contains(textoCorreo))
                {
                    if (!encontrado) MostrarEncabezado();
                    MostrarFila(id);
                    encontrado = true;
                }
            }
            break;

        case 5:
            foreach (int id in ids)
            {
                if (favoritos[id])
                {
                    if (!encontrado) MostrarEncabezado();
                    MostrarFila(id);
                    encontrado = true;
                }
            }
            break;

        default:
            Console.WriteLine("\nOpción de búsqueda no válida.");
            Pausar();
            return;
    }

    if (!encontrado)
    {
        Console.WriteLine("\nNo se encontraron resultados.");
    }

    Pausar();
}

void ModificarContacto()
{
    Console.Clear();
    Console.WriteLine("--- MODIFICAR CONTACTO ---");

    if (ids.Count == 0)
    {
        Console.WriteLine("No hay contactos para modificar.");
        Pausar();
        return;
    }

    Console.Write("ID del contacto a modificar: ");
    int idEditar = Convert.ToInt32(Console.ReadLine());

    if (!ids.Contains(idEditar))
    {
        Console.WriteLine("\nNo existe un contacto con ese ID.");
        Pausar();
        return;
    }

    Console.WriteLine("\nDeja vacío y presiona ENTER para mantener el dato actual:\n");

    Console.Write($"Nombre ({nombres[idEditar]}): ");
    string nuevoNombre = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevoNombre)) nombres[idEditar] = nuevoNombre.Trim();

    Console.Write($"Apellido ({apellidos[idEditar]}): ");
    string nuevoApellido = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevoApellido)) apellidos[idEditar] = nuevoApellido.Trim();

    Console.Write($"Teléfono ({telefonos[idEditar]}): ");
    string nuevoTel = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevoTel)) telefonos[idEditar] = nuevoTel.Trim();

    Console.Write($"Correo ({correos[idEditar]}): ");
    string nuevoCorreo = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevoCorreo)) correos[idEditar] = nuevoCorreo.Trim();

    Console.Write($"Dirección ({direcciones[idEditar]}): ");
    string nuevaDir = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevaDir)) direcciones[idEditar] = nuevaDir.Trim();

    Console.Write($"Edad ({edades[idEditar]}): ");
    string nuevaEdad = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevaEdad))
    {
        int edadNum = Convert.ToInt32(nuevaEdad);
        if (edadNum > 0) edades[idEditar] = edadNum;
    }

    string favActual = favoritos[idEditar] ? "Sí" : "No";
    Console.Write($"Favorito ({favActual}) - 1. Sí / 2. No: ");
    string nuevoFav = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevoFav))
    {
        int opFav = Convert.ToInt32(nuevoFav);
        favoritos[idEditar] = opFav == 1 ? true : false;
    }

    Console.WriteLine("\nContacto actualizado.");
    Pausar();
}

void EliminarContacto()
{
    Console.Clear();
    Console.WriteLine("--- ELIMINAR CONTACTO ---");

    if (ids.Count == 0)
    {
        Console.WriteLine("No hay contactos para eliminar.");
        Pausar();
        return;
    }

    Console.Write("ID del contacto a eliminar: ");
    int idBorrar = Convert.ToInt32(Console.ReadLine());

    if (!ids.Contains(idBorrar))
    {
        Console.WriteLine("\nNo existe un contacto con ese ID.");
        Pausar();
        return;
    }

    MostrarEncabezado();
    MostrarFila(idBorrar);

    Console.Write("\n¿Seguro que quieres eliminarlo? (S/N): ");
    string confirmar = (Console.ReadLine() ?? "").Trim().ToUpper();

    if (confirmar == "S")
    {
        ids.Remove(idBorrar);
        nombres.Remove(idBorrar);
        apellidos.Remove(idBorrar);
        telefonos.Remove(idBorrar);
        correos.Remove(idBorrar);
        direcciones.Remove(idBorrar);
        edades.Remove(idBorrar);
        favoritos.Remove(idBorrar);

        Console.WriteLine("\nContacto eliminado.");
    }
    else
    {
        Console.WriteLine("\nOperación cancelada.");
    }

    Pausar();
}

void MostrarEncabezado()
{
    Console.WriteLine("\n-------------------------------------------------------------------------------------------------------------");
    Console.WriteLine($"{"ID",-4} | {"Nombre",-12} | {"Apellido",-12} | {"Teléfono",-14} | {"Correo",-22} | {"Edad",-5} | {"Fav",-4} | {"Dirección"}");
    Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
}

void MostrarFila(int id)
{
    string fav = favoritos[id] ? "Sí" : "No";
    Console.WriteLine($"{id,-4} | {nombres[id],-12} | {apellidos[id],-12} | {telefonos[id],-14} | {correos[id],-22} | {edades[id],-5} | {fav,-4} | {direcciones[id]}");
}

void Pausar()
{
    Console.WriteLine("\nPresiona una tecla para continuar...");
    Console.ReadKey();
}
