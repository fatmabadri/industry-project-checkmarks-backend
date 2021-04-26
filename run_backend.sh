#!/bin/sh

##### SCRIPT TO RUN THE BACK END

# run this from the backend root folder
# make sure to give exec permission using command
#	prompt> chmod a+x run_backend.sh
#	prompt> ./run_backend.sh

# webapi
dotnet run --urls https://0.0.0.0:5001 --project ./CheckmarksWebApi >> cm-api.log &

# service to update DB
#dotnet run --project ./CheckmarksService >>  cm-service.log &

# done