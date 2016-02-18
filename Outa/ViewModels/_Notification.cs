using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Outa.Models;

namespace Outa.ViewModels
{
    public class _Notification
    {
        public List<Message> Messages { get; set; }
        public List<Offer> Offers { get; set; }
    }
}