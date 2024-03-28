using NodaTime;
using System.Globalization;
using test_case.api.Converter;
using test_case.api.Enums;
using test_case.api.Models.DTO;
using test_case.api.Models.Entities;
using test_case.api.Models.Transaction;

namespace test_case.api.Extensions
{
    public static class TransactionExtensions
    {
        public static async Task<Transaction> ToTransactionAsync(this TransactionCsvModel transaction, int ClientId)
        {
            var timeZoneConverter = new TimeZoneConverter();
            var amount = transaction.Amount?.Replace("$", "") ?? string.Empty;
            var coordinates = transaction.ClientLocation?.Split(",");
            var latitude = double.Parse(coordinates![0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
            var longitude = double.Parse(coordinates![1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
            var timeZone = await timeZoneConverter.ConvertCoordinatesToTimeZoneAsync(latitude, longitude);
            var date = DateTime.Parse(transaction.TransactionDate!);

            return new Transaction
            {
                TransactionId = transaction.TransactionId!,
                ClientId = ClientId,
                Created = timeZoneConverter.ConvertDateToUtc(date, timeZone),
                Offset = timeZoneConverter.GetOffsetByDateInSeconds(date, timeZone),
                TimeZone = timeZone,
                Latitude = latitude,
                Longitude = longitude, 
                Amount = decimal.Parse(amount.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
            };
        }


        public static void UpdateTransaction(this Transaction updateTransaction, Transaction transaction)
        {
            updateTransaction.Amount = transaction.Amount;
            updateTransaction.Offset = transaction.Offset;
            updateTransaction.Latitude = transaction.Latitude;
            updateTransaction.Longitude = transaction.Longitude;
            updateTransaction.TimeZone = transaction.TimeZone;  
            updateTransaction.Created = transaction.Created;
        }
    }
}
