using System;
using System.Collections.Generic;
using Tarea_3;

AgendaService agenda = new AgendaService();
bool running = true;

while (running)
{
    try
    {
        Console.Clear();
        Console.WriteLine("=========================================================================");
        Console.WriteLine("                  AGENDA DE CONTACTOS (VERSIÓN POO)");
        Console.WriteLine("=========================================================================");
        Console.WriteLine("1. Agregar contacto");
        Console.WriteLine("2. Listar todos los contactos");
        Console.WriteLine("3. Buscar contacto");
        Console.WriteLine("4. Modificar contacto");
        Console.WriteLine("5. Eliminar contacto");
        Console.WriteLine("6. Salir");
        Console.WriteLine("=========================================================================");
        Console.Write("Seleccione una opción (1-6): ");

        int opcion = Convert.ToInt32(Console.ReadLine());

        switch (opcion)
        {
            case 1:
                AgregarContactoUI();
                break;
            case 2:
                ListarContactosUI();
                break;
            case 3:
                BuscarContactoUI();
                break;
            case 4:
                ModificarContactoUI();
                break;
            case 5:
                EliminarContactoUI();
                break;
            case 6:
                Console.Write("\n¿Está seguro que desea salir? (S/N): ");
                string respuesta = (Console.ReadLine() ?? "").Trim().ToUpper();
                if (respuesta == "S")
                {
                    running = false;
                    Console.WriteLine("Cerrando aplicación...");
                }
                break;
            default:
                Console.WriteLine("\n[Aviso] Opción no válida. Digite un número del 1 al 6.");
                Pausar();
                break;
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("\n[Error] Debe ingresar un valor numérico válido.");
        Pausar();
    }
    catch (OverflowException)
    {
        Console.WriteLine("\n[Error] El número ingresado es demasiado grande.");
        Pausar();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[Error inesperado] {ex.Message}");
        Pausar();
    }
}

void AgregarContactoUI()
{
    Console.Clear();
    Console.WriteLine("--- AGREGAR NUEVO CONTACTO ---");

    string nombre = LeerTextoObligatorio("Nombre: ");
    string apellido = LeerTextoObligatorio("Apellido: ");
    string telefono = LeerTextoObligatorio("Teléfono: ");
    string correo = LeerTextoObligatorio("Correo electrónico: ");
    string direccion = LeerTextoObligatorio("Dirección: ");
    int edad = LeerEnteroPositivo("Edad: ");

    Console.Write("¿Es contacto favorito? (1. Sí / 2. No): ");
    bool esFavorito = Convert.ToInt32(Console.ReadLine()) == 1;

    Contact creado = agenda.Agregar(nombre, apellido, telefono, correo, direccion, edad, esFavorito);
    Console.WriteLine($"\n[Éxito] Contacto '{creado.NombreCompleto}' registrado con el ID #{creado.Id}.");
    Pausar();
}

void ListarContactosUI()
{
    Console.Clear();
    Console.WriteLine("--- LISTADO DE CONTACTOS ---");

    List<Contact> lista = agenda.ObtenerTodos();
    if (lista.Count == 0)
    {
        Console.WriteLine("No hay contactos registrados.");
    }
    else
    {
        ImprimirTabla(lista);
        Console.WriteLine($"\nTotal de contactos: {agenda.TotalContactos}");
    }

    Pausar();
}

void BuscarContactoUI()
{
    Console.Clear();
    Console.WriteLine("--- BUSCAR CONTACTOS ---");

    if (agenda.TotalContactos == 0)
    {
        Console.WriteLine("No hay contactos disponibles para buscar.");
        Pausar();
        return;
    }

    Console.WriteLine("1. Buscar por ID");
    Console.WriteLine("2. Buscar por Nombre, Teléfono, Correo o Dirección");
    Console.WriteLine("3. Mostrar solo Favoritos");
    Console.Write("Seleccione una opción (1-3): ");

    int criterio = Convert.ToInt32(Console.ReadLine());

    switch (criterio)
    {
        case 1:
            Console.Write("Digite el ID: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Contact? encontrado = agenda.BuscarPorId(id);
            if (encontrado != null)
            {
                ImprimirTabla(new List<Contact> { encontrado });
            }
            else
            {
                Console.WriteLine("\nNo se encontró ningún contacto con ese ID.");
            }
            break;

        case 2:
            string texto = LeerTextoObligatorio("Digite el texto a buscar: ");
            List<Contact> resultados = agenda.BuscarPorCriterio(texto);
            if (resultados.Count > 0)
            {
                ImprimirTabla(resultados);
            }
            else
            {
                Console.WriteLine("\nNo hubo coincidencias con su búsqueda.");
            }
            break;

        case 3:
            List<Contact> favoritos = agenda.ObtenerFavoritos();
            if (favoritos.Count > 0)
            {
                ImprimirTabla(favoritos);
            }
            else
            {
                Console.WriteLine("\nNo tiene contactos marcados como favoritos.");
            }
            break;

        default:
            Console.WriteLine("\n[Aviso] Criterio no válido.");
            break;
    }

    Pausar();
}

void ModificarContactoUI()
{
    Console.Clear();
    Console.WriteLine("--- MODIFICAR CONTACTO ---");

    if (agenda.TotalContactos == 0)
    {
        Console.WriteLine("No hay contactos disponibles para modificar.");
        Pausar();
        return;
    }

    Console.Write("Digite el ID del contacto a modificar: ");
    int id = Convert.ToInt32(Console.ReadLine());

    Contact? actual = agenda.BuscarPorId(id);
    if (actual == null)
    {
        Console.WriteLine($"\n[Aviso] No existe un contacto con el ID #{id}.");
        Pausar();
        return;
    }

    Console.WriteLine("\nPresione ENTER sin escribir nada para mantener el dato actual:\n");

    Console.Write($"Nombre ({actual.Nombre}): ");
    string? nombre = Console.ReadLine();

    Console.Write($"Apellido ({actual.Apellido}): ");
    string? apellido = Console.ReadLine();

    Console.Write($"Teléfono ({actual.Telefono}): ");
    string? telefono = Console.ReadLine();

    Console.Write($"Correo ({actual.Correo}): ");
    string? correo = Console.ReadLine();

    Console.Write($"Dirección ({actual.Direccion}): ");
    string? direccion = Console.ReadLine();

    Console.Write($"Edad ({actual.Edad}): ");
    string? edadTexto = Console.ReadLine();
    int? nuevaEdad = string.IsNullOrWhiteSpace(edadTexto) ? null : Convert.ToInt32(edadTexto);

    string favActual = actual.EsFavorito ? "Sí" : "No";
    Console.Write($"Favorito ({favActual}) -> 1. Sí / 2. No (ENTER para mantener): ");
    string? favTexto = Console.ReadLine();
    bool? nuevoFav = string.IsNullOrWhiteSpace(favTexto) ? null : (Convert.ToInt32(favTexto) == 1);

    agenda.Modificar(id, nombre, apellido, telefono, correo, direccion, nuevaEdad, nuevoFav);
    Console.WriteLine("\n[Éxito] Contacto actualizado correctamente.");
    Pausar();
}

void EliminarContactoUI()
{
    Console.Clear();
    Console.WriteLine("--- ELIMINAR CONTACTO ---");

    if (agenda.TotalContactos == 0)
    {
        Console.WriteLine("No hay contactos disponibles para eliminar.");
        Pausar();
        return;
    }

    Console.Write("Digite el ID del contacto a eliminar: ");
    int id = Convert.ToInt32(Console.ReadLine());

    Contact? contacto = agenda.BuscarPorId(id);
    if (contacto == null)
    {
        Console.WriteLine($"\n[Aviso] No existe un contacto con el ID #{id}.");
        Pausar();
        return;
    }

    ImprimirTabla(new List<Contact> { contacto });

    Console.Write("\n¿Confirma que desea eliminar este contacto? (S/N): ");
    string confirmacion = (Console.ReadLine() ?? "").Trim().ToUpper();

    if (confirmacion == "S")
    {
        agenda.Eliminar(id);
        Console.WriteLine("\n[Éxito] Contacto eliminado correctamente.");
    }
    else
    {
        Console.WriteLine("\nOperación cancelada.");
    }

    Pausar();
}

string LeerTextoObligatorio(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string entrada = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(entrada))
        {
            return entrada.Trim();
        }
        Console.WriteLine("[Aviso] Este campo es obligatorio.");
    }
}

int LeerEnteroPositivo(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        int valor = Convert.ToInt32(Console.ReadLine());
        if (valor > 0)
        {
            return valor;
        }
        Console.WriteLine("[Aviso] El número debe ser mayor que cero.");
    }
}

void ImprimirTabla(List<Contact> contactos)
{
    Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------------------------");
    Console.WriteLine($"{"ID",-4} | {"Nombre Completo",-22} | {"Teléfono",-14} | {"Correo",-24} | {"Edad",-5} | {"Fav",-4} | {"Dirección"}");
    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
    foreach (Contact c in contactos)
    {
        Console.WriteLine(c);
    }
}

void Pausar()
{
    Console.WriteLine("\nPresione cualquier tecla para continuar...");
    Console.ReadKey();
}
