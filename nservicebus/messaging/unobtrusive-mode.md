---
title: Unobtrusive Mode Messages
summary: No reference any NServiceBus assemblies from message assemblies.
reviewed: 2016-03-21
tags: 
- Conventions
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/conventions
redirects:
- nservicebus/unobtrusive-mode-messages
- nservicebus/how-do-i-centralize-all-unobtrusive-declarations
- nservicebus/invalidoperationexception-in-unobtrusive-mode
---

Message contracts can be defined using plain classes or interfaces. For NServiceBus to find those classes when scanning assemblies they need to be marked with the `IMessage` interface, which essentially says, "this is a message definition". This allows decoupling message contracts from the NServiceBus assembly.

This dependency can cause problems when there are different services that run different versions of NServiceBus. Jonathan Oliver has a [great write up on this very subject](http://blog.jonathanoliver.com/nservicebus-distributing-event-schemacontract/).

This is not a big deal for commands because they are always used with in the boundary of a single service and it's fair to require a service to use the same version of NServiceBus. But when it comes to events, this becomes more of a problem since it requires the services to all use the same version of NServiceBus, thereby forcing them to upgrade NServiceBus all at once.


## The solution

There are a couple of ways this can be solved.

 * NServiceBus follows the [semver.org](http://semver.org/) semantics, only changing the assembly version when changes are not backwards compatible or introduce substantial new functionality or improvements. This mean that Version 3.0.1 and Version 3.0.X have the same assembly version (3.0.0), and file version of course changes for every release/build. This means that as long as NuGet updates are done with the -safe flag the service contracts will stay compatible.
 * Support for running in "Unobtrusive" mode means no reference to any NServiceBus assemblies is required from message assemblies, thereby removing the problem altogether.


## Unobtrusive mode

NServiceBus allows defining custom [message conventions](conventions.md) instead of using the `IMessage`, `ICommand` or `IEvent` interfaces and attributes like `TimeToBeReceivedAttribute` and `ExpressAttribute`. NServiceBus also supports conventions for encrypted properties, express messages, databus properties and time to be received. With these conventions combined a reference to NServiceBus can be avoided.

Note: It is important to note that in .NET namespace is optional and hence can be null. So if any conventions do partial string checks, for example using `EndsWith` or `StartsWith`, then a null check should be used. So include `.Namespace != null` at the start of the convention. Otherwise a null reference exception will occur during the type scanning.
