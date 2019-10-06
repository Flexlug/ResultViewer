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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ResultViewerWPF.Viewer.Primitives
{
    /// <summary>
    /// Абстрактное представление панели, которая поддаётся анимации
    /// </summary>
    public class Bar
    {
        /// <summary>
        /// Коллекция из различных Transform-ов, которые можно анимировать
        /// </summary>
        public TransformGroup BarTG;

        /// <summary>
        /// Главная панель, на которой расположен весь декор
        /// </summary>
        public Panel mainPanel;

        /// <summary>
        /// Точка, куда движется объект на данный момент, или где он находится на данный момент
        /// </summary>
        public Point movingTo;

        public TranslateTransform GetTT()
        {
            return BarTG.Children[0] as TranslateTransform;
        }

        public ScaleTransform GetST()
        {
            return BarTG.Children[1] as ScaleTransform;
        }
    }
}
