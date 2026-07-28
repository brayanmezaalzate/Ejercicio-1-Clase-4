using System;
using System.IO;

class Program
{
    static int validas = 0;
    static int invalidas = 0;

    static void Main()
    {
        int opcion;

        do
        {
            Console.WriteLine("VALIDADOR DE TARJETAS");
            Console.WriteLine("1. Validar una tarjeta");
            Console.WriteLine("2. Validar desde archivo");
            Console.WriteLine("3. Generar número válido");
            Console.WriteLine("4. Estadísticas");
            Console.WriteLine("5. Salir");

            Console.Write("Seleccione una opción: ");
            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Console.Write("Ingrese número de tarjeta: ");
                    string numero = Console.ReadLine();

                    string marca = IdentificarMarca(numero);
                    bool esValida = ValidarTarjeta(numero);

                    Console.WriteLine("Número: " + numero);
                    Console.WriteLine("Marca: " + marca);
                    Console.WriteLine("Estado: " + (esValida ? " VÁLIDA" : " INVÁLIDA"));

                    if (esValida) validas++;
                    else invalidas++;
                    break;

                case 2:
                    Console.Write("Ingrese ruta del archivo: ");
                    string ruta = Console.ReadLine();
                    ValidarDesdeArchivo(ruta);
                    break;

                case 3:
                    string generado = GenerarNumeroValido();
                    Console.WriteLine("Número generado: " + generado);
                    Console.WriteLine("Marca: " + IdentificarMarca(generado));
                    break;

                case 4:
                    MostrarEstadisticas();
                    break;

            }

        } while (opcion != 5);
    }

    
    static bool ValidarTarjeta(string numero)
    {
        int suma = 0;
        bool duplicar = false;

        for (int i = numero.Length - 1; i >= 0; i--)
        {
            int digito = (int)char.GetNumericValue(numero[i]);

            if (duplicar)
            {
                digito *= 2;
                if (digito > 9)
                    digito -= 9;
            }

            suma += digito;
            duplicar = !duplicar;
        }

        return suma % 10 == 0;
    }

    
    static string IdentificarMarca(string numero)
    {
        if (numero.StartsWith("4"))
            return "Visa";

        if (numero.Length >= 2)
        {
            int prefijo = int.Parse(numero.Substring(0, 2));
            if (prefijo >= 51 && prefijo <= 55)
                return "Mastercard";
        }

        if (numero.StartsWith("34") || numero.StartsWith("37"))
            return "American Express";

        if (numero.StartsWith("6011") || numero.StartsWith("65"))
            return "Discover";

        return "Desconocida";
    }

    
    static void ValidarDesdeArchivo(string ruta)
    {
        try
        {
            string[] lineas = File.ReadAllLines(ruta);

            foreach (string linea in lineas)
            {
                string marca = IdentificarMarca(linea);
                bool valida = ValidarTarjeta(linea);

                Console.WriteLine(linea + " - " + marca + " - " + (valida ? "VÁLIDA" : "INVÁLIDA"));

                if (valida) validas++;
                else invalidas++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al leer archivo: " + e.Message);
        }
    }

    
    static string GenerarNumeroValido()
    {
        Random rnd = new Random();
        string numero = "4"; // Visa

        for (int i = 0; i < 14; i++)
        {
            numero += rnd.Next(0, 10);
        }

        
        for (int i = 0; i < 10; i++)
        {
            string intento = numero + i;
            if (ValidarTarjeta(intento))
                return intento;
        }

        return numero + "0";
    }

    
    static void MostrarEstadisticas()
    {
        Console.WriteLine("ESTADÍSTICAS");
        Console.WriteLine("Válidas: " + validas);
        Console.WriteLine("Inválidas: " + invalidas);
    }
}
