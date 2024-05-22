using System;

namespace Demo.PL.Services
{
    public interface ISingletonSrevice
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}
