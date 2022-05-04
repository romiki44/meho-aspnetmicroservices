cd $PSScriptRoot
Set-PSDebug -Trace 1
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build