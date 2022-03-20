using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transaction.infrastructure.Setting;

public class ApiSetting
{
    public string UserApi { get; set; }
    public int HandlerLifetimeMinutes { get; set; }
    public string UserAgent { get; set; }
}
