using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDI
{
    public class Operation :IOperationSingleton,IOperationTransient,IOperationScoped
    {
        private Guid _guid;

        public Operation()
        {
            _guid = Guid.NewGuid();
        }

        public Operation(Guid guid)
        {
            _guid = guid;
        }

        public Guid OperationId => _guid;
    }
}
