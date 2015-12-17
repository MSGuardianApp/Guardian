using System;
using System.Data.Entity;

namespace SOS.Model
{
    class GuardianDbConfiguration : DbConfiguration
    {
        public GuardianDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new System.Data.Entity.SqlServer.SqlAzureExecutionStrategy(3, TimeSpan.FromSeconds(1)));
        }
    }
}
