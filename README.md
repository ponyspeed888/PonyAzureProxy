This is a simple use for recently released YARP reverse proxy server. It implement code based proxy configuration 
so that you can set the target server address as environment variable.  An obvious use case for me is to proxy from azure
to my localhost, for example, if you need to access your localhost from Android emulator.  This code has been tested on
Azure WebApp

To try out, Set the environment (application, for Azure) variable YARPFORWARD_PREFIX to the url you want to proxy to.
If YARPFORWARD_PREFIX is not set or is whitespace only, then no proxy occured
Please note you may have to disable caching for you browser

[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fponyspeed888%2FPonyAzureProxy&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)

