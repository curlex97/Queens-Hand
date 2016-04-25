using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


  


    /// <summary>
    /// Разделитель слов по слогам
    /// </summary>
    public static class WordSeparator
    {
        /// <summary>
        /// массив гласных букв
        /// </summary>
        public static char[] Vowels = new[] {'а', 'о', 'у', 'ы', 'е', 'ю', 'я', 'и', 'э', 'ё'};

        /// <summary>
        /// масив пунктуационыых символов
        /// </summary>
        public static char[] Puncts = new[] {' ', ',', '.', ':', ';', '?', '!', '"', '(', ')'};

        /// <summary>
        /// возвращает графическую длину слова
        /// </summary>
        /// <param name="word">слово</param>
        /// <returns></returns>
        public static int GetWordLength(string word)
        {
            // создаем переменную для возврата
            int length = 0;
            // по циклу
            foreach (char c in word)
            {
                // в цикл
                foreach (LetterConfig letter in HandWriteConfig.Letters)
                {
                    // если попали на букву
                    if (letter.Value == c && letter.Variable == 0)
                        // прибавляем к длине
                        length = length + letter.LeftDx + letter.Size.Width + letter.RightDx;

                }
                // ну и если пробел
                if (c == ' ')
                    // прибавляем длину пробела 
                    length += HandRandom.DefaultSpace;
            }

            // возвращаем длину
            return length;
        }

        /// <summary>
        /// возвращает две части слова, 
        /// разделенного по слогу
        /// </summary>
        /// <param name="word">слово</param>
        /// <param name="freeSpace">свободное место
        ///  до разделения</param>
        /// <returns></returns>
        public static string[] SubEdge(string word, int freeSpace)
        {
            // создаем переменную
            string str = "";
            // по циклу
            for (int i = 0; i < word.Length; i++)
            {
                // берем символ из слова
                char c = word[i];
                // если строчка еще вмещает в себя кусок слова или слово уже больше половины как добаляется,
                // но при этом счетчик больше 0
                int length = GetWordLength(str + "-");
                if ((length >= freeSpace || i > word.Length/2) && i > 0)
                    // возвращаем массив с двумя элементами. Как бить.
                    return new[] {str.Remove(str.Length - 1), word.Remove(0, i - 1)};
                // если не вернули, то добавили к строке
                str += c;
            }
            // ну и если никак, то все слово.
            return new[] {str, ""};
        }

        /// <summary>
        /// Возвращает слово с тире,
        /// разделенного дефисом
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="wordNumber">номер слова в тексте</param>
        /// <param name="freeSpace">свободное место до правой 
        /// границы в холсте</param>
        /// <returns></returns>
        public static string GetSeparateWord(string text, int wordNumber, int freeSpace)
        {
            // метод, который переносит слово.
            // сначала все проверили на правильность ввода
            if (text.Trim().Length <= 1 || freeSpace <= 0) return "";
            // потом разбиваем по пробелу
            string[] strs = text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            string word = "";
            // берем слово из массива слов по индексу
            if (strs.Length > wordNumber) word = strs[wordNumber];
            // переприсваиваем
            string sepword = word;
            // получаем разделенное слово
            string[] edge = SubEdge(word, freeSpace);
            // слово будет равно первой его части
            word = edge[1];
            // получаем количество глассных буковок
            int syllables = word.Sum(c => Vowels.Count(vowel => c == vowel));
            // если их больше одного
            if (syllables > 1)
            {
                // int count = 0;
                int first = 0;
                // цикл
                for (int i = 0; i < word.Length; i++)
                {
                    // в цикле
                    foreach (char vowel in Vowels)
                    {
                        // если это глассная и это не конец слова или еще какая-нибудь херня
                        if (word[i] == vowel && i > 0 && i < word.Length - 2)
                        {
                            // то присваиваем индекс и брейкаемся
                            first = i;
                            i = word.Length;
                            break;
                        }
                    }
                }
                // ну и если мы нашли глассную, то возврат слова, разделенного тильдочкой - 
                // спецсимвол переноса слова.
                if (first > 0)
                    sepword = word.Substring(0, first + 1) + "~" + word.Substring(first + 1);



            }
            // а если не так, то просто возврат слова.
            return edge[0] + sepword;
        }

    }

    /// <summary>
    /// Заменитель символов согласно файлу конфигурации
    /// </summary>
    public static class SymbolReplacer
    {

        /// <summary>
        /// Словарь замен
        /// </summary>
        public static Dictionary<char, char> SymbolsDictionary = new Dictionary<char, char>();

        /// <summary>
        /// Строить словарь
        /// </summary>
        /// <param name="sessionId"></param>
        public static void BuildDictionary(int sessionId)
        {
            // от сглаза
            try
            {
                // очищаем
                SymbolsDictionary.Clear();
                // читаем файл конфигурации
                string[] strs =
                    File.ReadAllLines(DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name +
                                      @"\Data\assos.cfg");
                // по циклу парсим строчку и заносим все в словарик
                foreach (string str in strs)
                {
                    string[] sub = str.Split(new[] {' '});
                    if (sub.Length >= 2)
                        // опана
                        SymbolsDictionary.Add(sub[0][0], sub[1][0]);
                }

            }
            catch
            {

            }
        }

        /// <summary>
        /// Заменить символы
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSymbols(string str)
        {
            // убирает все гавно
            str = str.Replace("\r\n", "\n");
            str = str.Replace("\r", "\n");
            string ret = "";

            //  и по циклу через словарь заменяет в тексте одно на другое
            foreach (char c in str)
            {
                char buf = c;
                foreach (KeyValuePair<char, char> pair in SymbolsDictionary)
                {
                    if (c == pair.Key) buf = pair.Value;
                }
                // потом заменяет еще раз уже по скрипту
                foreach (KeyValuePair<char, char> pair in QueenHandScript.AssosDictionary)
                {
                    if (c == pair.Key) buf = pair.Value;
                }

                ret += buf;
            }
            // возврат замененного
            return ret;

        }
    }

