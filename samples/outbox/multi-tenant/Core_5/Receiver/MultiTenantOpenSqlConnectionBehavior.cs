﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class MultiTenantOpenSqlConnectionBehavior : IBehavior<IncomingContext>
{

    public void Invoke(IncomingContext context, Action next)
    {
        string defaultConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"]
            .ConnectionString;
        #region OpenTenantDatabaseConnection

        string tenant;
        if (!context.PhysicalMessage.Headers.TryGetValue("TenantId", out tenant))
        {
            throw new InvalidOperationException("No tenant id");
        }
        string connectionString = ConfigurationManager.ConnectionStrings[tenant]
            .ConnectionString;
        Lazy<IDbConnection> lazyConnection = new Lazy<IDbConnection>(() =>
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        });
        string key = $"LazySqlConnection-{defaultConnectionString}";
        context.Set(key, lazyConnection);
        try
        {
            next();
        }
        finally
        {
            if (lazyConnection.IsValueCreated)
            {
                lazyConnection.Value.Dispose();
            }

            context.Remove(key);
        }

        #endregion
    }
}