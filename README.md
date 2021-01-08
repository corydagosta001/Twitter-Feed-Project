# twitter-feed-project
emoji.json must be in the same directory as twitter.exe. 

This is a wpf project. The partial class that program flow starts out in is TwitterFeedDriver.
All objects used in project are instantiated in the TwitterFeedDriver constructor.
The last line of the constructor points the program flow to the method StartStreaming().
StartStreaming() has a timed event that fires off every 3 seconds.
Every 3 seconds the timed event is evoked the method ProcessFeed() is called.
ProcessFeed() is where all objects doing analasys are called.
