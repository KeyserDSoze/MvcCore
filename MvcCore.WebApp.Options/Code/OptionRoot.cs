using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.Options.Code
{
    public class OptionRoot
    {
        public string Option1 { get; set; }
        public string Option2 { get; set; }
    }
    public class OptionRoot2
    {
        public string Option3 { get; set; }
        public string Option4 { get; set; }
    }
    public class OptionRootMonitor : IOptionsMonitor<OptionRoot>
    {
        public OptionRoot CurrentValue => throw new NotImplementedException();

        public OptionRoot Get(string name)
        {
            throw new NotImplementedException();
        }

        public IDisposable OnChange(Action<OptionRoot, string> listener)
        {
            throw new NotImplementedException();
        }
    }
}
