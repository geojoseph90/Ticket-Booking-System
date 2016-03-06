using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TravelAgency
{
    class TravelAgency
    {
        private static Thread[] agencyThreads;
        private static readonly object key = new Object();
        private int noOfAgencies;
        //determines number of agencies that processed price cut event
        private int proceesed;
        //most recent price cut
        private Int32 updatedPrice;
        private int orderId = 0;
        public delegate void OrderCreateEventHandler(object agency);
        public event OrderCreateEventHandler OrderCreateEvent;
        //getter setter for agency count
        public int AgencyCount
        {
            get
            {
                return noOfAgencies;
            }

            set
            {
                this.noOfAgencies = value;
                proceesed = value;

            }
        }
        public TravelAgency(int noOfAgencies)
        {
            AgencyCount = noOfAgencies;

        }

        public void init()
        {
            if (agencyThreads != null)
                return;

            //launch agency threads
            agencyThreads = new Thread[this.AgencyCount];
            for (int i = 0; i < this.AgencyCount; i++)
            {
                agencyThreads[i] = new Thread(new ThreadStart(this.RoomsForSale));
                agencyThreads[i].Name = "Agency " + (i + 1);
                agencyThreads[i].Start();
            }
        }
        //wait for price change and creates order.
        public void RoomsForSale()
        {
            Int32 lastProceessedPrice = 0;
            while (true)
            {
                lock (key)
                {
                    //keep waiting until the price changes
                    while (lastProceessedPrice == updatedPrice)
                        Monitor.Wait(key);

                    //act on new price
                    Random rand = new Random();
                    int num = rand.Next(1, 10);
                    OrderClass roomOrder = new OrderClass();
                    String encryptedOrder;
                    roomOrder.setId(++orderId);
                    roomOrder.setAmount(num);
                    int cardNum = rand.Next(5000, 7000);
                    roomOrder.setcardNum(cardNum);
                    Encrypt encode = new Encrypt();

                    String final_id = Convert.ToString(roomOrder.getorderId());
                    String final_amount = Convert.ToString(roomOrder.getorderAmount());
                    String final_card = Convert.ToString(roomOrder.getCardNum());
                    String order = final_id + "@" + final_amount + "#" + final_card;
                    //encrypting order
                    encryptedOrder = encode.encrptyString(order);
                    MultiCellBuffer buffer = MultiCellBuffer.Instance;
                    //printing order and thread details(without encryption)
                    Console.WriteLine("Order {0} created by thread {1}", order, Thread.CurrentThread.Name);
                    //Order added to buffer
                    buffer.setOne(encryptedOrder);

                    //publish event to start processing orders
                    publishOrderCreatedEvent();

                    //update state and notify other threads
                    ++proceesed;
                    lastProceessedPrice = updatedPrice;
                    Monitor.PulseAll(key);

                }
            }

        }
        //Notify suppliers to start processing the created order
        private void publishOrderCreatedEvent()
        {
            this.OrderCreateEvent(this);
        }
        public void HandleOrderProcessed(Object sender, OrderClass order, DateTime processedTime)
        {
            Console.WriteLine("The order id is {0} has been confirmed at {1}", order.getorderId(), processedTime.ToLocalTime());
        }
        //Notify the travel agencies that there is a price cut.
        public void HandlePriceCut(Object sender, Int32 newPrice)
        {
            lock (key)
            {
                //wait until all retailers process the price before proceeding
                while (proceesed != AgencyCount)
                    Monitor.Wait(key);

                proceesed = 0;
                updatedPrice = newPrice;
                Console.WriteLine("Price cut handler received new price " + newPrice);
                Monitor.PulseAll(key);
            }

        }
    }
}

