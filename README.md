# TrafficLights
Code for an application that simulates a set of traffic lights at an intersection. The traffic lights are designated (N, S) and (E, W) like a compass.

# Requirements
- The lights will change automatically every 5 minutes.
- When switching from green to red, the yellow light must be displayed for 30 seconds prior to
it switching to red.
- You're not required to optimize the solution, just focus on a functional approach to
requirements.

# Intro
- Create new intersection:
```csharp
Intersection intersection = new Intersection( IntervalOfDirection );
```
- Create needed traffic lights:
```csharp
TrafficLight nsLight = new TrafficLight( intersection, IntervalOfYellow, Direction.NS );
TrafficLight ewLight = new TrafficLight( intersection, IntervalOfYellow, Direction.EW );
```
- ~~Light the fire~~ Switch intersection on:
```csharp
intersection.Start();
```
- When you are done, switch it off like this:
```csharp
intersection.Stop();
```
- Constants to control time intervals for various events:
```csharp
// Use TimeCoefficient to manipulate time scale in application;        

// Use this for honest 30 mintues simulation
public const int TimeCoefficient = 1000;        

// Use this for speeded up simulation
//public const int TimeCoefficient = 10;

// Interval of switching directions NS -> EW -> NS and so on.
public const int IntervalOfDirection = 5 * 60 * TimeCoefficient;

// How long will yellow be shown, during switch Green -> Red.
public const int IntervalOfYellow = 30 * TimeCoefficient;

// How long to wait before stoping simulation
public const int TimeToSimulate = 30 * 60 * TimeCoefficient;
```

# Ideas behind implementation
- There was a need in a single point of syncronization so an orchestration role "IDirectionSwitcher" appeared as a simple way to achieve this.
- This "IDirectionSwitcher" should know nothing about actual lights (red, yellow, green) and their pattern. Its role is to make sure, traffic is going only in 1 direction at a time - NS or EW. As so, it switches directions, not specific lights of traffic lights.
- With this, logic of specific pattern for lights changes, naturally went to "ITrafficLight". I used IoC so its traffic light, who checks the intersection. This way, we can avoid a situation when all the logic of all different traffic lights of the intersection sits in one class (intersection) and makes it very hard to understand and modify.
- While "IDirectionSwitcher" is for "ITrafficLight", "IIntersection" is for outside user, who wants to control the system. These 2 interfaces serv absolutely different roles so I avoided joining them into single entity.
- As I was asked not to optimize solution, current way of holding state of lights in TrafficLight is simple and naive. If system would get any more complex, I would change this approach to allow more flexible pattern of which lights are in TrafficLight at all and how do they behave. Probably there will be a place for one more IoC so that each light controlls itself based on the state of the TrafficLight in general.
- Another issue - currently, if intersection stops working, traffic lights will remain in whatever state they are. Its intended as solution for current requirements but I would like to give them a bit of logic on checking such situations. For example, if after anounced to TrafficLight time, direction switch had not occured, TrafficLight can go into "panic" mode switching to red and starting blinking yellow, to show that something weird is going on. 
