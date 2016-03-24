using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Dropbox.Api;


namespace Outa
{
    public class DropBoxConfig
    {
        public static void Init()
        {
            var task = Task.Run((Func<Task>)DropBoxConfig.Run);
            task.Wait();
        }

         private static async Task Run()
        {
            using (var dbx = new DropboxClient("YOUR ACCESS TOKEN"))
            {
                var full = await dbx.Users.GetCurrentAccountAsync();
                Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);
            }
        }
    }
}