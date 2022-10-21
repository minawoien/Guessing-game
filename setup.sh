#!/bin/sh
cd ./frontend
npm ci
npm run buildRelease
cd ..
dotnet build
cd Backend
./dbsetup.sh
echo "Frontend is built, migrations has been run on backend and the database has been created"
echo "Run project from backend folder, the images will be imported on first startup"
