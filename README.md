# Fluid
Fluid is a flexible, all-in-one SDK for Everybody Edits made with the programmer in mind.

###Install
```
PM> Install-Package Fluid
```

###Getting started
######Logging in
To use fluid you'll need to login to everybody edits. There are multiple ways of authentication you can
use to log in

```cshsarp
using Fluid;

IAuth eeAuth = new SimpleAuth("myEmail", "myEEPass");

IAuth kongAuth = new KongregateAuth("myKongUsername", "myKongPass");

IAuth armorAuth = new ArmorgamesAuth("myArmorUsername", "myArmorPass");

FluidClient client = new FluidClient(myAuth);

bool loginSucess = client.LogIn();

```
