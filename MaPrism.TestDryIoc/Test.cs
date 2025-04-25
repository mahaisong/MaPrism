using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaPrism.TestDryIoc
{
    public static class AppRomLocator
    {
        public static IContainerProvider Container { get;  set; }
    }
        public class Test
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
    public class TestTime
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
