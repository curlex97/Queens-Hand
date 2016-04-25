using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;



    /// <summary>
    /// Summary description for Data
    /// </summary>
    public class User
    {
        private uint userID;
        private string name;
        private string password;
        private Hand handWrite;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Hand HandWrite
        {
            get { return handWrite; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public uint UserID
        {
            get { return userID; }
        }

        public User(string name, string password, uint userId)
        {
            this.name = name;
            this.password = password;
            userID = userId;
            
        }

        public void Initialize(int sessionId)
        {
           handWrite = new Hand();
           handWrite.ReadConfig(sessionId);
        }
    }

    /// <summary>
    /// Класс взаимодействия с данными сервера
    /// </summary>
    public static class DataBase
    {
        /// <summary>
        /// Возвращает текущего пользователя
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static User CurrentUser(int idx)
        {
           if(idx >=0 && idx < Users.Count) return Users[idx];
            return null;
        }

      /// <summary>
      /// Список зарегистрированных пользователей
      /// </summary>
        public static List<User> Users = new List<User>();

        /// <summary>
        /// Путь к папке сервера на диске
        /// </summary>
        private static string serverPath = "";

        /// <summary>
        /// Путь к папке сервера на диске
        /// </summary>
        public static string ServerPath
        {
            get { return serverPath; }
            set
            {
                serverPath = value;
                ConnectionString = File.ReadAllText(ServerPath + @"\Content\Config\sqlconnection.cfg"); 
            }
        }
        
        /// <summary>
        /// Строка соединения с базой
        /// </summary>
        public static string ConnectionString = "";
        
        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="pass">пароль</param>
        /// <param name="repeatPass">повторить пароль</param>
        /// <returns></returns>
        public static int SignUp(string name, string pass, string repeatPass)
        {
            // все тримим и проверяем на валидность   
            if (pass.Trim().Length < 6 || pass.Trim().Length > 63) return -1;
            if (name.Trim().Length < 6 || name.Trim().Length > 63) return -1;

            // не должно содержать этого гавна
            string specials = "!@#$%^&*()<>,.'=+;%:?*[]{}";
            if (name.Any(c => specials.Contains(c))) return -1;
            if (pass.Any(c => specials.Contains(c))) return -1;

            // и если пароли совпадают
            if (pass == repeatPass)
            {
                // хэшим пароль 
                string hash = GetMd5Hash(new MD5Cng(), pass);
                // зафигачиваем пользователя в базу
                RunCommand("insert into Users values('" + name + "', '" + hash + "')");
                // создаем для него папочку на сервере
                CreateUserFileSystem(name);
                // обновляем список пользователей
                CreateUserList();
                // заходим в систему
                Login(name, pass);
                // возвращаем его индекс
                return Users.Count-1;
            }

            return -1;
        }

        /// <summary>
        /// Изменение данных пользователя в базе
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <param name="confirmPass"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static bool UpdateUser(string name, string pass, string confirmPass, int sessionId)
        {
            // опять же все проврить
            if (pass.Trim().Length < 6 || pass.Trim().Length > 63) return false;
            if (name.Trim().Length < 6 || name.Trim().Length > 63) return false;
            // на наличие вот такого
            string specials = "!@#$%^&*()<>,.'=+;%:?*[]{}";
            if (name.Any(c => specials.Contains(c))) return false;
            if (pass.Any(c => specials.Contains(c))) return false;

            // и если все гуд с паролями
            if (pass == confirmPass)
            {
                // хэшим пароль
                string hash = GetMd5Hash(new MD5Cng(), pass);
                // апдейтим юзера
                RunCommand("update Users set Name='" + name + "', Password='" + hash + "' where Name='" +
                           CurrentUser(sessionId).Name + "'");
                // апдейтим лист
                CreateUserList();
                // заходим в систему
                Login(name, pass);
                // окай!
                return true;
            }
            // иначе не окай
            return false;
        }

        /// <summary>
        /// Создание папки юзеру
        /// </summary>
        /// <param name="userName"></param>
        public static void CreateUserFileSystem(string userName)
        {
            // Если не знаем папку к серверу, то вылет
            if (ServerPath.Length == 0) return;
            // проверяем есть ли
            DirectoryInfo directory = new DirectoryInfo(ServerPath + @"\Content\Users\" + userName);
            if (!directory.Exists)
            {
                // если нет - создаем
                directory.Create();
                // создаем также папки для букв
                new DirectoryInfo(directory.FullName + @"\Let").Create();
                // настроек
                new DirectoryInfo(directory.FullName + @"\Data").Create();
                // скомпиленных листов
                new DirectoryInfo(directory.FullName + @"\Compile").Create();
                // форм почерка
                new DirectoryInfo(directory.FullName + @"\Font").Create();
                // а также закидываем туда файл замены символов
                new FileInfo(ServerPath + @"\Content\Samples\Config\assos.cfg").CopyTo(directory.FullName +
                                                                                       @"\Data\assos.cfg");
                // файл настройки прописных букв
                new FileInfo(ServerPath + @"\Content\Samples\Config\lower.cfg").CopyTo(directory.FullName +
                                                                                       @"\Data\lower.cfg");
                // файл настройки заглавных букв
                new FileInfo(ServerPath + @"\Content\Samples\Config\upper.cfg").CopyTo(directory.FullName +
                                                                                       @"\Data\upper.cfg");
                // и файл общей конфигурации
                new FileInfo(ServerPath + @"\Content\Samples\Config\config.ini").CopyTo(directory.FullName +
                                                                                        @"\Data\config.ini");
            }
        }

        /// <summary>
        /// Хеширование строки
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static int Login(string name, string pass)
        {
            // если нет пользователей - добавить
            if (Users.Count == 0) CreateUserList();
            // хеш пароля
            pass = GetMd5Hash(new MD5Cng(), pass);
            // и по юзерам
            for(int i=0; i<Users.Count; i++)
            {
                // если все гуд
                if (Users[i].Name == name && Users[i].Password == pass)
                {
                    // проверяем есть ли у него папочка
                    CreateUserFileSystem(name);
                    // возвращаем индекс юзера
                    return i;
                }
            }
            // иначе -1
            return -1;
        }

       

        /// <summary>
        /// исполняет команду в базе данных и возвращает массив полученных строк.
        /// </summary>
        /// <param name="comm"></param>
        /// <returns></returns>
        public static string[] RunCommand(string comm)
        {
            List<string> list = new List<string>();
            SqlConnection connection =
                new SqlConnection(
                    ConnectionString);
            SqlCommand command = new SqlCommand(comm, connection);
            //    command.Parameters.AddWithValue("@pricePoint", paramValue);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++) list.Add(reader[i].ToString());
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine(ex.Message);
            }
            string[] rets = new string[list.Count];
            for (int i = 0; i < list.Count; i++) rets[i] = list[i];

            return rets;
        }

        /// <summary>
        /// Создает список юзеров
        /// </summary>
        public static void CreateUserList()
        {
            // чистим
            Users.Clear();
            // читаем из базы
            string[] args = RunCommand("select * from Users");
            // и парсим
            for (int i = 0; i < args.Length; i += 3)
            {
                // добавляем
                Users.Add(new User(args[i + 1], args[i + 2], Convert.ToUInt32(args[i])));
                Users[Users.Count-1].Initialize(Users.Count-1);
            }

        }

       
    }

    /// <summary>
    /// Класс построения почерка
    /// </summary>
    public static class WebPrettyGirl
    {
        /// <summary>
        /// Строить почерк
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="sessionId">идентификатор сессии</param>
        /// <returns></returns>
        public static string Build(string text, int sessionId)
        {
            // получаем куда компилить изображение
            string path = DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name +
                          @"\Compile\LastCompile.png";
            // Hand.Width = 315;
            // Hand.Height = 446;
            // и компилим его
            // сначала билд
            DataBase.CurrentUser(sessionId).HandWrite.BuildAndWrite(sessionId, text);
            // потом сохранение
            DataBase.CurrentUser(sessionId).HandWrite.LastCompile.Save(path, ImageFormat.Png);
            return path;
        }

        /// <summary>
        /// Компиляция скрипта
        /// </summary>
        /// <param name="sessionId">идентификатор сессии</param>
        /// <param name="script">текст скрипта</param>
        public static void BuildScript(int sessionId, string script)
        {
            // вызов билдера скрипта
            DataBase.CurrentUser(sessionId).HandWrite.BulidScript(script);
        }



    }

