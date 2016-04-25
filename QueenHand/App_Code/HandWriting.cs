using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;



    /// <summary>
    /// Управляющий класс.
    /// Реализует генерирование графического 
    /// отображения букв, 
    /// преобразованных из текста.
    /// </summary>
    public class Hand
    {
        // объект скрипта
        QueenHandScript queenHandScript = new QueenHandScript();
        // 
        private HandWrite handWrite = new HandWrite();

        /// <summary>
        ///  вносит изменения в построение почерка по qh-скрипту.
        /// </summary>
        /// <param name="script"></param>
        public void BulidScript(string script)
        {
            queenHandScript.BuildScript(script);
        }


        public Bitmap LastCompile = null;

        /// <summary>
        /// Длина, ширина холста
        /// и нижний и верхний порог
        /// вариативности номера букв.
        /// </summary>
        public static int Width = 210*3, Height = 297*3, RMin = 0, RMax = 2;

        /// <summary>
        /// для рисования с помощью gdi
        /// </summary>
        private Graphics g;

        /// <summary>
        /// курсор по x
        /// </summary>
        private float x0 = HandRandom.BeginX;

        /// <summary>
        /// курсор по y
        /// </summary>
        private float y0 = HandRandom.BeginY;

        /// <summary>
        /// для отображения загрузки программы
        /// </summary>
        // ProgressBar progressBar = new ProgressBar();

        /// <summary>
        /// отображать ли "на ходу".
        /// </summary>
        public static bool LiveShow = false;

        private HandWriteConfig handWriteConfig = new HandWriteConfig();

        /// <summary>
        /// счетчик символов 
        /// во время генерирования
        /// </summary>
        public int cursor = 0;

        public float Y0
        {
            get { return y0; }
        }

        /// <summary>
        /// Контруктор
        /// </summary>
        /// <param name="pb">прогресс бар с формы</param>
     

        /// <summary>
        /// чтение конфигурации программы
        /// </summary>
        public void ReadConfig(int sessionId)
        {
            // строим словарь замен
            SymbolReplacer.BuildDictionary(sessionId);
            // далее читаем HandWriting'ом.
            handWriteConfig.ReadConfig(sessionId);
            //  handWriteConfig.WriteConfig();

            // от сглаза :)
            try
            {
                // теперь получаем все настройки шрифта, которые являются общими. У текущего пользователя.
               string[] strs =
                    File.ReadAllText(DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name +
                                     @"\Data\config.ini").Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

                // ну и если все гуд считалось, то заносим новые значения
                if (strs.Length >= 18)
                {

                    RMin = Convert.ToInt32(strs[0]);
                    RMax = Convert.ToInt32(strs[1]);
                    HandRandom.SetConfiguration(
                        Convert.ToInt32(strs[2]),
                        Convert.ToInt32(strs[3]),
                        Convert.ToInt32(strs[4]),
                        Convert.ToInt32(strs[5]),
                        Convert.ToInt32(strs[6]),
                        Convert.ToInt32(strs[7]),
                        Convert.ToInt32(strs[8]),
                        Convert.ToInt32(strs[9]),
                        Convert.ToInt32(strs[10]),
                        Convert.ToInt32(strs[11]),
                        Convert.ToInt32(strs[12]),
                        Convert.ToInt32(strs[13]),
                        Convert.ToInt32(strs[14]),
                        Convert.ToInt32(strs[15]),
                        Convert.ToInt32(strs[16]),
                        Convert.ToInt32(strs[17]));

                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// генерирование изображения почерка из текста
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="bitmap">готовый буфер. если нне null, 
        /// то текст допишется</param>
        /// <returns></returns>
        public Bitmap BuildAndWrite(int sessionId, string text)
        {
            // сначала полностью очищаем массив символов handWrite
            handWrite.Letters.Clear();
            // Далее получаем замененные символы в нашем тексте
            text = SymbolReplacer.ReplaceSymbols(text);
            // наш битмап, в который мы будем писать.
            Bitmap b1 = new Bitmap(Width, Height);
            // наши курсоры по x и y. Задаем им значение из настроек
            x0 = HandRandom.BeginX;
            y0 = HandRandom.BeginY;
            // счетчик символов.
            cursor = 0;
           
            // устанавливаем разрешение 300 dpi. Посчитал его оптимальным
            b1.SetResolution(300, 300);
            // Далее работаем через GDI+ с Graphics. Почему так убого и старо? Потому что быстро!
            g = Graphics.FromImage(b1);

            // bef - переменная, которая хранит в себе индекс символа, который прошел через TextToLetter.
            int bef = 0;
            // если символ пройден не был, был пропщуен, то в силу вступает past
            // будем считаться с тем символом, который был до него.
            int past = 1;

            // проходимся по каждому элементу массива текста
            for (int i = 0; i < text.Length; i++)
            {
                // буква, которая была до.
                string before = "";
                // если мы уже на первом элементе и дальше, то заносим туда предыдущий
                if (i > 0) before = "" + text[i - past] + bef;
                // сам символ
                char c = text[i];
                // получаем индекс обрабатываемого символа
                int rand = TextToLetter(sessionId, c, before);
                // в случае пропуска символа, исходя из правил скрипта, метод вернет -1
                // поэтому проверяем на -1, и если так - плюсуем past
                // иначе присваиваем индекс bef.
                if (rand >= 0) bef = rand;
                else past ++;

                // ну и если это был пробел, то
                if (c == ' ')
                {
                    // вручняк добавляем в список символов пробел.
                    handWrite.Letters.Add(Letter.Empty);
                    // получаем длину пробела
                    int dx = HandRandom.GetSpaceLength(cursor);
                    // прибаляем к нашему х-курсору пробел, деленный на два.
                    x0 += dx/2;
                    // слово закончилось, а значит обнуляем cursor
                    cursor = 0;

                    // получаем отступ от края.
                    int u = HandRandom.GetRightEdge();
                    // и если длина следующего слова больше, чем количество оставшегося места
                    if (x0 + WordSeparator.GetWordLength(text.Substring(i + 1).Split(new[] {' '})[0]) > Width - u)
                        // переносим слово
                       SeparateWord(sessionId, text, u, i);
                    

                }
                // если же это был переход на новую строку
                else if (c == '\n')
                {
                    // обнуляем курсор, слово закончилось
                    cursor = 0;
                    // ставим х на начало
                    x0 = HandRandom.BeginX;
                    // ставим у ниже по настройкам
                    y0 += HandRandom.GetLineSpace();
                }

                // а если у нас табуляция, то отступаем больше.
                else if (c == '\t')
                {
                    cursor = 0;
                    x0 += HandRandom.GetSpaceLength(3)*4;
                }


            }


            handWrite.Draw(g);
            LastCompile = b1;
            return b1;
        }

        /// <summary>
        /// возвращает смещение буквы
        /// справа и сверху и переносит крусор
        /// на смещение буквы слева
        /// </summary>
        /// <param name="c"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private PointF GetLetterParams(char c, int i)
        {
            // от сглаза
            try
            {
                // получаем объект настройки буквы.
                LetterConfig letter = handWriteConfig.GetLetter(c, i);
                // тут переносим курсор на длину, равную отступу слева 
                x0 += letter.LeftDx*float.Parse(HandRandom.GetMainScale().ToString());
                // и возвращаем отступ слева и сверху.
                return new PointF(float.Parse((letter.RightDx*HandRandom.GetMainScale()).ToString()),
                    y0 - float.Parse((letter.DY*HandRandom.GetMainScale()).ToString()));
            }
            catch (Exception)
            {
                // в случае ошибки возвращаем как есть.
                return new PointF(0, y0);
            }
        }

        /// <summary>
        /// рисует символ
        /// </summary>
        /// <param name="c">символ</param>
        /// <param name="random">номер по порядку</param>
        private int TextToLetter(int sessionId, char c, string before)
        {
            //  самый крутой метод :)
            // Здесь мы преобразуем символ в класс Letter списка из HandWriting.
            // А потом уже отображаем.

            // Сюда пишем параметры буквы.
            PointF point;

            //  список, который содержит все возможные значения переменной индекса.
            // исходя из скрипта, разумеется. Так-то можно все.
            List<string> idxes = new List<string>();
            // от минимального до максимального включительно
            for (int i = RMin; i < RMax + 1; i++)
            {
                // получаем правило для буквы
                var rule = queenHandScript.GetRule("" + c + i);
                // если возврат не равен нулу
                if (rule != null)
                {
                    // если мы проверили этот индекс, и он подходит, заносим его
                    if (rule.CheckRule(before, "")) idxes.Add(i.ToString());
                }
                // ну и если все разрешено, то мутим как есть
                else idxes.Add(i.ToString());
            }
            // если нельзя ничего рисовать
            if (idxes.Count == 0) 
                return -1;
            // ну и дальше случайно выбираем из того, что осталось
            int rand = Convert.ToInt32(idxes[HandRandom.Random.Next(0, idxes.Count)]);
            // теперь получаем правило уже для текущей буквы.
            var currule = queenHandScript.GetRule("" + c + rand);
            // если правил для него нет, берем из файла.
            if (currule == null)
                point = GetLetterParams(c, rand);
            // иначе 
            else
            {
                // переносим на отступ слева наш хю
                x0 += float.Parse(currule.LeftDeltaX.ToString());
                // и присваиваем поинту отступ справа и отступ сверху
                point = new PointF(float.Parse(currule.RightDeltaX.ToString()), float.Parse(currule.DeltaY.ToString()));
            }
            // если это ни пробел, ни спец. символ
            if (c != ' ' && c != '|')
            {
                // начинаем преобразовывать
                char p = c;
                if (p == '~') p = '-';
                // плюсуем курсор
                cursor++;
                // далее получаем имя файла с символом в зависимости от регистра.
                string str = DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Let\" + p;
                if (!Char.IsUpper(c)) str +=  rand +".png";
                else str += "!" + rand + ".png";

                try
                {
                    // если такой файл существует у пользователя в папке
                    if (new FileInfo(str).Exists)
                    {
                        // создаем объект Letter
                        Letter letter = new Letter(str, Convert.ToInt32(x0), Convert.ToInt32(point.Y), rand, c);
                        // зафигачиваем его в список
                        handWrite.Letters.Add(letter);
                        // делаем отступ, в зависимости от ширины слова. На глаз подобрал
                        x0 += letter.Width/1.27f + point.X;
                    }
                }
                catch
                {
                }
            }
            // возвращаем индекс символа
            return rand;

        }

        public HandWriteConfig HandWriteConfig
        {
            get { return handWriteConfig; }
        }

        public HandWrite HandWrite
        {
            get { return handWrite; }
        }

        /// <summary>
        /// рисует символ верхнего регистра
        /// </summary>
        /// <param name="c">символ</param>
        /// <param name="random">номер по порядку</param>

        /// <summary>
        /// разделяет и рисует слово по слогам
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="randSpace">свободное место до границы холста</param>
        /// <returns>возвращает длину символа</returns>
        public int SeparateWord(int sessionId, string text, int randSpace, int index)
        {
            // получаем весь оставшийся текст от момента последнего отображения
            text = text.Substring(index + 1);
            // заменяем все тирешки на палочки
            text = text.Replace("-", "|");
            // получаем некий ранд
            int rr = Convert.ToInt32(Width - x0 - randSpace - HandRandom.DefaultSpace);
            // и получаем текст с вставленным тире.
            string word = WordSeparator.GetSeparateWord(text, 0, rr);

            // две части слова
            string secondPath = "";
            string firstPath = "";
            // получаем список всех слов
            string[] strs2 = text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            // если он больше нуля, то
            if (strs2.Length > 0)
            {
                // если слово содержит наше тире
                if (word.Contains('~'))
                {
                    // парсим
                    string[] strs = word.Split(new[] {'~'}, StringSplitOptions.RemoveEmptyEntries);
                    // бьем на две части
                    firstPath = strs[0].Replace("|", "-") + "~";
                    secondPath = strs[1].Replace("|", "-");
                    // получаем длину первой части
                    int length = WordSeparator.GetWordLength(firstPath);
                    // если она не залезет
                    if (length + x0 > Width - randSpace)
                    {
                        x0 = HandRandom.BeginX;
                        y0 += HandRandom.GetLineSpace();
                        return 0;
                    }
                    // иначе делаем как в методе BuildAndWrite
                    int bef = 0;
                    int past = 1;
                    for (int i = 0; i < firstPath.Length; i++)
                    {
                        string before = "";
                        if (i > 0) before = firstPath[i - past] + "" + bef;
                        int y = TextToLetter(sessionId, firstPath[i], before);
                        if (y >= 0) bef = y;
                        else past++;
                    }




                }
            }
            // дальше переход на новую строку
            x0 = HandRandom.BeginX;
            y0 += HandRandom.GetLineSpace();
            // и также отображаем вторую часть строки
            int secbef = 0;
            int secpast = 1;
            for (int i = 0; i < secondPath.Length; i++)
            {
                string before = "";
                if (i > 0) before = secondPath[i - secpast] + "" + secbef;
                int y = TextToLetter(sessionId, secondPath[i], before);
                if (y >= 0) secbef = y;
                else secpast++;
            }
            // ну и все. Возврат уже не так важен. Сколько символов мы забрали.
            if (firstPath == "") return word.Length;
            return word.Length - 1;
        }


    }

