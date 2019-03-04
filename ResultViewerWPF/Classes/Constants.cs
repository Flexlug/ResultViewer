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
