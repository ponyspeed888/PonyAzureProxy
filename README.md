This is a simple use for recently released YARP reverse proxy server. It implement code based proxy configuration 
so that you can set the target server address as environment variable.  An obvious use case for me is to proxy from azure
to my localhost, for example, if you need to access your localhost from Android emulator.  This code has been tested on
Azure WebApp

To try out, Set the environment (application, for Azure) variable YARPFORWARD_PREFIX to the url you want to proxy to.
If YARPFORWARD_PREFIX is not set or is whitespace only, then no proxy occured
Please note you may have to disable caching for you browser

![Visitor Count](https://profile-counter.glitch.me/{YOUR USER}/count.svg)
