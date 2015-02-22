# Fluid
Fluid is a flexible, all-in-one SDK for Everybody Edits made with the programmer in mind.

###Install
If you're using visual studio, you can install via nuget.
```
PM> Install-Package Fluid
```

### Table of Contents
**[Getting started](#getting-started)**

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
WorldConnection myWorldCon = client.JoinWorld("PWtGKa64_JbkI");

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
LobbyConnection myLobbyCon = client.JoinLobby();
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
