﻿using System;
using global::NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region ConfiguringNHibernate

        //Use NHibernate for all persistence concerns
        busConfiguration.UsePersistence<NHibernatePersistence>();

        //or select specific concerns
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Timeouts>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();
        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();

        #endregion

        #region NHibernateSubscriptionCaching

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Subscriptions>();
        persistence.EnableCachingForSubscriptionStorage(TimeSpan.FromMinutes(1));

        #endregion
    }


    void CustomCommonConfiguration(BusConfiguration busConfiguration)
    {
        #region CommonNHibernateConfiguration

        Configuration nhConfiguration = new Configuration();
        nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";
        nhConfiguration.Properties["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
        nhConfiguration.Properties["connection.driver_class"] = "NHibernate.Driver.Sql2008ClientDriver";

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }

    void SpecificNHibernateConfiguration(BusConfiguration busConfiguration)
    {
        #region SpecificNHibernateConfiguration

        Configuration nhConfiguration = new Configuration();
        nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseSubscriptionStorageConfiguration(nhConfiguration);
        persistence.UseGatewayDeduplicationConfiguration(nhConfiguration);
        persistence.UseTimeoutStorageConfiguration(nhConfiguration);
        #endregion

    }


    void CustomCommonConfigurationWarning(BusConfiguration busConfiguration)
    {
        #region CustomCommonNhibernateConfigurationWarning

        Configuration nhConfiguration = new Configuration();
        nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence, StorageType.GatewayDeduplication>();
        persistence.UseConfiguration(nhConfiguration);
        #endregion
    }
}