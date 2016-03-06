using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TravelAgency
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            int noOfSuppliers = 1;
            int noOfAgencies = 5;
            HotelSupplier[] suppliers = new HotelSupplier[noOfSuppliers];
            TravelAgency agency = new TravelAgency(noOfAgencies);

            //launch supplier threads.Only one supplier in this case.
            for (int i = 0; i < suppliers.Length; i++)
            {
                suppliers[i] = new HotelSupplier(i);
                Thread supplierThread = new Thread(new ThreadStart(suppliers[i].PricingModel));
                //Adding event handler.pricecut and ordercreate
                suppliers[i].PriceCutEvent += new HotelSupplier.PriceCutEventHandler(agency.HandlePriceCut);
                suppliers[i].OrderProcessedEvent += new HotelSupplier.OrderProcessedEventHandler(agency.HandleOrderProcessed);
                agency.OrderCreateEvent += new TravelAgency.OrderCreateEventHandler(suppliers[i].HandleOrderCreated);
                supplierThread.Start();
            }

            //launch price cut event process thread.
            agency.init();


        }
    }
}
