FROM microsoft/dotnet:2.0.0-runtime-deps

WORKDIR /app
COPY ubuntu.16.04-x64/publish /app
COPY run.sh /app

# RUN dotnet restore
# RUN dotnet build

EXPOSE 5000/tcp
# ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["sh", "/app/run.sh"]
