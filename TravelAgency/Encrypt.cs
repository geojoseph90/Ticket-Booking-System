using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency
{
    class Encrypt
    {
        String orderId;
        String orderAmount;
        String cardNum;
        public String encrptyString(String input)
        {
            EncryptDecryptString.ServiceClient client = new EncryptDecryptString.ServiceClient();
            String encryptedString = client.Encrypt(input);
            //String encryptedString = input;

            return encryptedString;

        }

    }
}
