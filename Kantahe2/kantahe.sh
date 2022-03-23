#!/bin/bash

/home/kantahe/.dotnet/dotnet /home/kantahe/kantahe2/Kantahe2.dll
sleep 10
/usr/bin/chrome-browser --disable-gpu --autoplay-policy=no-user-gesture-required --kiosk http://localhost/qrcode