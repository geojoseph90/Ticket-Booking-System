using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency
{
    class OrderClass
    {
        private int orderId;
        private int orderAmount;
        private Int32 cardNum;


        public void setId(int i)
        {

            orderId = i;

        }
        public void setAmount(int num)
        {
            orderAmount = num;
        }
        public void setcardNum(Int32 number)
        {
            cardNum = number;
        }
        public Int32 getCardNum()
        {
            return cardNum;
        }
        public int getorderId()
        {
            return orderId;
        }
        public int getorderAmount()
        {
            return orderAmount;
        }


    }
}
