using System;
using System.Collections.Generic;

namespace HotelFinal
{
    internal class Inicio
    {
        //Variables globales
        static string diaV, nombreHuesped, ingresoNoches = "", mail;
        static int habitacion, dia = 0, numeroHabitacion, idReserva = 0, cantNoches = 0;
        static long dni;
        static bool disponibilidad = true, validarD = true, valNoche = true,
            valHabitacion = true, salida = true;
        static List<ReservasStruct> reservas = new List<ReservasStruct>();
        static List<huesped> huespedes = new List<huesped>();
        // Se crean las dos estructuras para Reservas y Huespedes
        struct ReservasStruct
        {
            public int IdReserva;
            public long DniHuesped;
            public int NumeroHabitacion;
            public DateTime CheckIn;
            public int CantidadNoches;

            public ReservasStruct(int idReserva, long dniHuesped, int numeroHabitacion, DateTime checkIn, int cantidadNoches)
            {
                IdReserva = idReserva;
                DniHuesped = dniHuesped;
                NumeroHabitacion = numeroHabitacion;
                CheckIn = checkIn;
                CantidadNoches = cantidadNoches;
            }
        }
        struct huesped
        {
            public string NombreHuesped;
            public int NumeroHabitacion;
            public long Dni;
            public string Mail;

            public huesped(string nombreHuesped, int numeroHabitacion, long dni, string mail)
            {
                NombreHuesped = nombreHuesped;
                NumeroHabitacion = numeroHabitacion;
                Dni = dni;
                Mail = mail;
            }
        }
        // Matrices para gestionar la disponibilidad de habitaciones para cada mes
        static bool[,] octubre = new bool[31, 10]; // 31 días x 10 habitaciones para octubre
        static bool[,] noviembre = new bool[30, 10]; // 30 días x 10 habitaciones para noviembre
        static bool[,] diciembre = new bool[31, 10]; // 31 días x 10 habitaciones para diciembre
        static void Main(string[] args)
        {
            inicializarMeses();
            cargarDatosDefault();
            idReserva = reservas[reservas.Count-1].IdReserva;

            // Bucle principal del menú
            do
            {
                Console.Clear();
                salida = MenuPrincipal();
            } 
            while (salida); // Repite mientras no se elija la opción de salida
        }
        //Funcion para mostrar un menu y devuelve falso para salir del bucle
       
        static Boolean MenuPrincipal()
        {           
            Console.Clear();           
            string[] opciones = new string[] {
             "Elija una opción",
             "1. Crear Reserva",
             "2. Modificar Reserva",
             "3. Cancelar Reserva",
             "4. Buscar huesped por Dni",
             "5. Buscar Reservas por Dni",
             "6. Mostrar Reservas",
             "7. Mostrar Huespedes",
             "8. Mostrar Disponibilidad",
             "9. Salir"
        };
            Boolean salidaMenu = true;
            Console.ForegroundColor = ConsoleColor.White;
            menuOpciones("Hotel Genesis", opciones);
            string eleccion = Console.ReadLine();
            switch (eleccion)
            {
                case "1":
                    Console.Clear();
                    agregarReserva(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();                   
                    break; // Llama al método para agregar una reserva
                case "2":
                    Console.Clear();
                    modificarReserva(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    break; // Para implementar la modificación de reservas
                case "3":
                    Console.Clear();
                    eliminarReserva(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();                   
                    break; // Para implementar la cancelación de reservas
                case "4":
                    Console.Clear();
                    buscarHuespedDni(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();                    
                    break; // Para buscar reservas por nombre
                case "5":
                    Console.Clear();
                    buscarReservasDni(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();                   
                    break; // Para listar reservas ordenadas
                case "6":
                    Console.Clear();
                    mostrarReservas2(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();                    
                    break;
                case "7":
                    Console.Clear();
                    mostrarHuespedes(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();                   
                    break;
                case "8":
                    Console.Clear();
                    mostrarDisponibilidad(); Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();                    
                    break;
                case "9": 
                    salidaMenu = false; 
                    return salidaMenu; // Salida del menú
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingreso inválido");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Presiona cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
            return salidaMenu;
        }
        //Funcion para centrar el texto del menu
        static string CentrarTexto(string texto, int longitudMaxima)
        {
            int espaciosTotales = longitudMaxima - texto.Length;   // Calcula los espacios que faltan para alinear
            int espaciosIzquierda = espaciosTotales / 2;           // Espacios a la izquierda
            int espaciosDerecha = espaciosTotales - espaciosIzquierda; // Espacios a la derecha

            // Usa PadRight para centrar el texto
            return new string(' ', espaciosIzquierda) + texto + new string(' ', espaciosDerecha).PadRight(espaciosDerecha);
        }
        //Funcion que inicializa en libres todas las habitacion
        static void inicializarMeses()
        {

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    octubre[i, j] = false;
                    diciembre[i, j] = false;
                }
            }
            for (int i = 0; i < 29; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    noviembre[i, j] = false;
                }
            }
        }
        static void agregarReserva()
        {///datos del huesped
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.Write("Ingrese el nombre del huésped: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            nombreHuesped = Console.ReadLine();
            Console.ResetColor();
            Console.Write("Ingrese el DNI del huésped: ");
            bool valIngreso = true;            
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string ingresoDni = Console.ReadLine();
                valIngreso=  validacionLong(ingresoDni,out dni);
            } 
            while (!valIngreso);//validacion de ingreso long (DNI)

            Console.ResetColor();
            Console.Write("Ingrese el mail del huésped: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            mail = Console.ReadLine();
            Console.ResetColor();
            ///datos de la reserva
            string[] opcionesMes = new string[] {
             "1. Octubre",
             "2. Noviembre",
             "3. Diciembre"
        };
            bool opcionValida = true;
            int opcionNumero;
            // valida que se ingrese bien la opcion del mes
            do
            {
                
                menuOpciones("Elije el Mes: ", opcionesMes);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string opcion = Console.ReadLine();
                opcionValida = int.TryParse(opcion, out opcionNumero);
                if (!opcionValida || opcionNumero > 3 || opcionNumero <= 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opcion erronea! Vuelva a ingresar");
                    opcionValida = false;
                }
            } while (!opcionValida);
            Console.ResetColor();
            //se elije el mes y se agrega a la matriz correspondiente

            switch (opcionNumero)
            {
                case 1:
                    segunMes(octubre, 1, 31, 10); // Ejecuta segunMes para cuando se elija octubre
                    break; // Sale del switch después de ejecutar el caso

                case 2:
                    segunMes(noviembre, 2, 30, 11); // Ejecuta segunMes para cuando se elija noviembre
                    break; // Sale del switch después de ejecutar el caso

                case 3:
                    segunMes(diciembre, 3, 31, 12); // Ejecuta segunMes para cuando se elija diciembre
                    break; // Sale del switch después de ejecutar el caso

                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Opción inválida!");
                    Console.ResetColor();
                    break;
            }
        }
        //Funcion que ejecuta el codigo para la reserva segun el mes que se elije en un menu
        static void segunMes(bool[,] mesSelect, int numeroMes, int catDiasMes, int mes)
        {                     
                        
            do
            {
                Console.Write("Ingrese el día: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                diaV = Console.ReadLine();
                validarD = validarDia(diaV, numeroMes);

                if (!validarD)
                {                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingreso no permitido!, vuelva a intentar");
                    Console.ResetColor();
                }
                if (validarD)
                {
                    dia = int.Parse(diaV);
                }

            } while (!validarD);

            Console.ResetColor();

            // validacion de noches:                    

            do
            {
                Console.Write("Ingrese la cantidad de noches: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                ingresoNoches = Console.ReadLine();
                valNoche = validarNoche(ingresoNoches, catDiasMes, dia);
                if (!valNoche)
                {                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingreso no permitido!, vuelva a intentar");
                    Console.ResetColor();
                }
                if (valNoche)
                {
                    cantNoches = int.Parse(ingresoNoches);
                }

            } while (!valNoche);

            do
            {
                //validar numero de habitacion
                do
                {
                    Console.ResetColor();
                    Console.Write("Ingrese del 1 al 10 el numero de habitacion: ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    string entradaHabitacion = Console.ReadLine();
                    valHabitacion = validarHabitacion(entradaHabitacion);
                    if (!valHabitacion)
                    {                      
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ingreso no permitido!, vuelva a intentar");
                        Console.ResetColor();
                    }


                    if (valHabitacion)
                    {
                        numeroHabitacion = int.Parse(entradaHabitacion);
                    }

                } while (!valHabitacion);


                disponibilidad = verificarDisponibilidad(mesSelect, dia, cantNoches, numeroHabitacion);
                if (disponibilidad)
                {
                    idReserva++;
                    DateTime checkIn = new DateTime(2025, mes, dia);
                    ReservasStruct reserva = new ReservasStruct(idReserva, dni, numeroHabitacion, checkIn, cantNoches);
                    reservas.Add(reserva);
                    huesped huespedNuevo = new huesped(nombreHuesped, numeroHabitacion, dni, mail);
                    huespedes.Add(huespedNuevo);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Reserva Creada con Exito!");
                    Console.ResetColor();
                }
                else
                {
                    obtenerHabitacionesDisponibles(mesSelect, dia, cantNoches, numeroHabitacion);
                }
            } while (!disponibilidad);
        }
        //Funcion que valida el ingreso de la cantidad de noches
        static bool validarNoche(string noche, int max, int dia)
        {
            int numeroNoches;
            bool validacionIngreso = int.TryParse(noche, out numeroNoches);
            max = max - (dia-1);
            if (validacionIngreso)
            {
                if (max - numeroNoches < 0 || numeroNoches > max || numeroNoches <= 0)
                {
                    validacionIngreso = false;
                }
            }
            return validacionIngreso;
        }
        //Funcion para validar el ingreso para el numero de habitacion
        static bool validarHabitacion(string ingresoHabitacion)
        {
            int numeroHabitacion = 0;
            bool valHabitacion = int.TryParse(ingresoHabitacion, out numeroHabitacion);
            if (valHabitacion)
            {
                if (numeroHabitacion <= 0 || numeroHabitacion > 10)
                {
                    valHabitacion = false;
                }
            }
            return valHabitacion;

        }
        //Funcion para validar el ingreso del dia check-in
        static bool validarDia(string dia, int mes)
        {
            bool valDia = true;
            int numDia;
            valDia = int.TryParse(dia, out numDia);
            if (valDia)
            {
                valDia = true;
            }
            if (valDia)
            {
                switch (mes)
                {
                    case 1:
                        if (numDia <= 30 && numDia > 0)
                        {
                            valDia = true;
                        }
                        else
                        {
                            valDia = false;
                        }

                        break;
                    case 2:
                        if (numDia <= 31 && numDia > 0)
                        {
                            valDia = true;
                        }
                        else
                        {
                            valDia = false;
                        }

                        break;
                    case 3:
                        if (numDia <= 30 && numDia > 0)
                        {
                            valDia = true;
                        }
                        else
                        {
                            valDia = false;
                        }
                        break;

                    default: return false;

                }
            }
            return valDia;

        }
        //Funcion Para Dibujar Menu
        static void menuOpciones(string titulo, string[] opciones)
        {

            int longitudMax = titulo.Length;
            foreach (string i in opciones)
            {
                if (i.Length > longitudMax)
                {
                    longitudMax = i.Length;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string borde = new string('*', longitudMax + 4);
            Console.WriteLine(borde);
            Console.WriteLine($"* {CentrarTexto(titulo, longitudMax)} *");
            Console.WriteLine(borde);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < opciones.Length; i++)
            {
                if (i == 0)
                {
                    Console.WriteLine($"* {CentrarTexto(opciones[i], longitudMax)} *");
                }
                else
                {
                    Console.WriteLine($"* {opciones[i].PadRight(longitudMax)} *");
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(borde);
            Console.ResetColor();
        }
        static void cargarDatosDefault()
        {
            // Cargar los huéspedes
            huespedes.Add(new huesped("Juan Pérez", 10, 123456, "juan.perez@hotel.com"));
            huespedes.Add(new huesped("María López", 10, 40000002, "maria.lopez@hotel.com"));
            huespedes.Add(new huesped("Carlos García", 3, 40000003, "carlos.garcia@hotel.com"));
            huespedes.Add(new huesped("Ana Martínez", 4, 40000004, "ana.martinez@hotel.com"));
            huespedes.Add(new huesped("Pedro Sánchez", 5, 40000005, "pedro.sanchez@hotel.com"));
            huespedes.Add(new huesped("Lucía Ramírez", 6, 40000006, "lucia.ramirez@hotel.com"));
            huespedes.Add(new huesped("Sofía Torres", 7, 40000007, "sofia.torres@hotel.com"));
            huespedes.Add(new huesped("Miguel Fernández", 8, 40000008, "miguel.fernandez@hotel.com"));
            huespedes.Add(new huesped("Laura González", 9, 40000009, "laura.gonzalez@hotel.com"));
            huespedes.Add(new huesped("Javier Rodríguez", 1, 40000010, "javier.rodriguez@hotel.com"));

            // Cargar las reservas           
            reservas.Add(new ReservasStruct(1, 40000001, 10, new DateTime(2023, 10, 5), 3)); // Octubre
            reservas.Add(new ReservasStruct(2, 40000002, 10, new DateTime(2023, 10, 10), 2)); // Octubre
            reservas.Add(new ReservasStruct(3, 40000003, 3, new DateTime(2023, 10, 20), 4)); // Octubre
            reservas.Add(new ReservasStruct(4, 40000004, 4, new DateTime(2023, 11, 1), 5));  // Noviembre
            reservas.Add(new ReservasStruct(5, 40000005, 5, new DateTime(2023, 11, 10), 3)); // Noviembre
            reservas.Add(new ReservasStruct(6, 40000006, 6, new DateTime(2023, 11, 20), 2)); // Noviembre
            reservas.Add(new ReservasStruct(7, 40000007, 7, new DateTime(2023, 12, 1), 4));  // Diciembre
            reservas.Add(new ReservasStruct(8, 40000008, 8, new DateTime(2023, 12, 10), 5)); // Diciembre
            reservas.Add(new ReservasStruct(9, 40000009, 9, new DateTime(2023, 12, 15), 3)); // Diciembre
            reservas.Add(new ReservasStruct(10, 40000010, 1, new DateTime(2023, 12, 25), 2)); // Diciembre
            reservas.Add(new ReservasStruct(11, 123456, 10, new DateTime(2023, 12, 5), 7)); // Diciembre

            // Actualizar la disponibilidad de las habitaciones
            foreach (var reserva in reservas)
            {
                // Determinar el mes de la reserva
                bool[,] mes;
                int diasMes;
                if (reserva.CheckIn.Month == 10)
                {
                    mes = octubre;
                    diasMes = 31;
                }
                else if (reserva.CheckIn.Month == 11)
                {
                    mes = noviembre;
                    diasMes = 30;
                }
                else if (reserva.CheckIn.Month == 12)
                {
                    mes = diciembre;
                    diasMes = 31;
                }
                else
                {
                    continue; // Ignorar meses que no están en nuestras matrices
                }

                // Marcar los días de la reserva como ocupados
                int diaInicio = reserva.CheckIn.Day - 1; // Día de inicio de la reserva (0-indexado)
                int duracion = reserva.CantidadNoches;
                for (int i = 0; i < duracion; i++)
                {
                    if (diaInicio + i < diasMes)
                    {
                        mes[diaInicio + i, reserva.NumeroHabitacion - 1] = true; // Marcar habitación ocupada
                    }
                }
            }
        }
        //Funcion de sobrecarga que valida el numero de mes el rango de fechas ingresado para reservar
        static bool verificarDisponibilidad(int mess, int diaa, int cantidadNoches, int habitacion)
        {
            var mesElegido=octubre;
            switch (mess)
            {
                case 1:
                    mesElegido = octubre;
                    break;
                case 2:
                    mesElegido = noviembre;
                    break;
                case 3:
                    mesElegido = diciembre;
                    break;   
            }

            for (int i = diaa - 1; i < (diaa-1) + cantidadNoches; i++)
            {
                if (i > mesElegido.GetLength(0) || mesElegido[i - 1, habitacion - 1]) // Día fuera de rango o habitación ocupada
                {
                    return false; // No disponible
                }
            }

            // Si pasa la validación, se marcan los días como ocupados
            for (int i = diaa; i < diaa + cantidadNoches; i++)
            {
                mesElegido[i - 1, habitacion - 1] = true;
            }

            return true; // Disponible
        }
        //Funcion que valida segun el mes si un rango de fecha esta disponible para reservar
        static bool verificarDisponibilidad(bool[,] mess, int diaa, int cantidadNoches, int habitacion)
        {
           

            for (int i = diaa; i < diaa + cantidadNoches; i++)
            {
                if (i > mess.GetLength(0) || mess[i - 1, habitacion - 1]) // Día fuera de rango o habitación ocupada
                {
                    return false; // No disponible
                }
            }

            // Si pasa la validación, se marcan los días como ocupados
            for (int i = diaa; i < diaa+ cantidadNoches; i++)
            {
                mess[i - 1, habitacion] = true;
            }

            return true; // Disponible
        }

        //  Función que retorna una lista de las habitaciones disponibles para un rango de días en un mes específico.
        static void obtenerHabitacionesDisponibles(bool[,] mess, int diaa, int cantidadNoches, int habitacion)
        {
            List<int> habitacionesLibres = new List<int>();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Habitación no disponible..!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Verificando disponibilidad desde el día {diaa} por {cantidadNoches} noches.");

            // Recorre las habitaciones de 1 a 10 para verificar si están disponibles en el rango de días
            for (int h = 0; h < 10; h++) // 10 habitaciones
            {
                bool disponible = true;

                // Verifica si la habitación está disponible en todos los días solicitados
                for (int d = diaa-1; d < (diaa-1) + cantidadNoches; d++)
                {
                    if (d >= mess.GetLength(0) || mess[d - 1, h]) // Día fuera de rango o habitación ocupada
                    {
                        disponible = false;
                        break;
                    }
                }

                // Si la habitación está disponible en todo el rango, la agrega a la lista
                if (disponible)
                {
                    habitacionesLibres.Add(h + 1); // +1 porque las habitaciones empiezan desde 1
                }
            }

            // Muestra las habitaciones disponibles una vez que se ha recorrido todas las habitaciones
            if (habitacionesLibres.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Habitaciones disponibles en el rango solicitado:");
                foreach (int hab in habitacionesLibres)
                {
                    Console.WriteLine($"Habitación {hab}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay habitaciones disponibles para el rango de fechas seleccionado.");
            }

            Console.ResetColor(); // Restaurar colores de consola.
        }
        //Funcion para eliminar una reserva
        static void eliminarReserva()
        {
            Console.Clear();
            long dniHues;
            int codigoEliminacion;
            bool valCodigo = true;
            bool eliminado = false;
            bool huespedEliminado = false; // Nueva bandera para el huésped
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("******_Eliminacion de una Reserva/Huesped_******");
            bool hayReservas = mostrarReservas();
            Console.ResetColor();


            if (hayReservas)
            {
                do
                {
                    Console.Write("Ingrese el id de la reserva que quiera eliminar: ");
                    Console.ResetColor();
                    string codigo = Console.ReadLine();
                    valCodigo = int.TryParse(codigo, out codigoEliminacion);
                    if (!valCodigo)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ingrese formato que corresponda!");
                    }

                    for (int i = reservas.Count - 1; i >= 0; i--)
                    {
                        if (reservas[i].IdReserva == codigoEliminacion)
                        {
                            dniHues = reservas[i].DniHuesped;
                            reservas.RemoveAt(i);
                            eliminado = true;
                            for (int j = huespedes.Count - 1; j >= 0; j--)
                            {
                                if (huespedes[j].Dni == dniHues)
                                {
                                    huespedes.RemoveAt(j);
                                    huespedEliminado = true; // Indicamos que se eliminó al huésped
                                }
                            }
                            if (!huespedEliminado)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("No se encontró un huésped con ese DNI.");
                            }
                        }
                    }

                    if (eliminado)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("¡Reserva eliminada!");
                        if (huespedEliminado)
                        {
                            Console.WriteLine("¡Huésped eliminado!");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No se encontró un huésped para la reserva eliminada.");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No se encontró la reserva.");
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                } while (!valCodigo);
            }
            else if (!hayReservas)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("No existen registros");
            }
        }
        //Funcion para mostrar la reservas Validando que hayan registros
        static bool mostrarReservas()
        {
            //Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach (ReservasStruct res in reservas)
            {
                Console.WriteLine(" ");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Id: ");
                Console.ResetColor();
                Console.Write($"{res.IdReserva}");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($" - DNI: ");
                Console.ResetColor();
                Console.Write(res.DniHuesped);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($" - Habitacion: ");
                Console.ResetColor();
                Console.Write(res.NumeroHabitacion);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($" - Check-in: ");
                Console.ResetColor();
                Console.Write($"{res.CheckIn.Day}/{res.CheckIn.Month}/{res.CheckIn.Year}");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($" - Cant noches: ");
                Console.ResetColor();
                Console.Write(res.CantidadNoches);
                Console.WriteLine(" ");
            }
            if (reservas.Count == 0)
            {
                return false;
            }
            return true;

        }
        //Funcion de sobrecarga para mostrar las reservas 
        static void mostrarReservas2()
        {
            //Console.Clear();
            
            if (reservas.Count > 0)
            {
                int n = reservas.Count;
                //Se ordena las reservas segun cantidad de noches  (de Mayor a menor)
                for (int i = 0; i < n - 1; i++)
                {                   
                    for (int j = 0; j < n - i - 1; j++)
                    {                       
                        if (reservas[j].CantidadNoches < reservas[j + 1].CantidadNoches)
                        {                            
                            ReservasStruct temp = reservas[j];
                            reservas[j] = reservas[j + 1];
                            reservas[j + 1] = temp;
                        }
                    }
                }
                Console.WriteLine("Datos de las Reservas: ");
                foreach (ReservasStruct res in reservas)
                {
                    Console.WriteLine(" ");
                    
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Id: "); 
                    Console.ResetColor();
                    Console.Write($"{res.IdReserva}");
                    
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($" - DNI: ");
                    Console.ResetColor();
                    Console.Write(res.DniHuesped);
                    
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($" - Habitacion: ");
                    Console.ResetColor();
                    Console.Write(res.NumeroHabitacion);
                    
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($" - Check-in: ");
                    Console.ResetColor();
                    Console.Write($"{res.CheckIn.Day}/{res.CheckIn.Month}/{res.CheckIn.Year}");
                    
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($" - Cant noches: ");
                    Console.ResetColor();
                    Console.Write(res.CantidadNoches);
                    Console.WriteLine(" ");
                }
                Console.ResetColor();
            }
            else if (reservas.Count == 0)
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("No se encuentran Registros");
                Console.ResetColor();
            }           
        }
        static void mostrarHuespedes()
        {
            if (huespedes.Count > 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Datos de los  huespedes: ");
                Console.ForegroundColor = ConsoleColor.Green;
                int n = huespedes.Count;
                //Se ordena por numero de DNI
                for (int i = 0; i < n - 1; i++)
                {                   
                    for (int j = 0; j < n - i - 1; j++)
                    {
                        
                        if (huespedes[j].Dni > huespedes[j + 1].Dni)
                        {                           
                            huesped temp = huespedes[j];
                            huespedes[j] = huespedes[j + 1];
                            huespedes[j + 1] = temp;
                        }
                    }
                }
                foreach (huesped guest in huespedes)
                {
                    Console.WriteLine("  ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"Nombre: ");
                    Console.ResetColor();
                    Console.Write(guest.NombreHuesped) ;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" DNI: ");
                    Console.ResetColor();
                    Console.Write(guest.Dni);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" Mail: ");
                    Console.ResetColor();
                    Console.Write(guest.Mail);                   
                    Console.WriteLine(" ");
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay Registros");
                Console.ResetColor();
            }
        }
        //Funcion para buscar un Huesped por el DNI 
        static void buscarHuespedDni()
        {
            if (huespedes.Count > 0)
            {
                mostrarHuespedes();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine();
                Console.Write("Ingrese el DNI del Huesped que desea buscar: ");
                Console.ResetColor();
                bool valDni = true;
                long dniH;
                string ingresoDni;               
                ingresoDni = Console.ReadLine();
                validacionLong(ingresoDni, out dniH);                              
                foreach (var hues in huespedes)
                {
                    if (hues.Dni == dniH)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("DNI ENCONTRADO!");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("Nombre: ");
                        Console.ResetColor();
                        Console.WriteLine(hues.NombreHuesped);

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("DNI: ");
                        Console.ResetColor();
                        Console.WriteLine(hues.Dni);

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("Mail: ");
                        Console.ResetColor();
                        Console.WriteLine(hues.Mail);

 
                    }
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay Registros");
                Console.ResetColor();
            }
        }
        //Funcion para buscar un reserva por el DNI del Huesped
        static void buscarReservasDni()
        {
            if (reservas.Count > 0)
            {
                mostrarReservas();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine();
                Console.Write("Ingrese el DNI del huesped que desea buscar: ");
                Console.ResetColor();
                bool valDni = true;
                long dniH;
                string ingresoDni;                
                ingresoDni = Console.ReadLine();
                validacionLong(ingresoDni, out dniH);              
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                foreach (ReservasStruct res in reservas)
                {
                    if (res.DniHuesped == dniH)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("DNI ENCONTRADO!");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write($"Id:");
                        Console.ResetColor();
                        Console.WriteLine(res.IdReserva);

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write($"DNI: ");
                        Console.ResetColor();
                        Console.WriteLine(res.DniHuesped);
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("Habitacion: ");
                        Console.ResetColor();
                        Console.WriteLine(res.NumeroHabitacion);

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write($"Check-in: ");
                        Console.ResetColor();
                        Console.WriteLine($"{res.CheckIn.Day}/{res.CheckIn.Month}/{res.CheckIn.Year}");

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write($"Cant de noches: ");
                        Console.ResetColor();
                        Console.WriteLine(res.CantidadNoches);
                    }
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay Registros");
                Console.ResetColor();
            }
        }
        //Funcion para Modificar un reserva (UPDATE)
        static void modificarReserva()
        {
            ReservasStruct reservaModificacada = new ReservasStruct();
            bool hayRes=  mostrarReservas();
            if (hayRes)
            {
                Console.ResetColor();
                Console.Write("Indica el numero del ID de la reserva que desea modificar: ");
                bool valIngreso = true;
                bool idEncontrado = true;
                long idIngresado = 0;
                int indice = -1;
                do
                {
                    do
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        string ingresoDni = Console.ReadLine();
                        valIngreso = validacionLong(ingresoDni, out idIngresado);
                    } while (!valIngreso);//validacion de ingreso long (DNI)
                    idEncontrado = false;
                    for (int i = 0; i < reservas.Count; i++)
                    {
                        if (reservas[i].IdReserva == idIngresado)
                        {
                            reservaModificacada = reservas[i];
                            idEncontrado = true;
                            indice = i;
                            break;
                        }

                    }
                    if (!idEncontrado)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ID no encontrado! Vuelva a intentar: ");
                        Console.ResetColor();
                    }
                } while (!idEncontrado);
                string[] opciones = new string[]
                {
                "1.Habitacion",
                "2.Check-in",
                "3.Cantidad de noches",
                };
                menuOpciones("Elija que quiere modificar:", opciones);
                int elegirOpcion = int.Parse(Console.ReadLine());
                bool verificarD = true;

                switch (elegirOpcion)
                {
                    case 1:
                        Console.Write($"Habitacion actual: {reservaModificacada.NumeroHabitacion} Nueva habitacion: ");
                        string entrada = "";
                        int dato;
                        entrada = Console.ReadLine();
                        reservaModificacada.NumeroHabitacion = validacionInt(entrada, out dato);
                        verificarD = verificarDisponibilidad(reservaModificacada.CheckIn.Month, reservaModificacada.CheckIn.Day, reservaModificacada.CantidadNoches, reservaModificacada.NumeroHabitacion);
                        break;
                    case 2:
                        Console.Write($"Fecha de entrada: {reservaModificacada.CheckIn.Day} / {reservaModificacada.CheckIn.Month} / {reservaModificacada.CheckIn.Year} Nuevo Dato: ");
                        entrada = Console.ReadLine();
                        DateTime fecha;
                        reservaModificacada.CheckIn = validacionDate(entrada, out fecha);
                        verificarD = verificarDisponibilidad(reservaModificacada.CheckIn.Month, reservaModificacada.CheckIn.Day, reservaModificacada.CantidadNoches, reservaModificacada.NumeroHabitacion);
                        break;
                    case 3:
                        Console.Write($"Noches: {reservaModificacada.CantidadNoches} Nuevo Dato: ");
                        entrada = Console.ReadLine();
                        reservaModificacada.CantidadNoches = validacionInt(entrada, out dato);
                        verificarD = verificarDisponibilidad(reservaModificacada.CheckIn.Month, reservaModificacada.CheckIn.Day, reservaModificacada.CantidadNoches, reservaModificacada.NumeroHabitacion);
                        break;
                    default:
                        break;

                }
                reservas[indice] = reservaModificacada;
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Datos guardados");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine("No hay Registros");
                Console.ResetColor();
            }
        }
        //Funcion para la validacion de entrada para un Long
        static bool validacionLong(string ingreso,out long dato)
        {
            bool validacion ;            
                Console.ForegroundColor = ConsoleColor.DarkYellow;             
                validacion = long.TryParse(ingreso, out dato);
                if (!validacion)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingreso invalido! vuelva a ingresar:");
                    return false;
                }
            return true;              
        }
        //Funcion para la validacion de entrada para un entero
        static int validacionInt(string ingreso, out int dato)
        {
            bool val = true;            
            do
            {               
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                val = int.TryParse(ingreso, out dato);
                if (!val)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingreso invalido! vuelva a ingresar:");
                    Console.ResetColor();
                    ingreso = Console.ReadLine();
                }             
            } while (!val);
            return dato;
        }
        //Funcion para la validacion de entrada de una fecha
        static DateTime validacionDate(string ingreso, out DateTime dato)
        {
            bool val=true;
            do
            {                
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                 val = DateTime.TryParse(ingreso, out dato);
                if (!val)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingreso invalido! vuelva a ingresar:");
                    Console.ResetColor();
                    ingreso = Console.ReadLine();
                }
            } while (!val);
            return dato;
        }
        //Funcion para mostrar un calendario de disponibilidad de acuerdo el mes
        static void mostrarDisponibilidad()
        {
            Console.Clear();
            string[] opciones = new string[]
            {
                "1. Octubre",
                "2. Noviembre",
                "3. Diciembre"
            };
            menuOpciones("Elija el mes: ",opciones);
            string entrada= Console.ReadLine();
            int op;
            validacionInt(entrada,out op);

            switch (op)
            {
               case 1:
                    dibujarCalendario(octubre); 
                    break;
               case 2:
                    dibujarCalendario(noviembre);
                    break; 
                case 3: 
                    dibujarCalendario(diciembre);
                    break;
                default: 
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Opcion Invalida");
                    Console.ResetColor();
                    break;
            }
        }
        //Funcion para Dibujar en consola El calendario para la Funcion mostrarDisponibilidad()
        static void dibujarCalendario(bool[,] mess)
        {
            char check = '\u2713';  // ✓
            char cruz = 'X'; // ✗            
            string tablaHab = " Hab 1  | Hab 2  | Hab 3  | Hab 4  |  Hab 5  | Hab 6  | Hab 7  | Hab 8  | Hab 9  | Hab 10 | ";
            string tablaDia = " Dia || ";
            string linea = repetirCaracter('_', tablaHab.Length + tablaDia.Length);

            // Encabezado de tabla
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(tablaDia);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(tablaHab);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(linea);
            Console.ResetColor();

            // Filas del calendario
            for (int i = 0; i < mess.GetLength(0); i++)
            {
                // Dibuja el número de día
                Console.Write("  " + (i + 1).ToString("D2") + " ");                
                for (int j = 0; j < mess.GetLength(1); j++)
                {
                    if (mess[i, j])
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("      " + cruz + "  ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("      " + check + "  ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine(" ");  // Salta a la siguiente fila después de imprimir las habitaciones de un día
            }
        }
        // Función auxiliar para crear líneas divisorias
        static string repetirCaracter(char caracter, int longitud)
        {
            return new string(caracter, longitud);
        }
    }
}
