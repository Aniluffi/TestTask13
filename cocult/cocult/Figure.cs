﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace cocult
{
    /// <summary>
    /// абстрактный базовый класс для всех фигур
    /// </summary>

    [MessagePackObject]
    public abstract class Figure
    {
        
        /// <summary>
        /// обстрактный метод для вычисления площади
        /// </summary>
        /// <returns>возращает тип дабл</returns>
        public abstract double S();
        /// <summary>
        /// обстрактный метод для вычисления периметра
        /// </summary>
        /// <returns>возращает тип дабл</returns>
        public abstract double P();

        
    }
}
