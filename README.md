# HostMonitorService
This is a simple host monitoring service for a job interview

## Backend
The backend is written in C#, in the .NET Framework. It's a bit overengineered for such a simple application. I did this to demonstrate my ability to create a simple app that can grow organically with more features with minimal redesign. I chose to make most of my classes that weren't data objects into interface implementations to allow for effective testing. There's a TrackedHost class and a TrackingRegistry class that would allow for the frontend user to dynamically add and remove hosts on their own with only a few more implementation steps. 

The backend has two main files: Program.cs and Worker.cs. Program.cs handles much of the overall configurations, dependencies, and setup. Worker.cs is where most of the logic is being done. It leverages the other classes to poll whatever hosts are supposed to be polled and record statistics.

## Frontend
The frontend is in Angular. It's just a simple program that records updates from the backend and displays them in a table. There's a lot more that could be done with this: you could add a filtering service (easy to do since I've added the TrackedHost class to the backend), you could show uptime percentages and statistics, etc.

I let ChatGPT mostly make the front-end. It's too simple for me to spend much time on. 

## Running the application: 
You will need to have Angular, .NET, and all of their dependencies installed. Then, run make to start the backend, and ng serve to serve up the front-end in a development capacity, and voila! It should work. If you want to, you can change the arguments being passed into the backend through the makefile, or via the command line. 
