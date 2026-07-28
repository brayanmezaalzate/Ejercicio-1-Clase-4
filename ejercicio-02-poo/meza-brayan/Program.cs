namespace GestorTareas
{
    class Program
    {
        static string archivoJSON = "tareas.json";
        static GestorTareas gestor = new GestorTareas();

        static void Main()
        {
            gestor.CargarDeJSON(archivoJSON);

            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": AgregarTarea(); break;
                    case "2": ListarTodas(); break;
                    case "3": ListarPorCategoria(); break;
                    case "4": ListarPorPrioridad(); break;
                    case "5": MarcarCompletada(); break;
                    case "6": MostrarVencidas(); break;
                    case "7": EliminarTarea(); break;
                    case "8": ExportarJSON(); break;
                    case "9": salir = true; break;
                    default: Console.WriteLine("Opción no válida."); break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }

            gestor.GuardarEnJSON(archivoJSON);
            Console.WriteLine("¡Hasta luego!");
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== GESTOR DE TAREAS ===");
            Console.WriteLine("1. Agregar tarea");
            Console.WriteLine("2. Listar todas");
            Console.WriteLine("3. Listar por categoría");
            Console.WriteLine("4. Listar por prioridad");
            Console.WriteLine("5. Marcar como completada");
            Console.WriteLine("6. Mostrar tareas vencidas");
            Console.WriteLine("7. Eliminar tarea");
            Console.WriteLine("8. Exportar a JSON");
            Console.WriteLine("9. Salir");
            Console.Write("\nSelecciona una opción: ");
        }

        static void AgregarTarea()
        {
            Console.Write("Título: ");
            string titulo = Console.ReadLine() ?? "";

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine() ?? "";

            Console.WriteLine("Prioridad: 1=Baja, 2=Media, 3=Alta, 4=Crítica");
            Console.Write("Selecciona: ");
            string? prioOpcion = Console.ReadLine();
            Prioridad prioridad = prioOpcion switch
            {
                "2" => Prioridad.Media,
                "3" => Prioridad.Alta,
                "4" => Prioridad.Critica,
                _ => Prioridad.Baja
            };

            Console.Write("Categoría: ");
            string categoria = Console.ReadLine() ?? "General";

            Console.Write("¿Tiene fecha de vencimiento? (s/n): ");
            string? conVencimiento = Console.ReadLine();

            if (conVencimiento?.ToLower() == "s")
            {
                Console.Write("Fecha de vencimiento (dd/MM/yyyy): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fechaVencimiento))
                {
                    TareaConVencimiento tarea = new TareaConVencimiento(titulo, descripcion, prioridad, categoria, fechaVencimiento);
                    gestor.Agregar(tarea);
                    Console.WriteLine($"Tarea con vencimiento agregada con ID #{tarea.Id}");
                }
                else
                {
                    Console.WriteLine("Fecha inválida. Se creó sin vencimiento.");
                    Tarea tarea = new Tarea(titulo, descripcion, prioridad, categoria);
                    gestor.Agregar(tarea);
                }
            }
            else
            {
                Tarea tarea = new Tarea(titulo, descripcion, prioridad, categoria);
                gestor.Agregar(tarea);
                Console.WriteLine($"Tarea agregada con ID #{tarea.Id}");
            }
        }

        static void ListarTodas()
        {
            List<Tarea> tareas = gestor.ObtenerTodas();
            Console.WriteLine($"\n--- {tareas.Count} tarea(s) encontrada(s) ---\n");

            // Ejemplo de polimorfismo: lista que puede contener Tarea y TareaConVencimiento
            List<Tarea> listaPolimorfica = tareas.Cast<Tarea>().ToList();
            foreach (Tarea t in listaPolimorfica)
            {
                t.MostrarInfo(); // Se llama al método correcto según el tipo real
                Console.WriteLine();
            }

            Console.WriteLine("--- Fin del listado ---");
        }

        static void ListarPorCategoria()
        {
            Console.Write("Categoría a buscar: ");
            string categoria = Console.ReadLine() ?? "";
            List<Tarea> tareas = gestor.ListarPorCategoria(categoria);

            Console.WriteLine($"\n--- {tareas.Count} tarea(s) en '{categoria}' ---\n");
            foreach (Tarea t in tareas)
            {
                t.MostrarInfo();
                Console.WriteLine();
            }
        }

        static void ListarPorPrioridad()
        {
            Console.WriteLine("Prioridad: 1=Baja, 2=Media, 3=Alta, 4=Crítica");
            Console.Write("Selecciona: ");
            string? prioOpcion = Console.ReadLine();
            Prioridad prioridad = prioOpcion switch
            {
                "2" => Prioridad.Media,
                "3" => Prioridad.Alta,
                "4" => Prioridad.Critica,
                _ => Prioridad.Baja
            };

            List<Tarea> tareas = gestor.ListarPorPrioridad(prioridad);
            Console.WriteLine($"\n--- {tareas.Count} tarea(s) con prioridad {prioridad} ---\n");
            foreach (Tarea t in tareas)
            {
                t.MostrarInfo();
                Console.WriteLine();
            }
        }

        static void MarcarCompletada()
        {
            Console.Write("ID de la tarea a completar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                gestor.Completar(id);
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
        }

        static void MostrarVencidas()
        {
            List<Tarea> vencidas = gestor.ObtenerVencidas();
            Console.WriteLine($"\n--- {vencidas.Count} tarea(s) vencida(s) ---\n");
            foreach (Tarea t in vencidas)
            {
                t.MostrarInfo();
                Console.WriteLine();
            }
        }

        static void EliminarTarea()
        {
            Console.Write("ID de la tarea a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                gestor.Eliminar(id);
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
        }

        static void ExportarJSON()
        {
            gestor.GuardarEnJSON(archivoJSON);
        }
    }
}
