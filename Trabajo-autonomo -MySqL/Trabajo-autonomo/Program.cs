using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabajo_autonomo
{
    internal class Program
    {

        private static string connectionString = "Server=localhost;uid=root;pwd=123456;database=recursos_humanos";

        static void Main(string[] args)
        {

            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("Menú de Administración de Usuarios MySQL");
                Console.WriteLine("Selecciona una opción: ");
                Console.WriteLine("1. Crear Usuario");
                Console.WriteLine("2. Modificar Usuario");
                Console.WriteLine("3. Eliminar Usuario");
                Console.WriteLine("4. Mostrar Usuarios");
                Console.WriteLine("5. Crear Rol");
                Console.WriteLine("6. Asignar Rol a Usuario");
                Console.WriteLine("7. Mostrar Roles");
                Console.WriteLine("8. Eliminar Rol");
                Console.WriteLine("9. Respaldar base de datos");
                Console.WriteLine("10. Restaurar base de datos");
                Console.WriteLine("11. Listar las entidades");
                Console.WriteLine("12. Listar atributos por entidades");
                Console.WriteLine("13. Agregar entidades con atributos a su consulta");
                Console.WriteLine("14. Generar reporte en PDF");
                Console.WriteLine("15. Generar procedimientos CRUD");
                Console.WriteLine("16. Salir");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        CrearUsuario();
                        break;
                    case "2":
                        Console.Clear();
                        ModificarUsuario();
                        break;
                    case "3":
                        Console.Clear();
                        EliminarUsuario();
                        break;
                    case "4":
                        Console.Clear();
                        ListarUsuarios();
                        break;
                    case "5":
                        Console.Clear();
                        CrearRol();
                        break;
                    case "6":
                        Console.Clear();
                        AsignarRolAUsuario();
                        break;
                    case "7":
                        Console.Clear();
                        ListarRoles();
                        break;
                    case "8":
                        Console.Clear();
                        EliminarRol();
                        break;
                    case "9":
                        Console.Clear();
                        RespaldarBaseDatos();
                        break;
                    case "10":
                        Console.Clear();
                        RestaurarBaseDatos();
                        break;
                    case "11":
                        Console.Clear();
                        ListarEntidades();
                        break;
                    case "12":
                        Console.Clear();
                        ListarAtributosPorEntidades();
                        break;
                    case "13":
                        Console.Clear();
                        AgregarEntidadesAConsulta();
                        break;
                    case "14":
                        Console.Clear();
                        GenerarReportePDF();
                        break;
                    case "15":
                        Console.Clear();
                        GenerarScriptCRUD();
                        break;
                    case "16":
                        continuar = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Opción no válida. Intenta nuevamente.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private static MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(connectionString);
        }

        private static void CrearUsuario()
        {
            Console.Write("Introduce el nombre del nuevo usuario: ");
            string nuevoUsuario = Console.ReadLine();

            Console.Write("Introduce la contraseña para el nuevo usuario: ");
            string contrasena = Console.ReadLine();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = $"CREATE USER '{nuevoUsuario}'@'localhost' IDENTIFIED BY '{contrasena}';";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Usuario creado con éxito.");
            }
        }

        private static void ModificarUsuario()
        {
            Console.Write("Introduce el nombre del usuario a modificar: ");
            string usuario = Console.ReadLine();

            Console.Write("Introduce el nuevo nombre para el usuario (déjalo vacío para no cambiarlo): ");
            string nuevoNombre = Console.ReadLine();

            Console.Write("Introduce la nueva contraseña (déjalo vacío para no cambiarla): ");
            string nuevaContrasena = Console.ReadLine();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = "";

                if (!string.IsNullOrWhiteSpace(nuevoNombre))
                {
                    query = $"RENAME USER '{usuario}'@'localhost' TO '{nuevoNombre}'@'localhost';";
                }

                if (!string.IsNullOrWhiteSpace(nuevaContrasena))
                {
                    query += $" ALTER USER '{nuevoNombre}'@'localhost' IDENTIFIED BY '{nuevaContrasena}';";
                }

                if (!string.IsNullOrWhiteSpace(query))
                {
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Usuario modificado con éxito.");
                }
                else
                {
                    Console.WriteLine("No se realizaron cambios porque no se proporcionaron valores nuevos.");
                }
            }
        }

        private static void EliminarUsuario()
        {
            Console.Write("Introduce el nombre del usuario a eliminar: ");
            string usuario = Console.ReadLine();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = $"DROP USER '{usuario}'@'localhost';";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Usuario eliminado con éxito.");
            }
            Console.ReadKey();
        }

        private static void ListarUsuarios()
        {
            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = "SELECT user, host FROM mysql.user WHERE host = 'localhost';";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string user = reader["user"].ToString();
                    string host = reader["host"].ToString();

                    Console.WriteLine($"Usuario: {user}, Host: {host}");
                }

                reader.Close();
            }
            Console.ReadKey();
        }

        private static void CrearRol()
        {
            Console.Write("Introduce el nombre del nuevo rol: ");
            string nuevoRol = Console.ReadLine();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = $"CREATE ROLE '{nuevoRol}'@'%';";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Rol creado con éxito.");
                Console.ReadKey();
            }
        }

        private static void AsignarRolAUsuario()
        {
            Console.Write("Introduce el nombre del rol a asignar: ");
            string rol = Console.ReadLine();

            Console.Write("Introduce el nombre del usuario: ");
            string usuario = Console.ReadLine();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();

                // Asignar el rol al usuario
                string grantQuery = $"GRANT {rol} TO '{usuario}'@'localhost';";
                MySqlCommand grantCmd = new MySqlCommand(grantQuery, con);

                try
                {
                    grantCmd.ExecuteNonQuery();
                    Console.WriteLine("Rol asignado al usuario con éxito.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al asignar rol: {ex.Message}");
                }

                Console.ReadKey();
            }
        }



        private static void ListarRoles()
        {
            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = "SELECT user, host FROM mysql.user WHERE host = '%';";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string roles = reader["user"].ToString();
                    string host = reader["host"].ToString();

                    Console.WriteLine($"Rol: {roles}, Host: {host}");
                }
                reader.Close();
               
            }
            Console.ReadKey();
        }

        private static void EliminarRol()
        {
            Console.Write("Introduce el nombre del rol a eliminar: ");
            string usuario = Console.ReadLine();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = $"DROP ROLE '{usuario}'@'%';";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Rol eliminado con éxito.");
                Console.ReadKey();
            }

        }


        private static void RespaldarBaseDatos()
        {
            List<string> basesDeDatos = ObtenerBasesDeDatos();

            if (basesDeDatos.Count == 0)
            {
                Console.WriteLine("No se encontraron bases de datos disponibles.");
                return;
            }

            Console.WriteLine("Bases de datos disponibles:");
            for (int i = 0; i < basesDeDatos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {basesDeDatos[i]}");
            }

            Console.Write("Selecciona el número de la base de datos que deseas respaldar: ");
            if (!int.TryParse(Console.ReadLine(), out int seleccion) || seleccion < 1 || seleccion > basesDeDatos.Count)
            {
                Console.WriteLine("Selección no válida.");
                return;
            }

            string nombreBaseDatos = basesDeDatos[seleccion - 1];
            Console.Write("Introduce el nombre del archivo de respaldo (sin extensión): ");
            string nombreArchivo = Console.ReadLine();

            string rutaRespaldo = @"C:\Users\Home\Documents\Respaldos_Base_de_Datos";
            if (!Directory.Exists(rutaRespaldo))
            {
                Directory.CreateDirectory(rutaRespaldo);
            }

            string archivoRespaldo = Path.Combine(rutaRespaldo, $"{nombreArchivo}.sql");

            string rutaMysqlBin = @"C:\Program Files\MySQL\MySQL Server 8.0\bin";

            // Comando para cambiar al directorio donde se encuentra mysqldump y luego realizar el respaldo
            string comando = $"cd /d \"{rutaMysqlBin}\" && mysqldump -u root -p123456 --databases {nombreBaseDatos} > \"{archivoRespaldo}\"";

            var processInfo = new ProcessStartInfo("cmd.exe", $"/c {comando}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(processInfo))
            {
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"Error al respaldar la base de datos: {error}");
                }
                else
                {
                    Console.WriteLine($"Respaldo de la base de datos '{nombreBaseDatos}' completado.");
                    Console.WriteLine($"Respaldo en '{rutaRespaldo}' ");
                }
            }

            Console.ReadKey();
        }





        private static List<string> ObtenerBasesDeDatos()
        {
            List<string> basesDeDatos = new List<string>();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = "SHOW DATABASES;";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string dbName = reader["Database"].ToString();
                    if (dbName != "information_schema" && dbName != "mysql" && dbName != "performance_schema" && dbName != "sys")
                    {
                        basesDeDatos.Add(dbName);
                    }
                }

                reader.Close();
            }

            return basesDeDatos;
        }


        private static void RestaurarBaseDatos()
        {
            List<string> archivosRespaldos = Directory.GetFiles(@"C:\Users\Home\Documents\Respaldos_Base_de_Datos", "*.sql").ToList();

            if (archivosRespaldos.Count == 0)
            {
                Console.WriteLine("No se encontraron archivos de respaldo disponibles.");
                return;
            }

            Console.WriteLine("Archivos de respaldo disponibles:");
            for (int i = 0; i < archivosRespaldos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(archivosRespaldos[i])}");
            }

            Console.Write("Selecciona el número del archivo de respaldo que deseas restaurar: ");
            if (!int.TryParse(Console.ReadLine(), out int seleccion) || seleccion < 1 || seleccion > archivosRespaldos.Count)
            {
                Console.WriteLine("Selección no válida.");
                return;
            }

            string archivoRespaldo = archivosRespaldos[seleccion - 1];

            string rutaMysqlBin = @"C:\Program Files\MySQL\MySQL Server 8.0\bin";


            string comando = $"mysql -u root -p123456 < \"{archivoRespaldo}\"";

            var processInfo = new ProcessStartInfo("cmd.exe", $"/c \"{comando}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = rutaMysqlBin
            };

            using (var process = Process.Start(processInfo))
            {
                process.WaitForExit();
            }

            Console.WriteLine($"Restauración de la base de datos completada.");
            Console.ReadKey();
        }

        private static void ListarEntidades()
        {
            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = "SHOW TABLES;";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString());
                }
                reader.Close();
            }
            Console.ReadKey();
        }


        private static void ListarAtributosPorEntidades()
        {
            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();

                // Obtener todas las entidades (tablas) de la base de datos
                string queryTablas = "SHOW TABLES;";
                MySqlCommand cmdTablas = new MySqlCommand(queryTablas, con);
                MySqlDataReader readerTablas = cmdTablas.ExecuteReader();

                List<string> entidades = new List<string>();
                while (readerTablas.Read())
                {
                    string entidad = readerTablas[0].ToString();
                    entidades.Add(entidad);
                    Console.WriteLine(entidad);
                }
                readerTablas.Close();

                // Solicitar al usuario que seleccione una entidad
                Console.Write("Introduce el nombre de la entidad: ");
                string entidadSeleccionada = Console.ReadLine();

                if (entidades.Contains(entidadSeleccionada))
                {
                    // Listar los atributos de la entidad seleccionada
                    string queryAtributos = $"DESCRIBE {entidadSeleccionada};";
                    MySqlCommand cmdAtributos = new MySqlCommand(queryAtributos, con);
                    MySqlDataReader readerAtributos = cmdAtributos.ExecuteReader();

                    Console.WriteLine($"---- Atributos de la entidad {entidadSeleccionada} ----");
                    while (readerAtributos.Read())
                    {
                        Console.WriteLine($"{readerAtributos["Field"]} - {readerAtributos["Type"]}");
                    }
                    readerAtributos.Close();
                }
                else
                {
                    Console.WriteLine("La entidad ingresada no es válida.");
                }
            }
            Console.ReadKey();
        }


        private static List<string> consultaEntidades = new List<string>();

        private static void AgregarEntidadesAConsulta()
        {
            Console.Write("Introduce el nombre de la nueva entidad (tabla): ");
            string entidad = Console.ReadLine();

            List<string> atributos = new List<string>();
            string continuar;

            do
            {
                Console.Write("Introduce el nombre del atributo: ");
                string atributoNombre = Console.ReadLine();

                Console.Write("Introduce el tipo de dato del atributo (por ejemplo, INT, VARCHAR(100), DATE): ");
                string atributoTipo = Console.ReadLine();

                atributos.Add($"{atributoNombre} {atributoTipo}");

                Console.Write("¿Quieres agregar otro atributo? (s/n): ");
                continuar = Console.ReadLine();
            }
            while (continuar.Equals("s", StringComparison.OrdinalIgnoreCase));

            // Componer la cadena de creación de tabla
            string query = $"CREATE TABLE {entidad} ({string.Join(", ", atributos)});";

            // Ejecutar la consulta para crear la tabla
            using (MySqlConnection con = ObtenerConexion())
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine($"Tabla '{entidad}' creada con éxito.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al crear la tabla: {ex.Message}");
                }
            }

            Console.ReadKey();
        }


        private static void GenerarReportePDF()
        {
            try
            {
                // Mostrar lista de entidades
                List<string> entidades = ObtenerEntidades();
                if (entidades.Count == 0)
                {
                    Console.WriteLine("No se encontraron entidades disponibles.");
                    return;
                }

                Console.WriteLine("Entidades disponibles:");
                for (int i = 0; i < entidades.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {entidades[i]}");
                }

                Console.Write("Selecciona el número de la entidad que deseas incluir en el reporte: ");
                if (!int.TryParse(Console.ReadLine(), out int seleccion) || seleccion < 1 || seleccion > entidades.Count)
                {
                    Console.WriteLine("Selección no válida.");
                    return;
                }

                string entidadSeleccionada = entidades[seleccion - 1];
                List<Atributo> atributos = ObtenerAtributos(entidadSeleccionada);
                if (atributos.Count == 0)
                {
                    Console.WriteLine($"No se encontraron atributos para la entidad '{entidadSeleccionada}'.");
                    return;
                }

                // Mostrar atributos disponibles
                Console.WriteLine("Atributos disponibles:");
                for (int i = 0; i < atributos.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {atributos[i].Nombre}");
                }

                Console.WriteLine("Selecciona los números de los atributos que deseas incluir en el reporte, separados por comas:");
                string seleccionAtributos = Console.ReadLine();
                List<int> indicesAtributos;
                try
                {
                    indicesAtributos = seleccionAtributos.Split(',').Select(s => int.Parse(s.Trim()) - 1).ToList();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Formato de selección no válido.");
                    return;
                }

                // Validar índices seleccionados
                if (indicesAtributos.Any(i => i < 0 || i >= atributos.Count))
                {
                    Console.WriteLine("Una o más selecciones no son válidas.");
                    return;
                }

                List<Atributo> atributosSeleccionados = indicesAtributos.Select(i => atributos[i]).ToList();

                // Obtener los datos de la entidad seleccionada
                List<Dictionary<string, string>> datosTabla = ObtenerDatosDeEntidad(entidadSeleccionada, atributosSeleccionados);
                if (datosTabla.Count == 0)
                {
                    Console.WriteLine($"No se encontraron datos para la entidad '{entidadSeleccionada}'.");
                    return;
                }

                // Generar el PDF con la respectiva ruta
                string rutaDirectorio = @"C:\Users\Home\Documents\Reportes_PDF";
                if (!Directory.Exists(rutaDirectorio))
                {
                    Directory.CreateDirectory(rutaDirectorio);
                }
                string rutaArchivoPDF = Path.Combine(rutaDirectorio, $"Reporte_{entidadSeleccionada}.pdf");

                using (FileStream fs = new FileStream(rutaArchivoPDF, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document doc = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();
                    doc.Add(new Paragraph($"Reporte de la entidad: {entidadSeleccionada}", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
                    doc.Add(new Paragraph("\n"));
                    PdfPTable table = new PdfPTable(atributosSeleccionados.Count)
                    {
                        WidthPercentage = 100
                    };

                    // Encabezados de la tabla (nombres de las columnas)
                    foreach (var atributo in atributosSeleccionados)
                    {
                        table.AddCell(new PdfPCell(new Phrase(atributo.Nombre, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));
                    }

                    // Agregar los datos de la tabla
                    foreach (var fila in datosTabla)
                    {
                        foreach (var atributo in atributosSeleccionados)
                        {
                            table.AddCell(new PdfPCell(new Phrase(fila[atributo.Nombre])));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    writer.Close();
                }

                Console.WriteLine($"Reporte generado con éxito: {rutaArchivoPDF}");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el reporte: {ex.Message}");
                Console.ReadKey();
            }

            Console.ReadKey();
        }

        private static List<string> ObtenerEntidades()
        {
            List<string> entidades = new List<string>();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = "SHOW TABLES;";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    entidades.Add(reader[0].ToString());
                }

                reader.Close();
            }

            return entidades;
        }

        private static List<Atributo> ObtenerAtributos(string entidad)
        {
            List<Atributo> atributos = new List<Atributo>();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string query = $"DESCRIBE {entidad};";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    atributos.Add(new Atributo
                    {
                        Nombre = reader["Field"].ToString(),
                        Tipo = reader["Type"].ToString()
                    });
                }

                reader.Close();
            }

            return atributos;
        }

        private static List<Dictionary<string, string>> ObtenerDatosDeEntidad(string entidad, List<Atributo> atributosSeleccionados)
        {
            List<Dictionary<string, string>> datos = new List<Dictionary<string, string>>();

            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string columnas = string.Join(", ", atributosSeleccionados.Select(a => a.Nombre));
                string query = $"SELECT {columnas} FROM {entidad};";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, string> fila = new Dictionary<string, string>();
                    foreach (var atributo in atributosSeleccionados)
                    {
                        fila[atributo.Nombre] = reader[atributo.Nombre].ToString();
                    }
                    datos.Add(fila);
                }

                reader.Close();
            }

            return datos;
        }

        private class Atributo
        {
            public string Nombre { get; set; }
            public string Tipo { get; set; }
        }


        public static void GenerarScriptCRUD()
        {
            using (MySqlConnection con = ObtenerConexion())
            {
                con.Open();
                string obtenerTablasQuery = "SHOW TABLES;";
                MySqlCommand obtenerTablasCmd = new MySqlCommand(obtenerTablasQuery, con);
                MySqlDataReader tablasReader = obtenerTablasCmd.ExecuteReader();
                List<string> tablas = new List<string>();

                while (tablasReader.Read())
                {
                    tablas.Add(tablasReader[0].ToString());
                }

                tablasReader.Close();

                foreach (string tabla in tablas)
                {
                    string insertProcedure = GenerarInsertProcedure(con, tabla);
                    EjecutarScript(con, insertProcedure);
                    string updateProcedure = GenerarUpdateProcedure(con, tabla);
                    EjecutarScript(con, updateProcedure);
                    string deleteProcedure = GenerarDeleteProcedure(con, tabla);
                    EjecutarScript(con, deleteProcedure);
                    string selectProcedure = GenerarSelectProcedure(tabla);
                    EjecutarScript(con, selectProcedure);

                }

            }
            Console.WriteLine("CRUD creado exito");
            Console.ReadKey();
        }

        private static string GenerarInsertProcedure(MySqlConnection con, string tabla)
        {
            StringBuilder insertProcedure = new StringBuilder();
            insertProcedure.AppendLine($"CREATE PROCEDURE Insertar_{tabla}(");
            List<string> atributos = new List<string>();
            string describeQuery = $"DESCRIBE {tabla};";
            MySqlCommand describeCmd = new MySqlCommand(describeQuery, con);
            MySqlDataReader describeReader = describeCmd.ExecuteReader();

            while (describeReader.Read())
            {
                string columna = describeReader["Field"].ToString();
                string tipoDato = describeReader["Type"].ToString();
                if (!columna.Equals("ID", StringComparison.OrdinalIgnoreCase) && !tipoDato.ToLower().Contains("auto_increment"))
                {
                    atributos.Add($"{columna} {tipoDato}");
                }
            }

            describeReader.Close();

            insertProcedure.AppendLine(string.Join(", ", atributos.Select(a => $"IN p_{a.Split(' ')[0]} {a.Split(' ')[1]}")));
            insertProcedure.AppendLine(")");
            insertProcedure.AppendLine("BEGIN");
            insertProcedure.AppendLine($"INSERT INTO {tabla}");
            insertProcedure.AppendLine($"({string.Join(", ", atributos.Select(a => a.Split(' ')[0]))})");
            insertProcedure.AppendLine($"VALUES");
            insertProcedure.AppendLine($"({string.Join(", ", atributos.Select(a => $"p_{a.Split(' ')[0]}"))});");
            insertProcedure.AppendLine("END;");

            return insertProcedure.ToString();
        }

        private static string GenerarUpdateProcedure(MySqlConnection con, string tabla)
        {
            StringBuilder updateProcedure = new StringBuilder();
            updateProcedure.AppendLine($"CREATE PROCEDURE Actualizar_{tabla}(");

            List<string> atributos = new List<string>();
            string clavePrimaria = string.Empty;

            string describeQuery = $"DESCRIBE {tabla};";
            MySqlCommand describeCmd = new MySqlCommand(describeQuery, con);
            MySqlDataReader describeReader = describeCmd.ExecuteReader();

            while (describeReader.Read())
            {
                string columna = describeReader["Field"].ToString();
                string tipoDato = describeReader["Type"].ToString();
                string key = describeReader["Key"].ToString();

                if (key == "PRI")
                {
                    clavePrimaria = columna;
                }
                atributos.Add($"{columna} {tipoDato}");
            }

            describeReader.Close();

           
            updateProcedure.AppendLine(string.Join(", ", atributos.Select(a => $"IN p_{a.Split(' ')[0]} {a.Split(' ')[1]}")));
            updateProcedure.AppendLine(")");
            updateProcedure.AppendLine("BEGIN");
            updateProcedure.AppendLine($"Update {tabla}");
            updateProcedure.AppendLine($"SET {string.Join(", ", atributos.Select(a => $"{a.Split(' ')[0]} = p_{a.Split(' ')[0]}"))}");
            updateProcedure.AppendLine($"WHERE {clavePrimaria} = p_{clavePrimaria};");
            updateProcedure.AppendLine("END;");

            return updateProcedure.ToString();
        }

        private static string GenerarDeleteProcedure(MySqlConnection con, string tabla)
        {
            StringBuilder deleteProcedure = new StringBuilder();
            deleteProcedure.AppendLine($"CREATE PROCEDURE Eliminar_{tabla}(");
            string clavePrimaria = string.Empty;
            string describeQuery = $"DESCRIBE {tabla};";
            MySqlCommand describeCmd = new MySqlCommand(describeQuery, con);
            MySqlDataReader describeReader = describeCmd.ExecuteReader();
            while (describeReader.Read())
            {
                string columna = describeReader["Field"].ToString();
                string key = describeReader["Key"].ToString();

                if (key == "PRI")
                {
                    clavePrimaria = columna;
                    break;
                }
            }
            describeReader.Close();
            deleteProcedure.AppendLine($"IN p_{clavePrimaria} INT");
            deleteProcedure.AppendLine(")");
            deleteProcedure.AppendLine("BEGIN");
            deleteProcedure.AppendLine($"DELETE FROM {tabla}");
            deleteProcedure.AppendLine($"WHERE {clavePrimaria} = p_{clavePrimaria};");
            deleteProcedure.AppendLine("END;");
            return deleteProcedure.ToString();
        }

        private static string GenerarSelectProcedure(string tabla)
        {
            string selectProcedure = $@"
CREATE PROCEDURE Seleccionar_{tabla}()
BEGIN
    SELECT *
    FROM {tabla};
END;";

            return selectProcedure;
        }

        private static void EjecutarScript(MySqlConnection con, string script)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                MySqlCommand cmd = new MySqlCommand(script, con);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar el script SQL: {ex.Message}");

            }
        }



    
    }
}
