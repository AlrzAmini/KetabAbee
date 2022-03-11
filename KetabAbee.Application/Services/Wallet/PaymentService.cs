using KetabAbee.Application.DTOs.Wallet;
using KetabAbee.Application.Interfaces.Wallet;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.Services.Wallet
{
    public class PaymentService : IPaymentService
    {
        public PaymentStatus CreatePaymentRequest(string merchantId, int amount, string behalf, string callbackUrl, ref string redirectUrl, string userEmail)
        {
            var payment = new ZarinpalSandbox.Payment(amount);
            var result = payment.PaymentRequest(behalf, callbackUrl, userEmail);

            if (result.Result.Status == (int)PaymentStatus.St100)
            {
                redirectUrl = "https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority;
                return (PaymentStatus)result.Result.Status;
            }

            return (PaymentStatus)result.Status;
        }

        public string GetAuthorityCodeFromCallback(HttpContext context)
        {
            if (context.Request.Query["Status"] == "" ||
                context.Request.Query["Status"].ToString().ToLower() != "ok" ||
                context.Request.Query["Authority"] == "")
            {
                return null;
            }

            string authority = context.Request.Query["Authority"];
            return authority.Length == 36 ? authority : null;
        }

        public PaymentStatus PaymentVerification(string merchantId, string authority, int amount, ref long refId)
        {
            var payment = new ZarinpalSandbox.Payment(amount);
            var result = payment.Verification(authority).Result;
            refId = result.RefId;
            return (PaymentStatus)result.Status;
        }
    }
}

