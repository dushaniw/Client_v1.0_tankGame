using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Client_v1._0
{
    class Game
    {
        public static String playerID;
        public static Object[,] cells=new Object [100,100];
        public static char[,] gamefield=new char[100, 100];
        public static Player[] otherPlayers = {new Player(),new Player(),new Player(),new Player()};
        public static String whetherShot;
        public static int CurrentxCordinate;
        public static int CurrentyCordinate;
        public static String cDirection;
        public static int health;
        public static int coins;
        public static int points;
        public static String message;
      

        
    }
}
