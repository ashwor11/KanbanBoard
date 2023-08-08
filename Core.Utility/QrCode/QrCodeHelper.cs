using QRCoder;

namespace Core.Utility.QrCode
{
    public class QrCodeHelper : IQrCodeHelper
    {
        public string? CreateQrCode(string message)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode pngByteQrCode = new PngByteQRCode(qrCodeData);
            return pngByteQrCode.ToString();
        }

        public string CreateTotpQrCode(string secret, string issuer, string label)
        {
            throw new NotImplementedException();
        }

        public string? CreateQrCode(string secret, string issuer, string label)
        {
            PayloadGenerator.OneTimePassword payload = new()
            {
                Issuer = issuer,
                Label = label,
                Secret = secret
            };
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(payload);
            PngByteQRCode pngByteQrCode = new PngByteQRCode(qrCodeData);
            return pngByteQrCode.ToString();
        }
    }
}