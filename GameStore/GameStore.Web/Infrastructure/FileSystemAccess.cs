using System.IO;
using System.Text;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Infrastructure
{
    public class FileSystemAccess
    {
        public void CreateGameFile(string path, string data)
        {
            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var info = new UTF8Encoding(true).GetBytes(data);
                fs.Write(info, 0, info.Length);
            }
        }

        public void CreateInvoiceFile(string path, PaymentViewModel model)
        {
            using (var sw = new StreamWriter(path, false, Encoding.Default))
            {
                sw.WriteLine("New bill");
                sw.WriteLine("Total price: $" + model.Sum);
                sw.WriteLine("Invoice number: " + model.InvoiceNumber);
                sw.WriteLine("Account number: " + model.AccountNumber);
                sw.WriteLine("Date: " + model.Date);
            }
        }
    }
}