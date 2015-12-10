using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Client_v1._0
{
    class Program
    {
        
        public static String playerID;
        public static GameGUI gameGUI;
        public static void Main(string[] args)
        {
            Thread startGameThread = new Thread(showGame);
            startCommunication();
            startGameThread.Start();
            
           // issueCommand();
         //   handleMessages();
            
            
        }
        public static void showGame()
        {
            Thread handleMsgThread = new Thread(handleMessages);
            handleMsgThread.Start();
            gameGUI = new GameGUI();
            Application.Run(gameGUI);
            
            //handleMessages();
           
        }

        public static void issueCommand() {
            try
            {                                      //Issue commands as UP,DOWN,LEFT,RIGHT,SHOOT from keyboard
                while (true)
                {
                    TcpClient sendSockect = new TcpClient();
                    sendSockect.Connect("localhost", 6000);
                    ConsoleKeyInfo cki = Console.ReadKey();
                    String str = "command";
                   // Console.WriteLine(cki.Key.ToString());
                    if (cki.Key.ToString().Equals("RightArrow"))
                    {
                        str = "RIGHT#";

                    }
                    else if (cki.Key.ToString().Equals("LeftArrow"))
                    {
                        str = "LEFT#";
                    }
                    else if (cki.Key.ToString().Equals("UpArrow"))
                    {
                        str = "UP#";
                    }
                    else if (cki.Key.ToString().Equals("DownArrow"))
                    {
                        str = "DOWN#";
                    }
                    else if (cki.Key.ToString().Equals("Spacebar"))
                    {
                        str = "SHOOT#";
                    }
                    else
                    {
                        continue;
                    }
                  
                    NetworkStream serverStream = sendSockect.GetStream();

                    ASCIIEncoding encode = new ASCIIEncoding();
                    byte[] outStream = encode.GetBytes(str);
                    serverStream.Write(outStream, 0, outStream.Length);
                    Console.WriteLine("Sent "+str);
                    serverStream.Close();
                    sendSockect.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }


        
        
        }

        public static void startCommunication() {

            try
            {
                TcpClient sendSockect = new TcpClient();
                sendSockect.Connect("localhost", 6000);  //join the game
                String str = "JOIN#";
                NetworkStream serverStream = sendSockect.GetStream();

                ASCIIEncoding encode = new ASCIIEncoding();
                byte[] outStream = encode.GetBytes(str);
                serverStream.Write(outStream, 0, outStream.Length);
                Console.WriteLine("Sent Join#");
                                
                serverStream.Close();
                sendSockect.Close();
                             
        
            }
            catch (Exception e)
            {
                Console.WriteLine("Error.." + e.StackTrace);
                
            }
                         
        }

        public static void handleMessages()
        {
            TcpListener listnerSocket = new TcpListener(IPAddress.Any,7000);
            Byte[] bytes = new Byte[1024];
            listnerSocket.Start();
            String data;
            
            while (true) {
                TcpClient gameServer = listnerSocket.AcceptTcpClient();
                data = null;
                NetworkStream serverStream = gameServer.GetStream();
                int i;
                while((i=serverStream.Read(bytes,0,bytes.Length))!=0){
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    formatMessage(data);    //format the message
                    if (gameGUI != null)
                    {
                        gameGUI.updateGUI();    //update client GUI
                    }
                     
                }
                gameServer.Close();
                serverStream.Close();

            
            
            }
            
        }

        public static void formatMessage(String msg){
            String[] firstpart = msg.Split('#');
            String[] parts = firstpart[0].Split(':');
            try
            {
                Game.message = "";    
                String msg_format = parts[0];
                if (msg_format.Equals("S"))    //parse intial acceptance message
                {
                    String[] playerDetails = parts[1].Split(';');

                    playerID = playerDetails[0];
                    
                    String[] loc_cordinates = playerDetails[1].Split(',');
                    String direction = playerDetails[2];
                    Game.playerID = playerID;

                    Console.WriteLine("================================================================================\n");
                    Console.WriteLine("Player accepted");
                    Console.WriteLine("Player: " + playerID);
                    Console.WriteLine("Location====>  X = " + loc_cordinates[0] + " Y = " + loc_cordinates[1]);
                    Console.WriteLine("Direction : " + direction);
                    Game.CurrentxCordinate = Int32.Parse(loc_cordinates[0]);
                    Game.CurrentyCordinate = Int32.Parse(loc_cordinates[1]);
                    if (direction.Equals("0"))
                    {
                        Game.cDirection="North";
                    }
                    else if (direction.Equals("1")){
                        Game.cDirection="East";
                    }
                    else if (direction.Equals("2")){
                        Game.cDirection="South";
                    }
                    else if (direction.Equals("3")){
                        Game.cDirection="West";
                    }
                   
                    Thread commandIssuerThread = new Thread(issueCommand);
                    commandIssuerThread.Start();


                }
                else if (msg_format.Equals("I")) // map details 
                 {
                     //Console.WriteLine("================================================================================\n");
                    // Console.WriteLine("New Game Instant received");
                     String player = parts[1];
                   //Console.WriteLine("Player: " + player);

                     // Brick cordinates details
                    //Console.WriteLine("Bricks -------------------------------------------");
                     String brick_map = parts[2];
                     String[] bricks = brick_map.Split(';');
                     int brick_no = 1;
                     foreach (String brick in bricks)
                     {
                         String[] brick_location = brick.Split(',');
                         Game.cells[Int32.Parse(brick_location[0]), Int32.Parse(brick_location[1])] = new Brick();
                         Game.gamefield[Int32.Parse(brick_location[0]), Int32.Parse(brick_location[1])] = 'B';
                         
                        // Console.WriteLine("Brick No. " + brick_no + " location ==>  X = " + brick_location[0] + " Y = " + brick_location[1]);
                         brick_no++;
                     }

                     // Stone cordinates details

                    // Console.WriteLine("Stones --------------------------------------------");
                     String stone_map = parts[3];
                     String[] stones = stone_map.Split(';');
                     int stone_no = 1;
                     foreach (String stone in stones)
                     {
                         String[] stone_location = stone.Split(',');
                         Game.cells[Int32.Parse(stone_location[0]), Int32.Parse(stone_location[1])]=new Stone();
                         Game.gamefield[Int32.Parse(stone_location[0]), Int32.Parse(stone_location[1])]='S';
                        
                        // Console.WriteLine("Stone No. " + stone_no + " location ==>  X = " + stone_location[0] + " Y = " + stone_location[1]);
                         stone_no++;
                     }

                     // Water cordinates details
                  //   Console.WriteLine("Water --------------------------------------------");
                     String water_map = parts[4];
                     String[] waters = water_map.Split(';');
                     int water_no = 1;
                     foreach (String water in waters)
                     {
                         String[] water_location = water.Split(',');
                         Game.cells[Int32.Parse(water_location[0]), Int32.Parse(water_location[1])] = new Water();
                         Game.gamefield[Int32.Parse(water_location[0]), Int32.Parse(water_location[1])] = 'W';
                        // Console.WriteLine("Water No. " + water_no + " location ==>  X = " + water_location[0] + " Y = " + water_location[1]);
                         water_no++;
                     }
               }
               else if (msg_format.Equals("G")) // global updates
               {
                   Console.WriteLine("================================================================================\n");
                   Console.WriteLine("New global Update received");
                     int player_no = 1;
                     int other_playerCount = 0;
                     for (player_no = 1; player_no <= 5; player_no++)
                     {
                         String player_code = parts[player_no];
                         if (player_code.Substring(0, 1).Equals("P")) // this is a player sub string
                         {
                           
                                 String[] player_details = player_code.Split(';');
                                 Console.WriteLine("Player Details -----------------------------");
                                 Console.WriteLine("Player name: " + player_details[0]);
                                 String[] player_loc = player_details[1].Split(',');
                                 Console.WriteLine("X ==> " + player_loc[0] + " y ==> " + player_loc[1]);
                                 Console.WriteLine("Direction ==> " + player_details[2]);
                                 Console.WriteLine("Whehter shot ==> " + player_details[3]);
                                 Console.WriteLine("Health ==> " + player_details[4]);
                                 Console.WriteLine("Coins ==> " + player_details[5]);
                                 Console.WriteLine("Points ==> " + player_details[6]);
                                     
                                 

                                 if (player_details[0] == playerID)   //update on client player (this player)
                                 {
                                     if (player_details[2].Equals("0"))
                                     {
                                         Game.cDirection = "North";
                                     }
                                     else if (player_details[2].Equals("1"))
                                     {
                                         Game.cDirection = "East";
                                     }
                                     else if (player_details[2].Equals("2"))
                                     {
                                         Game.cDirection = "South";
                                     }
                                     else if (player_details[2].Equals("3"))
                                     {
                                         Game.cDirection = "West";
                                     }
                                     Game.whetherShot = player_details[3];
                                     Game.health = Int32.Parse(player_details[4]);
                                     Game.coins = Int32.Parse(player_details[5]);
                                     Game.points = Int32.Parse(player_details[6]);
                                     Game.CurrentxCordinate = Int32.Parse(player_loc[0]);
                                     Game.CurrentyCordinate = Int32.Parse(player_loc[1]);
                                 }
                                 else
                                 {              //update on other players
                                    // Console.WriteLine(player_no - 1+"fdfdfdfdfd");
                                     Game.otherPlayers[other_playerCount].setX(Int32.Parse(player_loc[0]));
                                     Game.otherPlayers[other_playerCount].setY(Int32.Parse(player_loc[1]));
                                     Game.otherPlayers[other_playerCount].setId(player_details[0]);
                                     if (player_details[2].Equals("0"))
                                     {
                                         Game.otherPlayers[other_playerCount].setCDirection("North");
                                     }
                                     else if (player_details[2].Equals("1"))
                                     {
                                         Game.otherPlayers[other_playerCount].setCDirection("East");
                                     }
                                     else if (player_details[2].Equals("2"))
                                     {
                                         Game.otherPlayers[other_playerCount].setCDirection("South");
                                     }
                                     else if (player_details[2].Equals("3"))
                                     {
                                         Game.otherPlayers[other_playerCount].setCDirection("West");
                                     }
                                     Game.otherPlayers[other_playerCount].setwhetherShot(player_details[3]);
                                     Game.otherPlayers[other_playerCount].setHealth(Int32.Parse(player_details[4]));
                                     Game.otherPlayers[other_playerCount].setCoins(Int32.Parse(player_details[5]));
                                     Game.otherPlayers[other_playerCount].setPoints(Int32.Parse(player_details[6]));
                                     Console.WriteLine(other_playerCount);
                                     other_playerCount++;
                                    
                                 }


                             }

                            
                         
                         else
                             break;
                     }
                        Console.WriteLine("================================================================================\n");
                        Console.WriteLine("Moving shot details");
                     String[] shots = parts[player_no].Split(';');
                     foreach (String shot in shots)
                     {
                         String[] shot_details = shot.Split(',');
                         int x = Int32.Parse(shot_details[0]);
                         int y = Int32.Parse(shot_details[1]);
                         
                         if (Game.gamefield[x, y] == 'B')
                         {                                      //update the life time of bricks when damaged by shots
                             Brick brick = (Brick)(Game.cells[x, y]);
                             if (shot_details[2].Equals("4"))
                             {
                                 brick.setLife("0%");
                                 Game.cells[x, y] = null;
                                 Game.gamefield[x, y] = '\0';
                                                              }
                             else if (shot_details[2].Equals("3"))
                             {

                                 brick.setLife("25%");
                             }
                             else if (shot_details[2].Equals("2"))
                             {
                                 brick.setLife("50%");
                             }
                             else if (shot_details[2].Equals("1"))
                             {
                                 brick.setLife("75%");
                             }
                             else if (shot_details[2].Equals("0"))
                             {
                                 brick.setLife("100%");
                             }

                             Console.WriteLine("Shot details ####  x==> " + shot_details[0] + " y ==> " + shot_details[1] + " damage level ==> " + shot_details[2]);
                         }

                         }

                   
                 }

                 else if (msg_format.Equals("C")) // coin details
                {
                    Console.WriteLine("================================================================================\n");
                    Console.WriteLine("Coin appearance received");
                    String[] location = parts[1].Split(',');
                    Coin coin = new Coin();
                    coin.setTime(Int32.Parse(parts[2]));
                    coin.setValue(Int32.Parse(parts[3]));
                         
                    Game.cells[Int32.Parse(location[0]), Int32.Parse(location[1])] = coin;
                    Game.gamefield[Int32.Parse(location[0]), Int32.Parse(location[1])] = 'C';
                    
                     Console.WriteLine("Location x ==> " + location[0] + " y ==> " + location[1]);
                    Console.WriteLine("Time to disappear ==> " + parts[2]);
                    Console.WriteLine("Value of coins ==> " + parts[3]);
                 }
                 else if (msg_format.Equals("L")) // life pack details
                 {
                     Console.WriteLine("================================================================================\n");
                     Console.WriteLine("Life Pack appearance received");
                     String[] location = parts[1].Split(',');

                     LifePack life = new LifePack();
                     life.setTime(Int32.Parse(parts[2]));
                     
                     Game.cells[Int32.Parse(location[0]), Int32.Parse(location[1])] = life;
                     Game.gamefield[Int32.Parse(location[0]), Int32.Parse(location[1])] = 'L';


                     Console.WriteLine("Location x ==> " + location[0] + " y ==> " + location[1]);
                     Console.WriteLine("Time to disappear ==> " + parts[2]);

                 }
                else
                {

                    Game.message = firstpart[0];
                    Console.WriteLine(firstpart[0]);
                }
                

            }
            catch (Exception e)
            {

                Console.WriteLine(e.StackTrace);
            }



        }







    }
}
