using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_v1._0
{
    class Player
    {
        String playerID;
        int xCordinate;
        int yCordinate;
        int coins;
        int points;
        int health;
        String cDirection;
        String whetherShot;

        public void setId(String id)
        {
            playerID = id;
        }

        public String getid()
        {
            return playerID;
        }

        public void setHealth(int health)
        {
            this.health = health ;
        }

        public int getHealth()
        {
            return health;
        }

        public void setCoins(int coins)
        {
            this.coins=coins;
        }

        public int getCoins()
        {
            return coins;
        }

        public int getPoints()
        {
            return points;
        }

        public String getcDirection()
        {
            return cDirection;
        }

        public String getwhetherShot()
        {
            return whetherShot;
        }

        public void setPoints(int points)
        {
            this.points = points;
        }
        public void setCDirection(String d)
        {
            this.cDirection = d;
        }
        public void setwhetherShot(String shot)
        {
            this.whetherShot = shot;
        }

        public void setX(int x)
        {
            this.xCordinate = x;
        }

        public void setY(int y)
        {
            this.yCordinate = y;
        }

        public int getX()
        {
            return xCordinate;
        }

        public int getY()
        {
            return yCordinate;
        }
    }
}
