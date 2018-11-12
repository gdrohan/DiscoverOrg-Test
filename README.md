I solved this in C# as that is what I am work with day-to-day, and I don't have a Java IDE at the moment. 
My second choice would have been javascript. While Java and C# are very similar, and I don't have any problem coding in it other than recalling the different names for APIs and certain unique aspects,
I would have spent much time setting up an environment if i wanted to actually verify and run the tests first.

Questions i would ask for further refinement:

Would the manager class manage multiple fridges? I'm assuming for this example there is only one fridge it manages for simplicity of the exercise,
but otherwise the manager would possibly manage many fridges, or else the dependency might be inverted (and the name changed from manager to a specific function)

The word 'manager' in the name could imply many things and would require further explanation as to it's purpose - 
is this just receiving Fridge messages and storing or displaying them to a console? Is it acting as a broker to other services (maybe the manager is communicating with a robot that will balance energy usage, stocking one fridge to capacity and shutting down another until replenishments arrive?
Here I'm assuming the manager is just a monitor for a single refrigerator. 

I've made an assumption that the fridge is managing capacity by ItemType, and not by 'item'. I'm thinking item type for this example is simply 'milk', 'eggs', etc, and the UUID is like a sku. Or perhaps Milk is a category and then
itemType relates to type and, for this task at least, size:- 1/2 gallon 2% being an itemType.

This affects how the fridge stores the item objects as opposed to what the manager knows: internally, the fridge can have dictionaries of types (not hard-coded, as the number of itemTypes may vary).
Also i'm assuming the 'name' is the name of the item type. but then i don't see that being used - if a human is reading this info then the name should be returned as well as the ItemType.

Regarding the comment about not using empty containers in the calculation, I didn't get that  - isn't there a single container for all itemTypes? 

StopTracking - I changed this to StopStocking - as i would think you would still track remaining inventory? Though not for fillFactor and replenishment so I left the logic but changed the name.

As the manager just cares about itemType and fillFactor, I considered changing the return type of GetItems to a Dictionary (HashMap) to map the key value pairs of (itemType, fillFactor) rather than an array of arrays.
But I don't see how this collection is used - if no lookups are required, than the array makes sense, but then it could still be an array of ItemTypes or ItemTypeState - then it would easy to add other useful properties rather than require refactoring of the code and tests.
I decided to go with an array of ItemTypeState, though that object is at the moment essentially a key-value pair.

Exceptions - there are places i'm throwing exceptions where it would be better to return status, but that would change return types further.

Thanks for the test! I've been working on the front end and crud for so long, it was nice to write some business logic and rediscover how to unit test events.
