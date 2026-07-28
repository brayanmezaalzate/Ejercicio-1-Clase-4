namespace GestorTareas
{
    public class TareaConVencimiento : Tarea
    {
        public DateTime FechaVencimiento { get; set; }

        public int DiasRestantes
        {
            get
            {
                int dias = (FechaVencimiento - DateTime.Now).Days;
                return dias < 0 ? 0 : dias;
            }
        }

        public TareaConVencimiento(string titulo, string descripcion, Prioridad prioridad, string categoria, DateTime fechaVencimiento)
            : base(titulo, descripcion, prioridad, categoria)
        {
            FechaVencimiento = fechaVencimiento;
        }

        public override void MostrarInfo()
        {
            base.MostrarInfo();
            string estadoVencimiento = DateTime.Now > FechaVencimiento ? "\u001b[31mVENCIDA\u001b[0m" : $"\u001b[33m{DiasRestantes} días restantes\u001b[0m";
            Console.WriteLine($"   └─ Vence: {FechaVencimiento:dd/MM/yyyy} | {estadoVencimiento}");
        }
    }
}
