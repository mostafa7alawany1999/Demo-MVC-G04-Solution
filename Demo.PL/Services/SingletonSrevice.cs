using System;

namespace Demo.PL.Services
{
    public class SingletonSrevice : ISingletonSrevice
    {
        public Guid Guid { get; set; }

        public SingletonSrevice()
        {
            Guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return Guid.ToString();
        }
        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}
