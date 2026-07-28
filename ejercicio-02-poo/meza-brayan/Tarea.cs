namespace GestorTareas
{
    public class Tarea : IExportable
    {
        private static int contadorId = 0;

        public int Id { get; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Prioridad Prioridad { get; set; }
        public string Categoria { get; set; }
        public bool Completada { get; set; }
        public DateTime FechaCreacion { get; }

        public Tarea(string titulo, string descripcion, Prioridad prioridad, string categoria)
        {
            contadorId++;
            Id = contadorId;
            Titulo = titulo;
            Descripcion = descripcion;
            Prioridad = prioridad;
            Categoria = categoria;
            Completada = false;
            FechaCreacion = DateTime.Now;
        }

        public Tarea() : this("Sin título", "", Prioridad.Baja, "General") { }

        public virtual void MostrarInfo()
        {
            string estado = Completada ? "[COMPLETADA]" : "[PENDIENTE]";
            string color = Prioridad switch
            {
                Prioridad.Critica => "\u001b[31m",
                Prioridad.Alta => "\u001b[33m",
                Prioridad.Media => "\u001b[36m",
                _ => "\u001b[32m"
            };
            string reset = "\u001b[0m";

            Console.WriteLine($"{color}#{Id} {Titulo} {reset}| {Descripcion} | Prioridad: {Prioridad} | Cat: {Categoria} | {estado} | Creada: {FechaCreacion:dd/MM/yyyy HH:mm}");
        }

        public string Exportar()
        {
            return $"{Id}|{Titulo}|{Prioridad}|{Completada}";
        }

        public static void ReiniciarContador(int ultimoId)
        {
            contadorId = ultimoId;
        }
    }
}
