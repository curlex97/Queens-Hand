using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




    /// <summary>
    /// Генерирование случаных чисел
    /// для колебания почерка
    /// </summary>
    public static class HandRandom
    {
        /// <summary>
        /// генератор случайных чисел
        /// </summary>
        public static Random Random = new Random();

        /// <summary>
        /// старт по X в новой сроке
        /// </summary>
        public static int BeginX
        {
            get { return Random.Next(MinBeginX, MaxBeginX); }
        }

        // Конфигурационные переменные
        //==========================================================================================

        /// <summary>
        /// минимальная и максимальная доп. длины пробела и множитель длины слова 
        /// </summary>
        public static int MinGetSpaceLength = 6, MaxGetSpaceLength = 42, MultiGetSpaceLength = 5;

        /// <summary>
        /// минимальная и максимальная доп. длины межстрочного интервала
        /// </summary>
        public static int MinGetLineSpace = -7, MaxGetLineSpace = 8;

        /// <summary>
        /// делитель масштаба буквы
        /// </summary>
        public static int DivGetLetterScaleX = 9, DivGetLetterScaleY = 9, ScaleMultipler = 100;

        /// <summary>
        /// минимальная и максимальная длины смещения бувы по Y-координате
        /// </summary>
        public static int MinGetDeltaMoveY = -1, MaxGetDeltaMoveY = 2;

        /// <summary>
        /// отсуп от верхней границы
        /// </summary>
        public static int BeginY = 50;

        /// <summary>
        /// межстрочный интервал
        /// </summary>
        public static int DefaultLine = 94;

        /// <summary>
        /// пробел по умолчанию
        /// </summary>
        public static int DefaultSpace = 100, MinBeginX = 5, MaxBeginX = 55, RightEdge = 50;


        //==========================================================================================

        /// <summary>
        /// установка конфигурации
        /// </summary>
        /// <param name="minGetSpaceLength"></param>
        /// <param name="maxSpaceLength"></param>
        /// <param name="multiGetSpaceLength"></param>
        /// <param name="minGetLineSpace"></param>
        /// <param name="maxGetLineSpace"></param>
        /// <param name="divGetLetterScale"></param>
        /// <param name="minGetDeltaMoveY"></param>
        /// <param name="maxGetDeltaMoveY"></param>
        /// <param name="beginY"></param>
        /// <param name="defaultLine"></param>
        /// <param name="defaultSpace"></param>
        public static bool SetConfiguration(int minGetSpaceLength, int maxSpaceLength,
            int multiGetSpaceLength, int minGetLineSpace,
            int maxGetLineSpace, int divGetLetterScaleX, int divGetLetterScaleY,
            int minGetDeltaMoveY, int maxGetDeltaMoveY,
            int beginY, int defaultLine, int defaultSpace, int minBeginX, int maxBeginX, int scaleMultipler,
            int rightEdge)
        {

            bool flag = true;
            MinGetSpaceLength = minGetSpaceLength;
            MaxGetSpaceLength = maxSpaceLength;
            if (MinGetSpaceLength > MaxGetSpaceLength)
            {
                MinGetSpaceLength = MaxGetSpaceLength;
                flag = false;
            }
            MultiGetSpaceLength = multiGetSpaceLength;
            MinGetLineSpace = minGetLineSpace;
            MaxGetLineSpace = maxGetLineSpace;
            if (MinGetLineSpace > MaxGetLineSpace)
            {
                MinGetLineSpace = MaxGetLineSpace;
                flag = false;
            }
            DivGetLetterScaleX = divGetLetterScaleX;
            DivGetLetterScaleY = divGetLetterScaleY;
            MinGetDeltaMoveY = minGetDeltaMoveY;
            MaxGetDeltaMoveY = maxGetDeltaMoveY;
            if (MinGetDeltaMoveY > MaxGetDeltaMoveY)
            {
                MinGetDeltaMoveY = MaxGetDeltaMoveY;
                flag = false;
            }
            BeginY = beginY;
            DefaultLine = defaultLine;
            DefaultSpace = defaultSpace;
            MinBeginX = minBeginX;
            MaxBeginX = maxBeginX;
            if (MinBeginX > MaxBeginX)
            {
                MinBeginX = MaxBeginX;
                flag = false;
            }
            ScaleMultipler = scaleMultipler;
            RightEdge = rightEdge;
            return flag;
        }

        /// <summary>
        /// возвращает графическую длину пробела
        /// </summary>
        /// <param name="word">слово</param>
        /// <returns></returns>
        public static int GetSpaceLength(int word)
        {
            return DefaultSpace -
                   Random.Next(MinGetSpaceLength, MaxGetSpaceLength) +
                   (word - 6)*MultiGetSpaceLength;
        }

        public static int GetRightEdge()
        {
            return Random.Next(20, 70) + RightEdge;
        }

        /// <summary>
        /// возвращает межстрочный интервал
        /// </summary>
        /// <returns></returns>
        public static int GetLineSpace()
        {
            return DefaultLine + Random.Next(MinGetLineSpace, MaxGetLineSpace);
        }

        /// <summary>
        /// возвращает множитель масштаба X
        /// размера буквы
        /// </summary>
        /// <returns></returns>
        public static double GetLetterScaleX()
        {
            if (DivGetLetterScaleX == 0) return 1;
            return (Random.NextDouble()/DivGetLetterScaleX + 1)*
                   (Convert.ToDouble(ScaleMultipler)/100);
        }

        /// <summary>
        /// возвращает множитель масштаба Y
        /// размера буквы
        /// </summary>
        /// <returns></returns>
        public static double GetLetterScaleY()
        {
            if (DivGetLetterScaleY == 0) return 1;
            return (Random.NextDouble()/DivGetLetterScaleY + 1)*
                   (Convert.ToDouble(ScaleMultipler)/100);
        }

        /// <summary>
        /// возвращает смещение буквы
        /// по Y-координате
        /// </summary>
        /// <returns></returns>
        public static int GetDeltaMoveY()
        {
            return Random.Next(MinGetDeltaMoveY, MaxGetDeltaMoveY);
        }

        public static double GetMainScale()
        {
            return Convert.ToDouble(ScaleMultipler)/100;
        }
    }

