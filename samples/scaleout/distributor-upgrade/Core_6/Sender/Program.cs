﻿using System;
using System.Linq;
using NServiceBus;

internal class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Sender");
        busConfiguration.EnableInstallers();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.Conventions()
            .DefiningMessagesAs(t => t.GetInterfaces().Contains(typeof (IMessage)));

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'Enter' to send a message.");
            Console.WriteLine("Press any other key to exit.");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                SendMessage(bus);
            }
        }
    }

    static void SendMessage(IBus bus)
    {
        PlaceOrder placeOrder = new PlaceOrder
        {
            OrderId = Guid.NewGuid()
        };

        #region SenderRouting

        bus.Send("Samples.Scaleout.Distributor", placeOrder);

        #endregion

        Console.WriteLine("Sent PlacedOrder command with order id [{0}].", placeOrder.OrderId);
    }
}