using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace GameStore.Web.Infrastructure
{
    public class RenderImageHttpHandler : HttpTaskAsyncHandler
    {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            var path = context.Request.QueryString["path"];
            var filePath = context.Server.MapPath(path);
            var extension = Path.GetExtension(path);
            var mime = "image/" + extension?.Substring(1);
            await Task.Run(() =>
            {
                byte[] data;
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                }

                context.Response.ContentType = mime;
                context.Response.BinaryWrite(data);
            });
        }
    }
}