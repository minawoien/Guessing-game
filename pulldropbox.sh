#!/bin/sh
if [[ -d "./Backend/Assets" ]]
then
    while true; do
        read -p "This folder already exists, download again? " yn
            case $yn in
                [Yy]* ) rm -rf "./Backend/Assets"; break;;
                [Nn]* ) exit;;
                * ) echo "Please answer yes or no.";;
            esac
        done
fi

mkdir "./Backend/Assets"

cd "./Backend/Assets"
wget -O images.zip "https://www.dropbox.com/sh/j80xn9prbw086wc/AAA3hCVfPAsHxNq1Ie0okL-Ta?dl=0"

unzip "images"
unzip "scattered images.zip"
ls
rm -rf "__MACOSX"
rm 'scattered images.zip'
rm 'images.zip'
rm 'Two-Player_Image_recognition_game.pdf'
cd "task 2 scattered images"
find . -type f -name "*.DS*" -delete
cd ".."
mv "task 2 scattered images" "img"
