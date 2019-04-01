using System;

namespace Notifier2
{
    public class RuleBuilder
    {
        private MainWindowVm _vm;

        public RuleBuilder(MainWindowVm vm)
        {
            _vm = vm;
        }

        public bool Allow;

        public FirewallRule BuildRule()
        {
            throw new NotImplementedException();
        }


    }
}