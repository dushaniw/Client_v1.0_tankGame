using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_v1._0
{
    class Brick
    {
        private int xCordinate;
        private int yCordinate;
        private String LifeLevel = "100%";
        public void setCordinates(int x,int y)
        {
            this.xCordinate = x;
            this.yCordinate = y;
        }

        public void setLife(String lifeamount)
        {
            this.LifeLevel = lifeamount;
        }

        public String getLife()
        {
            return this.LifeLevel;
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
