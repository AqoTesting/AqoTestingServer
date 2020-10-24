using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.Infrastructure
{
    public class WorkContext : IWorkContext
    {
        public ObjectId UserId { get; set; }
    }
}
