$path = ls .\bin\Debug\net6.0\mydb.db

cp $path .\web_client -Recurse -Force
cp $path .\desktop_client\bin\Debug\net6.0-windows -Recurse -Force
