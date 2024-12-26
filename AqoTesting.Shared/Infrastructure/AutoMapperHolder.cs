using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Infrastructure
{
    public static class AutoMapperHolder
    {
        public static Mapper Mapper { get; set => field = (field == null) ? value : throw new ArgumentException("Mapper is already set", nameof(Mapper)); } = null!;
    }
}
