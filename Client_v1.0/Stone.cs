using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_v1._0
{
    class Stone
    {
        private int xCordinate;
        private int yCordinate;

        public void setCordinates(int x, int y)
        {
            this.xCordinate = x;
            this.yCordinate = y;
        }

        public int getxCordinate()
        {
            return this.xCordinate;
        }

        public int getyCordinate()
        {
            return this.yCordinate;
        }
    }
}
