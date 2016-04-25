using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;


    /// <summary>
    /// Реализует рукописную букву,
    /// содержащую все параметры отображения.
    /// </summary>
    public class LetterConfig
    {

        private char value;
        private int variable;
        private int leftDX, rightDX, dY;
        private Size size;

        /// <summary>
        /// Конструктор класса Letter
        /// </summary>
        /// <param name="c">символ</param>
        /// <param name="variable">номер символа по порядку</param>
        /// <param name="ldx">смещение слева</param>
        /// <param name="rdx">смещение справа</param>
        /// <param name="dy">смещение вверх</param>
        /// <param name="size">графический размер буквы</param>
        public LetterConfig(char c, int variable, int ldx, int rdx, int dy, Size size)
        {
            value = c;
            this.variable = variable;
            leftDX = ldx;
            rightDX = rdx;
            dY = dy;
            this.size = size;

        }

        /// <summary>
        /// Сравнивает значение символа
        ///и номер по порядку
        /// </summary>
        /// <param name="c">символ</param>
        /// <param name="i">номер по порядку</param>
        /// <returns></returns>
        public bool Equivalent(char c, int i)
        {
            return value == c && variable == i;
        }

        /// <summary>
        /// Символ буквы
        /// </summary>
        public char Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Номер символа
        /// </summary>
        public int Variable
        {
            get { return variable; }
            set { variable = value; }
        }

        /// <summary>
        /// Смещение буквы справа
        /// </summary>
        public int RightDx
        {
            get { return rightDX; }
            set { rightDX = value; }
        }

        /// <summary>
        /// Смещение буквы слева
        /// </summary>
        public int LeftDx
        {
            get { return leftDX; }
            set { leftDX = value; }
        }

        /// <summary>
        /// смещение буквы вверх
        /// </summary>
        public int DY
        {
            get { return dY; }
            set { dY = value; }
        }

        /// <summary>
        /// Графический размер буквы
        /// </summary>
        public Size Size
        {
            get { return size; }
            set { size = value; }
        }



        public override string ToString()
        {
            //о: 0, -3, -1, 0;
            return value + ": " + variable + ", " + LeftDx + ", " + RightDx + ", " + DY + ";";
        }
    }

    /// <summary>
    /// Управляющий класс, содержащий все настройки символов
    /// </summary>
    public class HandWriteConfig
    {
        /// <summary>
        /// список конфигурации графических букв.
        /// заполняется в конструкторе и дальнейшее
        /// генерирование проиходит из памяти, что
        /// обеспечивает более быструю работу.
        /// </summary>
        public static List<LetterConfig> Letters = new List<LetterConfig>();

        /// <summary>
        /// Чтение конфигурации символов
        /// </summary>
        /// <param name="sessionId"></param>
        public void ReadConfig(int sessionId)
        {
            // очищаем массив настроек символов
            Letters.Clear();
            // получаем настройки прописных букв
            string[] strs =
                File.ReadAllLines(DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name +
                                  @"\Data\lower.cfg");
            // заходим в форич этой штуки
            foreach (string str in strs)
            {
                // от сглаза
                try
                {
                    // их парсим.
                    string[] buf = str.Replace(" ", String.Empty)
                        .Split(new[] {';', ',', ':'}, StringSplitOptions.RemoveEmptyEntries);
                    // строим путь к файлу
                    string path = DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Let\" +
                                  buf[0] + buf[1] + ".png";
                    // открываем битмап
                    Bitmap bitmap = new Bitmap(path);
                    // и добавляем букву в список
                    Letters.Add(new LetterConfig(buf[0][0], Convert.ToInt32(buf[1]), Convert.ToInt32(buf[2]),
                        Convert.ToInt32(buf[3]), Convert.ToInt32(buf[4]), new Size(bitmap.Width, bitmap.Height)));
                }
                catch
                {
                }
            }
            // теперь проделываем тоже самое с верхним регистром
            strs =
                File.ReadAllLines(DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name +
                                  @"\Data\upper.cfg");
            // тоже цикл
            foreach (string str in strs)
            {
                // от сглаза
                try
                {
                    // парсим
                    string[] buf = str.Replace(" ", String.Empty)
                        .Split(new[] {';', ',', ':'}, StringSplitOptions.RemoveEmptyEntries);
                    // путь к файлу
                    string path = DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Let\" +
                                  buf[0] + buf[1] + ".png";
                    // битмап
                    Bitmap bitmap = new Bitmap(path);
                    // добавляем в список
                    Letters.Add(new LetterConfig(buf[0][0], Convert.ToInt32(buf[1]), Convert.ToInt32(buf[2]),
                        Convert.ToInt32(buf[3]), Convert.ToInt32(buf[4]), new Size(bitmap.Width, bitmap.Height)));
                }
                catch
                {
                }
            }

        }

        /// <summary>
        /// Добавление конфигурации
        /// </summary>
        /// <param name="letter"></param>
        public void AddConfig(LetterConfig letter)
        {
            // ну тут просто проверяем есть ли в списке этот элемент
            int idx = GetLetterIndex(letter.Value, letter.Variable);
            // если нет - добавляем
            if (idx < 0)
                Letters.Add(letter);
            // по-другому - присваиваем новые значения
            else
            {
                Letters[idx].DY = letter.DY;
                Letters[idx].LeftDx = letter.LeftDx;
                Letters[idx].RightDx = letter.RightDx;
                Letters[idx].Size = letter.Size;
            }

        }

        /// <summary>
        /// Запись конфигурации
        /// </summary>
        /// <param name="sessionId"></param>
        public void WriteConfig(int sessionId)
        {
            // тут создаем два списка символов
            List<string> upperStr = new List<string>();
            List<string> lowerStr = new List<string>();


            // по циклу записываем в них значения
            foreach (var letter in Letters)
            {
                if (Char.IsUpper(letter.Value)) upperStr.Add(letter.ToString());
                else lowerStr.Add(letter.ToString());
            }
            // и после пишем в файл
            File.WriteAllLines(
                DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Data\lower.cfg", lowerStr);
            File.WriteAllLines(
                DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Data\upper.cfg", upperStr);
        }

        /// <summary>
        /// Возвращает объект Letter по символу и нидексу
        /// </summary>
        /// <param name="c">символ</param>
        /// <param name="i">индекс</param>
        /// <returns></returns>
        public LetterConfig GetLetter(char c, int i)
        {
            return Letters.FirstOrDefault(letter => letter.Equivalent(c, i));
        }

        /// <summary>
        /// Хрен его знает
        /// </summary>
        /// <param name="c"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int GetLetterIndex(char c, int i)
        {
            if (Letters.Any(t => t.Equivalent(c, i)))
                return i;
            return -1;
        }
    }

    /// <summary>
    /// Класс символа, содержащий все параметры отображения
    /// </summary>
    public class Letter
    {
        public static Letter Empty = new Letter();
        private int x, px;
        private int y, py;
        private int width;
        private int height;
        private int index;
        private char symbol;
        private Bitmap image;
        private double pWidth = 1, pHeight = 1;
        private string path = "";

        /// <summary>
        /// Изображение символа
        /// </summary>
        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// позиция по Х
        /// </summary>
        public int X
        {
            get { return x + px; }
            set { px = value; }
        }

        /// <summary>
        /// позиция по У
        /// </summary>
        public int Y
        {
            get { return y + py; }
            set { py = value; }
        }

        /// <summary>
        /// Ширина символа
        /// </summary>
        public int Width
        {
            get { return Convert.ToInt32(Convert.ToDouble(width)*pWidth); }
            set { if (value > 0) pWidth = Convert.ToDouble(value)/100; }
        }

        /// <summary>
        /// Высота символа
        /// </summary>
        public int Height
        {
            get { return Convert.ToInt32(Convert.ToDouble(height)*pHeight); }
            set { if (value > 0) pHeight = Convert.ToDouble(value)/100; }
        }
        
        /// <summary>
        /// Индекс символа
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// Значение символа
        /// </summary>
        public char Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        private Letter()
        {
            x = 0;
            y = 0;
            width = 0;
            height = 0;
            index = 0;
            symbol = ' ';
            image = null;
        }

        /// <summary>
        /// Контруктор с параметрами
        /// </summary>
        /// <param name="str">путь</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="index"></param>
        /// <param name="symbol"></param>
        public Letter(string str, int x, int y, int index, char symbol)
        {
            // просто писваиваем значения
            this.x = x;
            this.y = y + HandRandom.GetDeltaMoveY();
            path = str;
            this.index = index;
            this.symbol = symbol;
            double scaleX = HandRandom.GetLetterScaleX();
            double scaleY = HandRandom.GetLetterScaleY();
            try
            {
                // получаем битмап
                // и иним его
                image = new Bitmap(str);
                this.width = image.Width;
                this.height = image.Height;
                image = new Bitmap(image, Convert.ToInt32(width*scaleX), Convert.ToInt32(height*scaleY));
                image.SetResolution(300, 300);
            }
            catch
            {
                return;
            }



        }

        /// <summary>
        /// Присваивание изображению новое значение
        /// </summary>
        public void UpdateImage()
        {
            try
            {
                image = new Bitmap(new Bitmap(path), Width, Height);
                image.SetResolution(300, 300);
            }
            catch (Exception)
            {


            }

        }

        /// <summary>
        /// Прорисовка символа
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            // если есть что рисовать
            if (image != null)
            {
                //  от сглаза
                try
                {
                    // рисуем изображение
                    image = new Bitmap(image, Width, Height);
                    image.SetResolution(300, 300);
                    g.DrawImage(image, new PointF(X, Y));
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// В строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return symbol + "^" + index + "^" + X + "^" + Y + "^" + Width + "^" + Height;
        }

        /// <summary>
        /// Есть ли изображение
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return image == null;
        }
    }

    /// <summary>
    /// Управляющий класс графического отображения почерка
    /// </summary>
    public class HandWrite
    {
        private List<Letter> letters = new List<Letter>();
        private int selectedLetter;

        /// <summary>
        /// ВЫбранный символ
        /// </summary>
        public Letter SelectedLetter
        {
            get
            {
                if (selectedLetter >= 0 && selectedLetter < letters.Count) return letters[selectedLetter];
                return null;
            }
        }

        /// <summary>
        /// Список символов
        /// </summary>
        public List<Letter> Letters
        {
            get { return letters; }
            set { letters = value; }
        }

        /// <summary>
        /// Возвращает строку из символов
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // через цикл загоняем все символы в строчку и возвращаем ее
            string str = "";
            for (int i = 0; i < letters.Count; i++) str += letters[i].ToString() + "\n";
            return str;
        }   

        /// <summary>
        /// Перерисовка каждого символа
        /// </summary>
        public void Update()
        {
            // Апдейтим каждый символ
            for (int i = 0; i < letters.Count; i++)
                letters[i].UpdateImage();
        }

        /// <summary>
        /// Прорисовка текста
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            // очищаем поле и рисуем по новой каждый символ
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            for (int i = 0; i < letters.Count; i++)
                if (!letters[i].IsEmpty())
                    letters[i].Draw(g);
        }

        /// <summary>
        /// Выбрать букву по координатам
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pw"></param>
        /// <param name="ph"></param>
        public void SelectLetter(int x, int y, int pw, int ph)
        {
            // по координатам вычисляем какую букву выбрать
            // это с учетом масштаба
            x = Convert.ToInt32(Convert.ToDouble(Hand.Width)/Convert.ToDouble(pw)*Convert.ToDouble(x));
            y = Convert.ToInt32(Convert.ToDouble(Hand.Height)/Convert.ToDouble(ph)*Convert.ToDouble(y));
            // и по циклу проверяем чье оно
            for (int i = 0; i < letters.Count; i++)
                if (
                    new Rectangle(letters[i].X, letters[i].Y, letters[i].Width, letters[i].Height).Contains(new Point(
                        x, y)))
                    // присваиваем, если нашли
                    selectedLetter = i;

        }
    }

