# Credit card payment system
Pub-sub system that accepts credit card payments via rest and then posts them to a rabbit message queue. Later they are consumed by another service and saved into database.

To run locally with docker-compose:
The api is setup to run with https. You need to generate a local self signed certificate named aspnet.pfx, and the password set to 'password'.
https://docs.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide

On docker-compose startup, the api and the processor are waiting on rabbitmq to be fully active before they start. This can take significant delay on startup.
After everything is fully started, the api is listening for https requests on https://localhost:5001/. Use /swagger for OpenApi interface.
