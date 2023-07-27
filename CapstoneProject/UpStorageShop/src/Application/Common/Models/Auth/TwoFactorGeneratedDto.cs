using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Auth
{
    public class TwoFactorGeneratedDto
    {
        public byte[] QrCodeImage { get; set; }
        public string Key { get; set; }
    }
}
