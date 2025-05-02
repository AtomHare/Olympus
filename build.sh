#!/bin/sh

mkdir -p love
dotnet restore sharp/*.csproj
dotnet publish sharp/*.csproj --runtime linux-x64 --self-contained

cp -rfv sharp/bin/Release/net8.0/linux-x64/publish love/sharp

cd love 
curl -O https://github.com/love2d/love/releases/download/11.5/love-11.5-x86_64.AppImage
cd ..
cp -a luarocks/lib/lua/5.1/. love/

cd src 
zip -r ../love/olympus.love *
