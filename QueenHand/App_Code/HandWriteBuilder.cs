using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    /// <summary>
    /// строитель почерка по форме заполнения почерка
    /// </summary>
    public class HandWriteBuilder
    {


        /// <summary>
        /// список символов настройки почерка
        /// </summary>
        private static List<LetterConfig> configuration = new List<LetterConfig>();

        /// <summary>
        /// Метод очистки формы от сетки
        /// </summary>
        /// <param name="image">изображние формы</param>
        /// <param name="x">откуда чистим по х</param>
        /// <param name="y">откуда чистим по у</param>
        public static void ClearGrid(Bitmap image, int x, int y)
        {



            // создаем список точек
            List<Point> points = new List<Point>();

            // в этом цикле ищем черное
            while (y < 300)
            {
                Color color1 = image.GetPixel(x, y);
                int avg1 = (color1.R + color1.G + color1.B) / 3;
                if (avg1 < 100)
                {
                    // находим, выходим
                    points.Add(new Point(x, y));
                    break;
                }
                y++;

            }

            // буфферный лист
            List<Point> list = new List<Point>();

            // пока есть черные точки
            while (points.Count > 0)
            {
                // очищаем буффер
                list.Clear();

                // проход по точкам
                for(int i=0; i<points.Count; i++) 
                {
                    // изменение им цвета на белый
                        image.SetPixel(points[i].X, points[i].Y, Color.FromArgb(255, 255, 255, 255));

                        // проверка их соседей на черность
                        for (int _x = -1; _x < 2; _x++)
                        {
                            for (int _y = -1; _y < 2; _y++)
                            {
                                // получаем цвет соседа
                                Color color = image.GetPixel(points[i].X + _x, points[i].Y + _y);
                                // находим среднее значение
                                int avg = (color.R + color.G + color.B) / 3;
                                // если темный
                                if (avg <= 225 && !list.Contains(new Point(points[i].X + _x, points[i].Y + _y)))
                                    // заносим в буффер
                                    list.Add(new Point(points[i].X + _x, points[i].Y + _y));
                            }
                        }
                    

                }
                // очищаем точки, поскольку теперь они белые
                points.Clear();
                // и зафигачиваем в них буффер, то есть найденные точки
                // такой себе аналог рекурсии через память
                // стековерфлоу, простите
                Point[] temp = list.ToArray().Clone() as Point[];
                points.AddRange(temp);
            }

          
        }

        /// <summary>
        /// Конфигурации символов
        /// </summary>
        public static List<LetterConfig> Configuration
        {
            get { return configuration; }
        }
        

        /// <summary>
        /// Возврат пути к файлу изображения символа
        /// </summary>
        /// <param name="sessionId">сессия</param>
        /// <param name="leftMark">марка левая</param>
        /// <param name="rightMark">марка правая</param>
        /// <param name="symbol">символ</param>
        /// <param name="index">индекс символа</param>
        /// <returns></returns>
        protected static string GetPath(int sessionId, Color leftMark, Color rightMark, char symbol, int index)
        {
            // нижний регистр, это когда левая марка черная, а правая светлая
            if (leftMark.GetBrightness() < .5f && rightMark.GetBrightness() > .5f)
                // возврат нижнего регистра
                return DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Let\" + symbol + index +
                       ".png";
            // если и то и то белое - верхний регистр
            if (leftMark.GetBrightness() > .5f && rightMark.GetBrightness() > .5f)
                // возврат верхнего регистра
                return DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Let\" + symbol + "!" +
                       index + ".png";
            // а если белый левый, а правый черный, то это специальные символы и цифры
            if (leftMark.GetBrightness() > .5f && rightMark.GetBrightness() < .5f)
                // для них также как и для нижнего реистра
                return DataBase.ServerPath + @"\Content\Users\" + DataBase.CurrentUser(sessionId).Name + @"\Let\" + symbol + index +
                       ".png";
            // если ваще не то шото.
            return String.Empty;
        }

        /// <summary>
        /// Парсинг одного столбца букв
        /// </summary>
        /// <param name="sessionId">сессия</param>
        /// <param name="image">изображение формы</param>
        /// <param name="leftMark">левая марка</param>
        /// <param name="rightMark">правая марка</param>
        /// <param name="startX">начало парса</param>
        /// <param name="startY">начало парса</param>
        /// <param name="symbols">какие будут символы в столбце</param>
        /// <param name="startId">начало индексов</param>
        /// <param name="endId">конец индексов</param>
        protected static void BuildLine(int sessionId, Bitmap image,
            Color leftMark,
            Color rightMark,
            int startX,
            int startY,
            string symbols,
            int startId,
            int endId)
        {
            // получаем количество индексов
            int length = endId - startId;

            //  и поехали по всем буквам
            for (int i = 0; i < symbols.Length; i++)
            {
                // работаем по строкам
                for (int j = 0; j < length; j++)
                {
                    // получаем кусок изображения
                    Bitmap letterBitmap = image.Clone(new Rectangle(startX + j*143, startY + i*300, 115, 115),
                        image.PixelFormat);
                    // приводим в божеский вид
                    Bitmap outBitmap = new Bitmap(letterBitmap.Width, letterBitmap.Height);

                    // заменяем весь белый на прозрачный
                    for (int bi = 0; bi < outBitmap.Height; bi++)
                    {
                        for (int bj = 0; bj < outBitmap.Width; bj++)
                        {
                            Color color = letterBitmap.GetPixel(bj, bi);
                            int avg = (color.R + color.B + color.G)/3;
                            if (avg < 225) outBitmap.SetPixel(bj, bi, color);
                        }
                    }

                    // полчучаем путь для сохранения изображения
                    string path = GetPath(sessionId, leftMark, rightMark, symbols[i], startId + j);
                    // MessageBox.Show(path);
                    //   if(new FileInfo(path).Exists) new FileInfo(path).Delete();
                    // обрезаем прозрачные куски
                    Bitmap resultBitmap = SliceLetter(outBitmap);
                    // сохраняем изображение
                    resultBitmap.Save(path, ImageFormat.Png);
                    // добавляем в конфигурацию буковку
                    configuration.Add(GetConfig(symbols[i], j, resultBitmap.Width, resultBitmap.Height));
                }
            }
        }

        /// <summary>
        /// Строит почерк по форме
        /// </summary>
        /// <param name="sessionId">сессия</param>
        /// <param name="path">путь к изображению</param>
        /// <param name="startId">начало вариативности индекса</param>
        /// <param name="endId">конец вариативности индекса</param>
        public static void BuildHandWrite(int sessionId, string path, int startId, int endId)
        {
            // три столбца
            string firstPath = "", secondPath = "", thirdPath = "";
            // три конфигурации для стобцов
            int firstX = 0, firstY = 0;
            int secondX = 0, secondY = 0;
            int thirdX = 0, thirdY = 0;
            // получаем форму
            Bitmap image = new Bitmap(path);


            // получаем марки
            Color leftMark = image.GetPixel(100, 100);
            Color rightMark = image.GetPixel(2450, 100);

            // чистим грид
            ClearGrid(image, 200, 150);
            // image.Save(DataBase.ServerPath + "default.png", ImageFormat.Png);

            // в зависимости от конфигурации ставим значения.

            if (leftMark.GetBrightness() < .5f && rightMark.GetBrightness() > .5f)
            {
                firstX = 200;
                firstY = 205;
                secondX = 925;
                secondY = 357;
                thirdX = 1648;
                thirdY = 205;
                firstPath = "абвгдежзийк";
                secondPath = "лмнопрстуф";
                thirdPath = "хцчшщъыьэюя";
            }
             if (leftMark.GetBrightness() > .5f && rightMark.GetBrightness() > .5f)
            {
                firstX = 195;
                firstY = 205;
                secondX = 920;
                secondY = 357;
                thirdX = 1643;
                thirdY = 205;
                firstPath = "абвгдежзийк".ToLower();
                secondPath = "лмнопрстуф".ToLower();
                thirdPath = "хцчшщъыьэюя".ToLower();
            }
            if (leftMark.GetBrightness() > .5f && rightMark.GetBrightness() < .5f)
            {

            }

            // строим изображениякаждого столбца
            BuildLine(sessionId, image, leftMark, rightMark, firstX, firstY, firstPath, startId, endId);
            BuildLine(sessionId, image, leftMark, rightMark, secondX, secondY, secondPath, startId, endId);
            BuildLine(sessionId, image, leftMark, rightMark, thirdX, thirdY, thirdPath, startId, endId);
        }


        /// <summary>
        /// Возвращает конфигурацию буквы
        /// </summary>
        /// <param name="c"></param>
        /// <param name="idx"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static LetterConfig GetConfig(char c, int idx, int width, int height)
        {
            
            return new LetterConfig(c, idx, 0, 2, 0, new Size(width, height));
        }

        /// <summary>
        /// Обрезает букву, убирая прозрачные части
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        protected static Bitmap SliceLetter(Bitmap letter)
        {
            // исходные значения
            int x = 0, y = 0, w = letter.Width, h = letter.Height;

            // проходимся слева напрво.
            for (int i = 0; i < letter.Width; i++)
            {
                int count = 0;
                for (int j = 0; j < letter.Height; j++)
                    if (letter.GetPixel(i, j).A > 20) count++;
                // прифигачиваем к иксу, если не нашли больше трех непрозрачных
                if (count < 3) x++;
                else break;
            }
            // проходимся сверху вниз
            for (int i = 0; i < letter.Height; i++)
            {
                int count = 0;
                for (int j = 0; j < letter.Width; j++)
                    if (letter.GetPixel(j, i).A > 20) count++;
                // тоже самое к у
                if (count < 3) y++;
                else break;
            }

            // проходимся спарва налево
            for (int i = letter.Width - 1; i >= 0; i--)
            {
                int count = 0;
                for (int j = 0; j < letter.Height; j++)
                    if (letter.GetPixel(i, j).A > 20) count++;
                // также все
                if (count < 3) w--;
                else break;
            }

            // проходимся снизу вверх
            for (int i = letter.Height - 1; i >= 0; i--)
            {
                int count = 0;
                for (int j = 0; j < letter.Width; j++)
                    if (letter.GetPixel(j, i).A > 20) count++;
                // получаем последнюю переменную
                if (count < 3) h--;
                else break;
            }
            // дальше, если это еще площадь чего либо
            if (w - x > 0 && h - y > 0)
            {
                // создаем такой битмап и возвращаем его
                Bitmap bitmap = new Bitmap(w - x, h - y);
                bitmap = letter.Clone(new Rectangle(x, y, w - x, h - y), letter.PixelFormat);
                return bitmap;
            }
            // иначе просто пустой битмап 64 на 64
             return new Bitmap(64, 64);
        }
    }

