# Fluid
Fluid is a flexible, all-in-one SDK for Everybody Edits made with the programmer in mind.

###Install
If you're using visual studio, you can install via nuget.
```
PM> Install-Package Fluid
```

## Table of Contents
**[Getting started](#getting-started)**

**[All about events](#all-about-events)**

###Getting started
######Logging in
To use fluid you'll need to login to everybody edits. There are multiple ways of authentication you can
use to log in

```c#
using Fluid;

//Authentication examples
IAuth guestAuth = new GuestAuth();
IAuth eeAuth = new SimpleAuth("myEmail", "myEEPass");
IAuth kongAuth = new KongregateAuth("myKongUsername", "myKongPass");
IAuth armorAuth = new ArmorgamesAuth("myArmorUsername", "myArmorPass");

//Create client
FluidClient client = new FluidClient(myAuth);
bool loginSucess = client.LogIn();
```

######Joining a world
To join a world, all you need is a client that's logged in. You can use a world id or world url.

```c#

//Use logged in client
WorldConnection myWorldCon = client.GetWorldConnection("PWtGKa64_JbkI").Join();

```

######Managing world events
After you've joined a world, you can now monitor the world's events

```c#
//Add your event handlers
myWorldCon.AddServerEVentHandler<CrownEvent>(OnCrown);
```

And add your own code for each event! It's that simple.

```c#
public static void OnCrown(CrownEvent e)
{ 
  //My code
}
```

######Interacting with the world
At any point in time if you want to update your world you can use the WorldConnection to send events.

```c#
worldCon.SetTitle("Hello everybodyedits!");
```

See the documentation for more interactions.

######Joining the lobby

With fluid, one of it's features is the ability to join the lobby.
To join the lobby you can use very simliar syntax as joining a world.

```c#

//Use logged in client
LobbyConnection myLobbyCon = client.GetLobbyConnection().Join();
```

Note that the lobby's functionalities differ per type of connection, specifically guest connections.
After you've joined the lobby you can fetch player profiles, spend energy, interact with friends and more.

######Debugging feautures

Fluid provides a variety of ways to optimize your code. The best way to view these messages from the log is setting an output to the fluid logger. There are a variety of ways you can do this, but one of the most simple being when using a console application.

```c#
client.Log.Output = Console.Out;
```

You also have the option to set the output to a file.

```c#
client.Log.Output = File.CreateText("myLog.txt");
```

###All about events

Fluid provides a multiplitude of ways to handle events in worlds. In fluid there are two types of events, Events, and ServerEvents. ServerEvents represent any event that was received directly and contains a PlayerIO.Message. Events on the other hand are either custom events to help utilize fluid or events without PlayerIO.Message's. It is very important that you add your events before you call Join() for any of your connections. 

Also, note that events are stored in the namespaces Fluid.Events and Fluid.ServerEvents so adding these at the top of your code file might be easier for you to find your events quicker.

```c#
using Fluid.Events;
using Fluid.ServerEvents;
```

Now that we've referenced the event namespaces we can create the handlers.

```c#
//Adding a disconnection event handler
con.AddEventHandler<DisconnectEvent>(OnDisconnect);

//As a shortcut to creating the method, in visual studio right click on "OnDisconnect" and Generate->Method Stub.
private static void OnDisconnect(object sender, DisconnectEvent e)
{
    //My disconnection handling
}
```

Server events are very simliar in syntax to create also.

```c#
//Adding a block server event handler
con.AddServerEventHandler<BlockEvent>(OnBlock);

//Same tip can be used as metioned in the above example to create the method quicker.
private static void OnBlock(BlockEvent eventMessage)
{
    //Your block handling code
}
```

Server events are great, but sometimes it might be easier for you to handle similiar events in one handler. The syntax is just as similiar in the previous examples.

```c#
//Adding a block and portal event to one handler
//You can add as many events to one handler as you want
con.AddServerEventHandlers(BlockOrPortal, typeof(BlockEvent), typeof(PortalBlockEvent));

private static void BlockOrPortal(object sender, IServerEvent e)
{
    //Depending on what you've grouped you can grab the event information
    //By testing e for the event
    
    Fluid.Blocks.Block block = null;
    if (e is BlockEvent)
    {
        BlockEvent blockEvent = (BlockEvent)e;
        block = blockEvent.Block;
    }
    else if (e is PortalBlockEvent)
    {
        PortalBlockEvent portalEvent = (PortalBlockEvent)e;
        block = portalEvent.Portal;
    }
    
    //My handler code
}
```
