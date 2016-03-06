using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TravelAgency
{
    class MultiCellBuffer
    {
         private static MultiCellBuffer instance = new MultiCellBuffer();
        //Semphores for tracking number of free and empty slots in buffer.
        private Semaphore emptySlots = new Semaphore(2, 2);
        private Semaphore fullSlots = new Semaphore(0, 2);
        private String[] orderArray = new String[2];
        private MultiCellBuffer()
        {

        }
        //returns singleton
        public static MultiCellBuffer Instance
        {
            get
            {
                return instance;
            }
        }

        public String getOne()
        {

            fullSlots.WaitOne();
            String orderString = null;
            lock (this)
            {
                //write to buffer.free the slots and return. 
                for (int j = 0; j < orderArray.Length; j++)
                {
                    if (orderArray[j] == null)
                        continue;

                    orderString = String.Copy(orderArray[j]);
                    orderArray[j] = null;
                    break;
                }
            }
            emptySlots.Release();
            return orderString;
        }

        public void setOne(String order)
        {

            emptySlots.WaitOne();
            lock (this)
            {
                //insert value to an empty buffer space .
                for (int i = 0; i < orderArray.Length; i++)
                {
                    if (orderArray[i] == null)
                    {
                        orderArray.SetValue(order, i);
                        break;
                    }
                }

            }
            //release semaphore
            fullSlots.Release();

        }
    }

    }
