﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_v1._0
{
    class Coin
    {
        int value;
        int time;
       /* int start_time;

        public int getStart_tine()
        {
            return start_time;
        }


        public void setStart_time(int s_time) {
            start_time = s_time;
        }
        */

        public int getValue()
        {
            return value;
        }
        public int getTime()
        {
            return time;
        }

        public void setValue(int val)
        {
            value = val;
        }

        public void setTime(int t)
        {
            time = t;
        }



    }
}
