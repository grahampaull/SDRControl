# SDR Control for remote management of the SDRSharp client using telnet

## Get Started

Please ensure you have Telnet installed on your machine. Run the SDPSharp client and tick 'Network' option and ensure port is 3382.

You can now use the Web UI or CLI to interact with the client.

## Run the Web UI

From the SDRControl.Web folder, run the following

````
    dotnet run
````

It will start the web server and display the url to access via your browser

## Install CLI Tool

Nothing on NuGet as yet, so to install the CLI tool, run the following commands from the SDRControl.Tool folder:

````
    dotnet pack
    dotnet tool install --global --add-source ./nupkg SdrControl.Tool
````

If you make changes to the CLI tool, you can run the following commands to pack and re-install

````
    dotnet pack
    dotnet tool update --global --add-source ./nupkg SdrControl.Tool
````

Once installed you can control the client using commands as follows:

````
    sdr start
    sdp isplaying
    sdr stop
````

To run a preset use the 'play' command

````
    sdr play --preset radio1
````