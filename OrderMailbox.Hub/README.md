# OrderMailBox-Hub

## Api Key:
Name: "X-Api-Key"
Key format: "{ObjectIdentifier}:{CountryId}"

Example: "11dc38c7-7ff7-48fe-8567-084047d96041:34"

**AES Encrypt & Decrypt Online Tool:**
* https://encode-decode.com/aes-128-ecb-encrypt-online/
  * aes-128-ecb
* https://randomkeygen.com/
  * 152-bit WEP Keys

## Images

* docker images
* docker rmi $(docker images -q)
* docker build -t boilerplate.image .

## Containers

* docker ps -a
* docker rm $(docker ps -a -q)
* docker run -p 52516:80 boilerplate.image
* docker stop 0fe505610ddb
* docker start 0fe505610ddb
* docker exec -it 0fe505610ddb sh
* docker logs -f 0fe505610ddb > c:\tmp\boilerplate.image.log

## Volumes (C:\Users\Public\Documents\Hyper-V\Virtual hard disks)

* docker volume ls
* docker inspect boilerplate_eslocal
* docker volume rm boilerplate_eslocal
* docker volume prune

## dotnet

* dotnet restore
* dotnet publish
* dotnet build
* dotnet run
* dotnet watch run
* dotnet test
* dotnet watch test
* dotnet watch test /p:CollectCoverage=true /p:Exclude="[xunit*]*"

## Docker compose

* docker-compose up -d
* docker-compose down
* docker-compose start
* docker-compose stop

## nuget

* <https://pkgs.dev.azure.com/TelepizzaIT/_packaging/Sales.Seedwork/nuget/v3/index.json>
* .\NuGet.exe sources Add -Name "Sales.Seedwork" -Source "https://pkgs.dev.azure.com/TelepizzaIT/_packaging/Sales.Seedwork/nuget/v3/index.json" -UserName "vectordesarrollo@telepizza.com" -Password "z5j4zr6s7d5jwwt6jcny4hodkhjnko5qdtu6vcdjn3n52dm4i5fa" -StorePasswordInClearText
