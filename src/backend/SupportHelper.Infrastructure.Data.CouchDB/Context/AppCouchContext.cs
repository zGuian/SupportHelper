using CouchDB.Driver;
using CouchDB.Driver.Options;
using Microsoft.Extensions.Configuration;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.Data.CouchDB.Models;

namespace SupportHelper.Infrastructure.Data.CouchDB.Context
{
    public class AppCouchContext : CouchContext
    {
        public CouchDatabase<MachineModel> Machines { get; set; }
        private readonly IConfiguration _configuration;

        public AppCouchContext(CouchOptions<AppCouchContext> opts, IConfiguration configuration) : base(opts)
        {
            _configuration = configuration;
        }

        protected override void OnDatabaseCreating(CouchDatabaseBuilder db)
        {
            db.Document<MachineModel>().ToDatabase("machine-dev-db");
        }
    }
}
