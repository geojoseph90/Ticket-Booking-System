using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TravelAgency
{
    class HotelSupplier
    {
        public delegate void PriceCutEventHandler(object supplier, Int32 price);
        public delegate void OrderProcessedEventHandler(object supplier, OrderClass order, DateTime processedTime);
        public event PriceCutEventHandler PriceCutEvent;
        public event OrderProcessedEventHandler OrderProcessedEvent;
        public HotelSupplier(int id)
        {
            supplierId = id;
        }

        private Int32 roomPrice = 200;
        private int supplierId;
        public Int32 RoomPrice
        {
            get
            {
                return roomPrice;
            }
            set
            {
                roomPrice = value;
            }
        }

        //Pricing model generates new prices for travel agencies.
        public void PricingModel()
        {
            Random rand = new Random();
            Int32 newRoomPrice;
            Int32 num;
            for (Int32 i = 0; i < 10; i++)
            {
                num = rand.Next(-10, 10);
                newRoomPrice = RoomPrice + num;

                if (newRoomPrice < RoomPrice)
                {
                    Console.WriteLine("Sending out price cut event for the {0}", newRoomPrice);
                    RoomPrice = newRoomPrice;
                    //Notify agency on the price change.
                    publishPriceCutEvent(newRoomPrice);
                }
            }
        }

        private void publishPriceCutEvent(Int32 newPrice)
        {
            this.PriceCutEvent(this, newPrice);
        }
        //Handler to handle new order created request
        public void HandleOrderCreated(object sender)
        {
            String newOrder = MultiCellBuffer.Instance.getOne();
            //Launch new thread for processing the order
            new Thread(new ThreadStart(() =>
            {
                proceedOrder(newOrder);
            })).Start();
        }
        //The order string is read and processed
        public void proceedOrder(String newOrder)
        {
            Decryption decode = new Decryption();
            OrderClass decodedOrder = decode.decryptString(newOrder);
            if ((decodedOrder.getCardNum() > 5000) && (decodedOrder.getCardNum() < 7000))
            {
                Console.WriteLine("Order processed successfully {0}", decodedOrder.getorderId());
                this.publishOrderProcessedEvent(decodedOrder);


            }
            else
            {
                Console.WriteLine("Processing failed for order {0}", decodedOrder.getorderId());
            }
        }
        private void publishOrderProcessedEvent(OrderClass order)
        {
            this.OrderProcessedEvent(this, order, DateTime.Now);
        }

    }
}
