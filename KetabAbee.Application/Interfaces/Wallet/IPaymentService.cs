using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Wallet;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.Interfaces.Wallet
{
    public interface IPaymentService
    {
        PaymentStatus CreatePaymentRequest(string merchantId, int amount, string behalf, string callbackUrl, ref string redirectUrl, string userEmail);

        PaymentStatus PaymentVerification(string merchantId, string authority, int amount, ref long refId);

        string GetAuthorityCodeFromCallback(HttpContext context);
    }
}
