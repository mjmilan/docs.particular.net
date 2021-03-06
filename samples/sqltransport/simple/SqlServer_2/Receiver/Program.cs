﻿using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.SimpleReceiver";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.SimpleReceiver");
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=SqlServerSimple;Integrated Security=True");
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.WriteLine("Waiting for message from the Sender");
            Console.ReadKey();
        }
    }
}