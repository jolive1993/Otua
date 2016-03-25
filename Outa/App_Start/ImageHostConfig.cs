using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace Outa
{
    public class ImageHostConfig
    {
        public static Cloudinary Cloudinary { get; set; }
        public static void Init()
        {
            Account account = new Account(
            "rockitshipshawty",
            "969736378743428",
            "SouveLJZlKhFpf2B01Qm80bV_9E");
            Cloudinary = new Cloudinary(account);
        }
    }
}