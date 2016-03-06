using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency
{
    class Decryption
    {
        String orderId;
        String orderAmount;
        String cardNum;
        OrderClass orderObject = new OrderClass();
        public OrderClass decryptString(String input)
        {
            EncryptDecryptString.ServiceClient client = new EncryptDecryptString.ServiceClient();
            String decryptedString = client.Decrypt(input);

            orderObject.setId(int.Parse(decryptedString.Split('@')[0]));
            orderObject.setAmount(int.Parse(decryptedString.Split('@')[1].Split('#')[0]));
            orderObject.setcardNum(int.Parse(decryptedString.Split('@')[1].Split('#')[1]));
            return orderObject;

        }

    }
}
