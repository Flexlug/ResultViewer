// Copyright 2019 Flexlug

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultViewerWPF.Viewer
{
    /// <summary>
    /// Номинация, на которую могут претендовать несколько человек.
    /// </summary>
    public class ColorRange
    {
        /// <summary>
        /// Название номинации
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество мест, которое может войти в эту номинацию
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цвет, который будет обозначать эту номинацию
        /// </summary>
        public Color CurrentColor { get; set; }

        /// <summary>
        /// Инициализирует экземпляр класса Nomination
        /// </summary>
        /// <param name="name">Название номинации</param>
        /// <param name="count">Количество мест, которое может войти в эту номинацию</param>
        /// <param name="rangeColor">Цвет, который будет обозначать эту номинацию</param>
        public ColorRange(string name, int count, Color rangeColor)
        {
            Name = name;
            Count = count;
            CurrentColor = rangeColor;
        }
    }
}
