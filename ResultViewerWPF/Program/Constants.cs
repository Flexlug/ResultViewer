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

namespace ResultViewerWPF
{
    class Constants
    {
        /// <summary>
        /// Означает, что участник на данном этапе не получил баллов
        /// </summary>
        public const double MEMBER_NO_POINTS = -1;

        /// <summary>
        /// Означает, что участник отсутствовал на данном этапе
        /// </summary>
        public const double MEMBER_ABSENT = -2;
        
        /// <summary>
        /// Константа, которая указывает, что данный балл уже был присвоен в данном туре и его больше не надо трогать
        /// </summary>
        public const double VIEWER_POINT_USED = -3;
    }
}
