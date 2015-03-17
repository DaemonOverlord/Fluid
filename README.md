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
**[Sending blocks](#sending-blocks)**                                                                                       
**[Players](#players)**                                                                                                     
**[Using Storage Providers](#using-storage-providers)**

###Getting started
#####Logging in
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

#####Joining a world
To join a world, all you need is a client that's logged in. You can use a world id or world url.

```c#

//Use logged in client
WorldConnection myWorldCon = client.GetWorldConnection("PWtGKa64_JbkI").Join();

```

#####Managing world events
After you've joined a world, you can now monitor the world's events

```c#
//Add your event handlers
myWorldCon.AddServerEventHandler<CrownEvent>(OnCrown);
```

And add your own code for each event! It's that simple.

```c#
public static void OnCrown(ConnectionBase c, CrownEvent e)
{ 
  //My code
}
```

#####Interacting with the world
At any point in time if you want to update your world you can use the WorldConnection to send events.

```c#
worldCon.SetTitle("Hello everybodyedits!");
```

See the documentation for more interactions.

#####Joining the lobby

With fluid, one of it's features is the ability to join the lobby.
To join the lobby you can use very simliar syntax as joining a world.

```c#

//Use logged in client
LobbyConnection myLobbyCon = client.GetLobbyConnection().Join();
```

Note that the lobby's functionalities differ per type of connection, specifically guest connections.
After you've joined the lobby you can fetch player profiles, spend energy, interact with friends and more.

#####Debugging feautures

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
private static void OnBlock(ConnectionBase c, BlockEvent eventMessage)
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

###Sending blocks

Sending blocks in fluid is really simple and handled extensively to provide a flexible codebase.
To start join a world as demonstrated in Getting Started.

```c#
worldCon.SendBlock(BlockIDS.Blocks.Basic.Black, 5, 10);
```

This code snippet will place a block a the coordinate 5, 10, in the foreground layer since the block BasicBlack is a foreground block. You will not need to worry about this. The speed of the upload is determined automatically by fluid based upon your connection to the servers. To override this simply provide another parameter, the lowest recommended speed in 10ms as everybodyedits does have a upload limit per connection. If you need to place more blocks in less than ~10ms then you can use multiple connections as another option.

```c#
//Upload a block with a delay of 15 milliseconds
worldCon.SendBlock(BlockIDS.Blocks.Basic.Black, 5, 10, 15);
```

Because fluid places these blocks in a queue, these methods are asynchronous, meaning the completion of the method does not guarantee the block was uploaded. To wait for all blocked queued to be uploaded you can use the UploadManager in the world connection.

```c#
//Will wait for all blocks to be sent
worldCon.Uploader.WaitForBlocks();

//... all blocks have been sent!
```

If you need to continue doing things but still want to know when the uploader is finished you can add an event to the uploader.

```c#

//Add finish event
worldCon.Uploader.OnQueueFinished += OnDone;

private static void OnDone(object sender, EventArgs e)
{
   //My code here
}

```

In everybodyedits some blocks cannot be just defined using an x and y coordinate and an id. To upload portals, doors, gates, and another complex blocks you must create them first.

```c#

//Create a new regular portal at 25, 3, rotated left, a source id of 1 and a portal target of 2
Portal portal = new Portal(BlockIDS.Action.Portals.Portal, 25, 3, Rotation.Left, 1, 2);

//Create a invisible portal at 8, 12, rotated right, a source id of 2 and a portal target of 0
Portal invisPortal = new Portal(BlockIDS.Action.Portals.InvisPortal, 8, 12, Rotation.Right, 2, 0);

//Upload the portal
worldCon.SendBlock(portal);

//Upload the invisible portal at a speed of 12ms
worldCon.SendBlock(invisPortal, 12);

```

###Players

In Fluid, when you need to access players you will need to get the PlayerManager. If you've joined a World, you can get the PlayerManager from your WorldConnection.

```c#
var players = connection.Players;
```

Most of the time, it will be easier to use the players directly without a variable.

In this example we will say hello to all the players in the room except ourselves (the bot). Please note that the bot is included in the list of connected players.

```c#
foreach (WorldPlayer player in con.Players)
{
    //Make sure the player is not us
    if (!player.IsConnectedPlayer)
    {
        con.Say("Hello " + player.Username);
    }
}
```

All information about each of the players can be accessed through each player. We can find out the player's position, conditions such as whether they have the crown, is flying, access level, potions they have active, velocity, connection type, and even what keys they are pressing.

#####Attaching information to players

Sometimes, you want your own information about each player specific to your bot. In fluid you can do this without having to create your own class around the player.

In this example, we will increment a score value every time the player gets a coin.

```c#
con.AddServerEventHandler<CoinEvent>(OnCoin);

public static void OnCoin(ConnectionBase connection, CoinEvent e)
{
    WorldPlayer p = e.Player;
    
    //In this scenario we have our own integer variable named "Score"
    int currentScore = p.Get<int>("Score");
    
    //Increment the score by 1
    p.Set<int>("Score", currentScore + 1);
}
```

And it's as simple as that. Please note that after your program exits your player information will not be saved. To learn how to save player data to hard memory skip to ***[Using Storage Providers](#using-storage-providers)***.
