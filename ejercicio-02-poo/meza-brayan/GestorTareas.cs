using System.Text.Json;

namespace GestorTareas
{
    public class GestorTareas
    {
        private List<Tarea> tareas = new List<Tarea>();

        public void Agregar(Tarea tarea)
        {
            tareas.Add(tarea);
        }

        public void Completar(int id)
        {
            Tarea? tarea = tareas.Find(t => t.Id == id);
            if (tarea != null)
            {
                tarea.Completada = true;
                Console.WriteLine($"Tarea #{id} marcada como completada.");
            }
            else
            {
                Console.WriteLine($"Tarea #{id} no encontrada.");
            }
        }

        public List<Tarea> ListarPorCategoria(string categoria)
        {
            return tareas.Where(t => t.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Tarea> ListarPorPrioridad(Prioridad prioridad)
        {
            return tareas.Where(t => t.Prioridad == prioridad).ToList();
        }

        public List<Tarea> ObtenerVencidas()
        {
            return tareas.OfType<TareaConVencimiento>().Where(t => DateTime.Now > t.FechaVencimiento).ToList<Tarea>();
        }

        public void Eliminar(int id)
        {
            Tarea? tarea = tareas.Find(t => t.Id == id);
            if (tarea != null)
            {
                tareas.Remove(tarea);
                Console.WriteLine($"Tarea #{id} eliminada.");
            }
            else
            {
                Console.WriteLine($"Tarea #{id} no encontrada.");
            }
        }

        public List<Tarea> ObtenerTodas()
        {
            return tareas;
        }

        public void GuardarEnJSON(string archivo)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };

            var datosGuardar = new List<Dictionary<string, object?>>();
            foreach (var tarea in tareas)
            {
                var datos = new Dictionary<string, object?>
                {
                    { "Id", tarea.Id },
                    { "Titulo", tarea.Titulo },
                    { "Descripcion", tarea.Descripcion },
                    { "Prioridad", tarea.Prioridad.ToString() },
                    { "Categoria", tarea.Categoria },
                    { "Completada", tarea.Completada },
                    { "FechaCreacion", tarea.FechaCreacion },
                    { "Tipo", tarea.GetType().Name }
                };

                if (tarea is TareaConVencimiento tv)
                {
                    datos["FechaVencimiento"] = tv.FechaVencimiento;
                }

                datosGuardar.Add(datos);
            }

            string json = JsonSerializer.Serialize(datosGuardar, opciones);
            File.WriteAllText(archivo, json);
            Console.WriteLine($"Datos guardados en {archivo}");
        }

        public List<Tarea> CargarDeJSON(string archivo)
        {
            try
            {
                if (!File.Exists(archivo))
                {
                    Console.WriteLine("No existe archivo previo. Iniciando sistema vacío.");
                    return new List<Tarea>();
                }

                string json = File.ReadAllText(archivo);
                var datos = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(json);

                if (datos == null) return new List<Tarea>();

                int maxId = 0;
                tareas.Clear();

                foreach (var item in datos)
                {
                    int id = item["Id"].GetInt32();
                    string titulo = item["Titulo"].GetString() ?? "";
                    string descripcion = item["Descripcion"].GetString() ?? "";
                    Prioridad prioridad = Enum.Parse<Prioridad>(item["Prioridad"].GetString() ?? "Baja");
                    string categoria = item["Categoria"].GetString() ?? "General";
                    bool completada = item["Completada"].GetBoolean();
                    DateTime fechaCreacion = item["FechaCreacion"].GetDateTime();
                    string tipo = item["Tipo"].GetString() ?? "Tarea";

                    Tarea tarea;
                    if (tipo == "TareaConVencimiento" && item.ContainsKey("FechaVencimiento"))
                    {
                        DateTime fechaVencimiento = item["FechaVencimiento"].GetDateTime();
                        tarea = new TareaConVencimiento(titulo, descripcion, prioridad, categoria, fechaVencimiento);
                    }
                    else
                    {
                        tarea = new Tarea(titulo, descripcion, prioridad, categoria);
                    }

                    tarea.Completada = completada;
                    tareas.Add(tarea);

                    if (id > maxId) maxId = id;
                }

                Tarea.ReiniciarContador(maxId);
                Console.WriteLine($"Cargadas {tareas.Count} tareas desde {archivo}");
                return tareas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar archivo: {ex.Message}");
                Console.WriteLine("Iniciando sistema vacío.");
                return new List<Tarea>();
            }
        }
    }
}
