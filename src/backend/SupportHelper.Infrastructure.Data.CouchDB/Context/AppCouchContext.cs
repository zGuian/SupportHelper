using CouchDB.Driver;
using CouchDB.Driver.Options;
using Microsoft.Extensions.Configuration;
using SupportHelper.Infrastructure.Data.CouchDB.Models;

namespace SupportHelper.Infrastructure.Data.CouchDB.Context
{
    public class AppCouchContext : CouchContext
    {
        public CouchDatabase<MachineModel> Machines { get; set; }

        public AppCouchContext(CouchOptions<AppCouchContext> opts) : base(opts)
        { }
    }
}
